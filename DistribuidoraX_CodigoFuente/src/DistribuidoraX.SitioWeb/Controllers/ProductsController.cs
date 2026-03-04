using DistribuidoraX.Shared.Dtos.GenericDtos;
using DistribuidoraX.Shared.Dtos.ProductDtos;
using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using DistribuidoraX.Shared.Dtos.SupplierDtos;
using DistribuidoraX.Shared.Services.ProductServices;
using DistribuidoraX.Shared.Services.ProductSupplierServices;
using DistribuidoraX.Shared.Services.SupplierServices;
using DistribuidoraX.Shared.Services.TypeProductServices;
using DistribuidoraX.SitioWeb.Models.CustomerErrorsModels.ViewModels;
using DistribuidoraX.SitioWeb.Models.GenericModels;
using DistribuidoraX.SitioWeb.Models.ProductModels;
using DistribuidoraX.SitioWeb.Models.ProductModels.ViewModels;
using DistribuidoraX.SitioWeb.Models.ProductSuplierModels;
using DistribuidoraX.SitioWeb.Models.ProductSuplierModels.ViewModels;
using DistribuidoraX.SitioWeb.Models.SupplierModels;
using DistribuidoraX.SitioWeb.Models.SupplierModels.ViewModels;
using DistribuidoraX.SitioWeb.Models.TypeProductModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Text.RegularExpressions;

namespace DistribuidoraX.SitioWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductQueriesService _productQueriesService;
        private readonly ITypeProductQueriesService _typeProductQueriesService;
        private readonly IProductCommandsService _productCommandsService;
        private readonly IProductSupplierQueriesService _productSupplierQueriesService;
        private readonly ISupplierQueriesService _supplierQueriesService;
        private readonly IValidator<ProductFullDataDto> _productFullDataDtoValidator;
        private readonly IValidator<SupplierProductDto> _supplierProductDtoValidator;
        public ProductsController(IProductQueriesService productQueriesService, ITypeProductQueriesService typeProductQueriesService, IProductCommandsService productCommandsService, IProductSupplierQueriesService productSupplierQueriesService, ISupplierQueriesService supplierQueriesService, IValidator<ProductFullDataDto> productFullDataDtoValidator, IValidator<SupplierProductDto> supplierProductDtoValidator)
        {
            _productQueriesService = productQueriesService;
            _typeProductQueriesService = typeProductQueriesService;
            _productCommandsService = productCommandsService;
            _productSupplierQueriesService = productSupplierQueriesService;
            _supplierQueriesService = supplierQueriesService;
            _productFullDataDtoValidator = productFullDataDtoValidator;
            _supplierProductDtoValidator = supplierProductDtoValidator;
        }
        public async Task<IActionResult> Index()
        {
            ProductsViewModel productsViewModel = new();
            ProductsTableViewModel productsTableModel = new();
            List<TypeProductModel> typeProductList = [];
            List<ProductModel> productList = [];

            try
            {
                var typeProductApiResponse = await _typeProductQueriesService.GetListAsync();
                if (typeProductApiResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    if (typeProductApiResponse.Value != null)
                    {
                        foreach (var typeProduct in typeProductApiResponse.Value)
                        {
                            typeProductList.Add(new TypeProductModel
                            {
                                TypeProductItem = typeProduct.TypeProductItem,
                                TypeProductName = typeProduct.TypeProductName,
                            });
                        }
                    }
                }
                else
                {
                    productsViewModel.TypeProductMessageError = "El servicio no responde como se esperaba: Consulte al administrador del aplicativo.";
                }

                var productsApiResponse = await _productQueriesService.GetListByFiltersAsync(new SearchProductsFiltersBaseDto());
                if (productsApiResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    if (productsApiResponse.Value != null)
                    {
                        foreach (var product in productsApiResponse.Value)
                        {
                            productList.Add(new ProductModel
                            {
                                ProductItem = product.ProductItem,
                                ProductName = product.ProductName,
                                ProductCode = product.ProductCode,
                                ProductPrice = product.ProductPrice
                            });
                        }
                    }

                }
                else
                {
                    productsTableModel.ProductListErrorMessage = "El servicio no responde como se esperaba: Consulte al administrador del aplicativo.";
                }

                productsViewModel.TypeProductList = typeProductList;
                productsTableModel.ProductList = productList;
                productsViewModel.ProductsTableModel = productsTableModel;

                return View(productsViewModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);            
            }
            catch (Exception ex) 
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts([AsParameters] SearchProductsFiltersModel filters)
        {
            SearchProductsFiltersBaseDto searchProductsFiltersBaseDto = new()
            {
                ProductCodeFilter = filters.ProductCode ?? null,
                TypeProductFilter = filters.TypeProductItem > 0 ? filters.TypeProductName : null
            };

            ProductsTableViewModel productsTableViewModel = new();
            List<ProductModel> productList = [];
            try
            {
                var productsApiResponse = await _productQueriesService.GetListByFiltersAsync(searchProductsFiltersBaseDto);
                if (productsApiResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    if (productsApiResponse.Value != null)
                    {
                        foreach (var product in productsApiResponse.Value)
                        {
                            productList.Add(new ProductModel
                            {
                                ProductItem = product.ProductItem,
                                ProductName = product.ProductName,
                                ProductCode = product.ProductCode,
                                ProductPrice = product.ProductPrice
                            });
                        }
                    }
                }
                else
                {
                    productsTableViewModel.ProductListErrorMessage = "El servicio no responde como se esperaba: Consulte al administrador del aplicativo.";
                }

                productsTableViewModel.ProductList = productList;
                return PartialView("_ProductsTablePartialView", productsTableViewModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByConfirmDelete(int item)
        {
            try
            {
                var productApiResponse = await _productQueriesService.ProductByIdAsync(item);
                DeleteProductViewModel deleteProductViewModel = new();
                if (productApiResponse.Value != null)
                {
                    deleteProductViewModel.ItemProduct = productApiResponse.Value.ProductItem;
                    deleteProductViewModel.NameProduct = productApiResponse.Value.ProductName;
                    deleteProductViewModel.CodeProduct = productApiResponse.Value.ProductCode;
                    deleteProductViewModel.PriceProduct = productApiResponse.Value.ProductPrice;
                }

                return PartialView("_DeleteProductPartialView", deleteProductViewModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }

        }

        

        [HttpDelete]
        public async Task<JsonResult> DeleteProduct(int item)
        {
            try
            {
                var productDeletedApiResponse = await _productCommandsService.DeleteProductoAsync(item);
                return Json(new { isSuccess = productDeletedApiResponse.Value, message = productDeletedApiResponse.Message });
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return JsonResult_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return JsonResult_CustomerExceptionHandler(string.Empty, ex.HResult);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddUpdateProduct(int id)
        {
            ProductSupplierViewModel productSupplierViewModel = new();
            List<TypeProductModel> typeProductList = [];
            List<ProductSupplierModel> productSupplierList = [];

            try
            {
                if (id == 0)
                {
                    var typeProductApiResponse = await _typeProductQueriesService.GetListAsync();
                    if (typeProductApiResponse.Value != null)
                    {
                        foreach (var typeProduct in typeProductApiResponse.Value)
                        {
                            typeProductList.Add(new TypeProductModel
                            {
                                TypeProductItem = typeProduct.TypeProductItem,
                                TypeProductName = typeProduct.TypeProductName,
                            });
                        }
                    }
                }


                if (id > 0)
                {
                    var productSupplierApiResponse = await _productSupplierQueriesService.GetListByProductIdAsync(id);
                    if (productSupplierApiResponse != null)
                    {
                        if (productSupplierApiResponse.Value != null)
                        {
                            foreach (var typeProduct in productSupplierApiResponse.Value.TypeProductList!)
                            {
                                typeProductList.Add(new TypeProductModel
                                {
                                    TypeProductItem = typeProduct.TypeProductItem,
                                    TypeProductName = typeProduct.TypeProductName,
                                });
                            }

                            foreach (var productSupplier in productSupplierApiResponse.Value.ProductSupplierList!)
                            {
                                productSupplierList.Add(new ProductSupplierModel
                                {
                                    ProductSupplierItem = productSupplier.SupplierProductItem,
                                    ProductSupplierCode = productSupplier.SupplierProductCode,
                                    ProductSupplierCost = productSupplier.SupplierProductCost,
                                    SupplierItem = productSupplier.SupplierItem,
                                    SupplierName = productSupplier.SupplierName
                                });
                            }

                            productSupplierViewModel.ProductItem = productSupplierApiResponse.Value.ProductItem;
                            productSupplierViewModel.ProductName = productSupplierApiResponse.Value.ProductName!.TrimEnd();
                            productSupplierViewModel.ProductCode = productSupplierApiResponse.Value.ProductCode!.TrimEnd();
                            productSupplierViewModel.ProductActive = productSupplierApiResponse.Value.ProductActive;
                            productSupplierViewModel.TypeProductId = productSupplierApiResponse.Value.TypeProductId;
                        }
                    }

                }

                productSupplierViewModel.TypeProductList = typeProductList;
                productSupplierViewModel.ProductSupplierList = productSupplierList;
                return PartialView("_ProductSupplierPartialView", productSupplierViewModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ShowAddUpdateNewSupplier([FromBody] ProductSupplierMemoryDataModel productSupplierData)
        {
            SupplierViewModel supplierViewModel = new();
            List<SupplierModel> SupplierList = [];

            try
            {
                var supplierLisApiResponse = await _supplierQueriesService.GetListAsync();
                if (supplierLisApiResponse != null)
                {
                    foreach (var supplier in supplierLisApiResponse.Value!)
                    {
                        SupplierList.Add(new SupplierModel
                        {
                            SupplierItem = supplier.SupplierItem,
                            SupplierName = supplier.SupplierName,
                        });
                    }
                }
                supplierViewModel.SuppliersList = SupplierList;

                if (productSupplierData.Id != 0)
                {
                    if (productSupplierData.ProductSupplierList!.Count > 0)
                    {
                        var proveedorProducto = productSupplierData.ProductSupplierList!.Find(p => p.ProductSupplierItem == productSupplierData.Id);
                        if (SupplierList.Count > 0)
                        {
                            var proveedor = SupplierList.Find(p => p.SupplierName == proveedorProducto!.SupplierName);
                            supplierViewModel.SupplierItem = proveedor != null ? proveedor.SupplierItem : 0;
                        }

                        supplierViewModel.ProductSupplierItem = proveedorProducto!.ProductSupplierItem;
                        supplierViewModel.SupplierName = proveedorProducto!.SupplierName;
                        supplierViewModel.ProductSupplierCode = proveedorProducto!.ProductSupplierCode;
                        supplierViewModel.ProductSupplierCost = proveedorProducto!.ProductSupplierCost;
                    }
                }

                return PartialView("_SupplierPartialView", supplierViewModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSupplier_UpdateProductSupplierTable([FromBody] ProductSupplierUpdateMemoryDataModel productSupplierData)
        {
            List<ProductSupplierModel> producSupplierListModel = [];
            List<SupplierModel> SupplierList = [];

            try
            {
                var supplierLisApiResponse = await _supplierQueriesService.GetListAsync();
                if (supplierLisApiResponse != null)
                {
                    foreach (var supplier in supplierLisApiResponse.Value!)
                    {
                        SupplierList.Add(new SupplierModel
                        {
                            SupplierItem = supplier.SupplierItem,
                            SupplierName = supplier.SupplierName,
                        });
                    }
                }

                if (productSupplierData.ProductSupplierList!.Count != 0)
                {
                    //Update SupplierItem on ProductSupplierList
                    foreach (var productSupplier in productSupplierData.ProductSupplierList!)
                    {
                        if (SupplierList.Count > 0)
                        {
                            var supplierObj = SupplierList!.Find(p => p.SupplierName == productSupplier.SupplierName);
                            productSupplier.SupplierItem = supplierObj != null ? supplierObj!.SupplierItem : productSupplier.SupplierItem;
                        }

                        producSupplierListModel.Add(productSupplier);
                    }

                    var lastProductSupplier = producSupplierListModel.Last();
                    if (lastProductSupplier.ProductSupplierItem > 0)
                        productSupplierData.ProductSupplierUpdateData!.ProductSupplierItem = -1;

                    if (lastProductSupplier.ProductSupplierItem < 0)
                        productSupplierData.ProductSupplierUpdateData!.ProductSupplierItem = lastProductSupplier.ProductSupplierItem - 1;

                    producSupplierListModel.Add(productSupplierData.ProductSupplierUpdateData!);
                }
                else
                {
                    productSupplierData.ProductSupplierUpdateData!.ProductSupplierItem = -1;
                    producSupplierListModel.Add(productSupplierData.ProductSupplierUpdateData!);
                }

                return PartialView("_ProductSuppliersTablePartialView", producSupplierListModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateSupplier_UpdateProductSupplierTable([FromBody] ProductSupplierUpdateMemoryDataModel productSupplierData)
        {
            List<ProductSupplierModel> producSupplierListModel = [];
            List<SupplierModel> SupplierList = [];

            try
            {
                var supplierLisApiResponse = await _supplierQueriesService.GetListAsync();
                if (supplierLisApiResponse != null)
                {
                    foreach (var supplier in supplierLisApiResponse.Value!)
                    {
                        SupplierList.Add(new SupplierModel
                        {
                            SupplierItem = supplier.SupplierItem,
                            SupplierName = supplier.SupplierName,
                        });
                    }
                }

                foreach (var productSupplier in productSupplierData.ProductSupplierList!)
                {
                    if (SupplierList.Count > 0)
                    {
                        var supplierObj = SupplierList!.Find(p => p.SupplierName == productSupplier.SupplierName);
                        productSupplier.SupplierItem = supplierObj != null ? supplierObj!.SupplierItem : productSupplier.SupplierItem;
                    }

                    if (productSupplierData.ProductSupplierUpdateData != null)
                    {
                        if (productSupplier.ProductSupplierItem == productSupplierData.ProductSupplierUpdateData!.ProductSupplierItem)
                        {
                            productSupplier.SupplierName = productSupplierData.ProductSupplierUpdateData!.SupplierName;
                            productSupplier.ProductSupplierCode = productSupplierData.ProductSupplierUpdateData!.ProductSupplierCode;
                            productSupplier.ProductSupplierCost = productSupplierData.ProductSupplierUpdateData!.ProductSupplierCost;
                        }
                    }

                    producSupplierListModel.Add(productSupplier);
                }

                return PartialView("_ProductSuppliersTablePartialView", producSupplierListModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }

        [HttpPut]
        public IActionResult SuppliertByConfirmDelete([FromBody] ProductSupplierMemoryDataModel productSupplierData)
        {
            ProductSupplierModel? productSupplierModel = new();
            if (productSupplierData != null)
            {
                if (productSupplierData.ProductSupplierList != null)
                {
                    productSupplierModel = productSupplierData.ProductSupplierList!.Find(p => p.ProductSupplierItem == productSupplierData.Id);
                }
            }
            
            return PartialView("_DeleteProductSupplierPartialView", productSupplierModel);
        }

        [HttpPut]
        public async Task<IActionResult> deleteSupplier_UpdateProductSupplierTable([FromBody] ProductSupplierMemoryDataModel productSupplierData)
        {
            List<ProductSupplierModel> producSupplierListModel = [];
            List<SupplierModel> SupplierList = [];

            try
            {
                var supplierLisApiResponse = await _supplierQueriesService.GetListAsync();
                if (supplierLisApiResponse != null)
                {
                    foreach (var supplier in supplierLisApiResponse.Value!)
                    {
                        SupplierList.Add(new SupplierModel
                        {
                            SupplierItem = supplier.SupplierItem,
                            SupplierName = supplier.SupplierName,
                        });
                    }
                }

                if (productSupplierData != null)
                {
                    foreach (var productSupplier in productSupplierData.ProductSupplierList!)
                    {
                        if (SupplierList.Count > 0)
                        {
                            var supplierObj = SupplierList!.Find(p => p.SupplierName == productSupplier.SupplierName);
                            productSupplier.SupplierItem = supplierObj != null ? supplierObj!.SupplierItem : productSupplier.SupplierItem;
                        }
                        producSupplierListModel.Add(productSupplier);
                    }

                    if (productSupplierData.Id != 0)
                    {
                        producSupplierListModel.RemoveAll(p => p.ProductSupplierItem == productSupplierData.Id);
                    }
                }

                return PartialView("_ProductSuppliersTablePartialView", producSupplierListModel);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return RedirectToAction_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return RedirectToAction_CustomerExceptionHandler(string.Empty, ex.HResult, string.Empty);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveProductSupplierFullData([FromBody] ProductSupplierFullDataModel productSupplierData)
        {
            ProductFullDataDto productFullDataDto = new();

            try
            {
                if (productSupplierData.ProductData != null)
                {
                    ProductFullDataDto productDataDto = new()
                    {
                        ProductItem = productSupplierData.ProductData.ProductItem,
                        ProductCode = productSupplierData.ProductData.ProductCode,
                        ProductName = productSupplierData.ProductData.ProductName,
                        ProductActive = productSupplierData.ProductData.ProductActive,
                        TypeProductItem = productSupplierData.ProductData.TypeProductItem
                    };

                    var validationResult = await _productFullDataDtoValidator.ValidateAsync(productDataDto);
                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors;
                        Log.Error("GenericMessage: Ocurrio un error de validacion con los datos del producto.| ValidationError: {ErrorMessage}", errors[0].ErrorMessage);
                        if (errors[0].PropertyName.Equals(nameof(productDataDto.ProductItem)))
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                propertyName = string.Empty,
                                validationError = string.Empty,
                                message = $"Ocurrio el siguiente error de validacion: {errors[0].ErrorMessage}."
                            });
                        }

                        string propertyNameString = errors[0].PropertyName;
                        return Json(new
                        {
                            isSuccess = false,
                            propertyName = char.ToLower(propertyNameString[0]) + propertyNameString.Substring(1),
                            validationError = errors[0].ErrorMessage,
                            message = "Ocurrio uno o mas errores de validacion."
                        });
                    }

                    productFullDataDto = productDataDto;
                }

                List<SupplierProductDto> supplierProductList = [];
                if (productSupplierData.ProductSupplierList != null)
                {
                    var supplierLisApiResponse = await _supplierQueriesService.GetListAsync();
                    List<SupplierDto> supplierList = [];
                    if (supplierLisApiResponse != null)
                    {
                        if (supplierLisApiResponse.Value != null)
                            supplierList = supplierLisApiResponse.Value;
                    }

                    foreach (var productSupplier in productSupplierData.ProductSupplierList)
                    {
                        SupplierDto? supplierObj = new();
                        if (supplierList.Count > 0)
                        {
                            supplierObj = supplierList.Find(p => p.SupplierName == productSupplier.SupplierName);
                        }


                        SupplierProductDto productSupplierDto = new()
                        {
                            SupplierProductItem = productSupplier.ProductSupplierItem < 0 ? 0 : productSupplier.ProductSupplierItem,
                            SupplierProductCode = productSupplier.ProductSupplierCode,
                            SupplierProductCost = productSupplier.ProductSupplierCost,
                            SupplierItem = supplierObj != null ? supplierObj!.SupplierItem : productSupplier.SupplierItem,
                            SupplierName = productSupplier.SupplierName

                        };

                        var validationResult = await _supplierProductDtoValidator.ValidateAsync(productSupplierDto);
                        if (!validationResult.IsValid)
                        {
                            var errors = validationResult.Errors;
                            Log.Error("Ocurrio un error de validacion con el proveedor con clave {SupplierProductCode} | ValidationError: {ErrorMessage}", productSupplierDto.SupplierProductCode, errors[0].ErrorMessage);
                            return Json(new
                            {
                                isSuccess = false,
                                propertyName = string.Empty,
                                validationError = string.Empty,
                                message = $"Ocurrio un error de validacion con el proveedor con clave {productSupplierDto.SupplierProductCode}: {errors[0].ErrorMessage}."
                            });
                        }

                        supplierProductList.Add(productSupplierDto);
                    }
                }

                ProductSupplierRequestDto productSupplierRequestDto = new()
                {
                    ProductDataDto = productFullDataDto,
                    ProductSupplierListDto = supplierProductList
                };

                if (productSupplierRequestDto.ProductDataDto.ProductItem == 0)
                {
                    var saveProductApiResponse = await _productCommandsService.SaveProductAsync(productSupplierRequestDto);
                    if (saveProductApiResponse != null)
                    {
                        if (saveProductApiResponse.Value)
                        {
                            return Json(new { isSuccess = saveProductApiResponse.Value, message = "La operacion fue realizada satisfactoriamente." });
                        }
                        else
                        {
                            Log.Error("Service:SaveProduct | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message} | Error: {Error}", saveProductApiResponse.StatusCode, saveProductApiResponse.Message, saveProductApiResponse.Error);
                            return Json(new
                            {
                                isSuccess = false,
                                propertyName = string.Empty,
                                validationError = string.Empty,
                                message = $"Ocurrio un error: {saveProductApiResponse.Error}."
                            });
                        }
                    }
                }
                else
                {
                    var saveProductApiResponse = await _productCommandsService.UpdateProductAsync(productSupplierRequestDto, productSupplierRequestDto.ProductDataDto.ProductItem);
                    if (saveProductApiResponse != null)
                    {
                        if (saveProductApiResponse.Value)
                        {
                            return Json(new { isSuccess = saveProductApiResponse.Value, message = "La operacion fue realizada satisfactoriamente." });
                        }
                        else
                        {
                            Log.Error("Service:UpdateProduct | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message} | Error: {Error}", saveProductApiResponse.StatusCode, saveProductApiResponse.Message, saveProductApiResponse.Error);
                            return Json(new
                            {
                                isSuccess = false,
                                propertyName = string.Empty,
                                validationError = string.Empty,
                                message = $"Ocurrio un error: {saveProductApiResponse.Error}."
                            });
                        }
                    }
                }

                return Json(new { isSuccess = false, message = "La operacion se completo parcialmente: favor de contactar al administrador del aplicativo." });
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return JsonResult_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return JsonResult_CustomerExceptionHandler(string.Empty, ex.HResult);
            }
        }

        [HttpGet]
        public IActionResult ConfirmCancelProductOperationShowModal()
        {
            return PartialView("_ConfirmCancelOperationPartialView");
        }

        [HttpGet]
        public IActionResult ConfirmAcceptProductOperationShowModal()
        {
            return PartialView("_ConfirmAcceptOperationPartialView");
        }


        //Fields validations
        [HttpGet]
        public JsonResult ProductCodeFilterValidate(string? productCodeFilter)
        {
            if (productCodeFilter != null)
            {
                if (!productCodeFilter.All(char.IsLetterOrDigit))
                {
                    Log.Warning("El campo-filtro Clave sólo acepta caracteres alfanuméricos.");
                    return Json(new { isError = true, message = "Sólo acepta caracteres alfanuméricos." });
                }
            }

            return Json(new { isError = false, message = "" });
        }

        [HttpGet]
        public JsonResult AmountFieldValidate(string? paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return Json(new { isError = true, message = "El campo es requerido." });
            }
            else
            {
                if (!Regex.IsMatch(paramValue, @"^[0-9 \.]+$"))
                {
                    return Json(new { isError = true, message = "Sólo acepta digitos y punto decimal." });
                }
                else
                {
                    if (!Regex.IsMatch(paramValue, @"^\d{1,7}(\.\d{1,2})?$"))
                    {
                        return Json(new { isError = true, message = "No coincide con el patron 0000000.00" });
                    }
                }
            }

            return Json(new { isError = false, message = "" });
        }

        [HttpGet]
        public JsonResult ProductSupplierCostFieldValidate(string? paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return Json(new { isError = true, message = "El campo es requerido." });
            }
            else
            {
                if (!Regex.IsMatch(paramValue, @"^[0-9 \.]+$"))
                {
                    return Json(new { isError = true, message = "Sólo acepta digitos y punto decimal." });
                }
                else
                {
                    if (!Regex.IsMatch(paramValue, @"^\d{1,9}(\.\d{1,2})?$"))
                    {
                        return Json(new { isError = true, message = "No coincide con el patron 000000000.00" });
                    }
                }
            }

            return Json(new { isError = false, message = "" });
        }


        [HttpPost]
        public async Task<JsonResult> ProductCodeFieldValidate([FromBody] ExistCodeParametersModel existCodeParametersModel)
        {
            try
            {
                if (string.IsNullOrEmpty(existCodeParametersModel.CodeValue))
                {
                    Log.Warning("El campo Clave Producto es requerido.");
                    return Json(new { isError = true, message = "El campo es requerido." });
                }
                else
                {
                    if (!Regex.IsMatch(existCodeParametersModel.CodeValue, @"^[a-zA-Z0-9\-]*$"))
                    {
                        Log.Warning("El campo Clave Producto sólo acepta caracteres alfanuméricos.");
                        return Json(new { isError = true, message = "Sólo acepta caracteres alfanuméricos." });
                    }

                    if (existCodeParametersModel.CodeValue.Length > 30)
                    {
                        Log.Warning("El campo Clave Producto no debe rebasar los 30 caracteres.");
                        return Json(new { isError = true, message = "No rebasar los 30 caracteres." });
                    }

                    var existProductCode_ApiResponse = await _productQueriesService
                                                                    .ExistProductCodeAsync(
                                                                                    new ExistCodeParametersDto
                                                                                    {
                                                                                        ItemId = existCodeParametersModel.Item,
                                                                                        CodeValue = existCodeParametersModel.CodeValue
                                                                                    });

                    if (existProductCode_ApiResponse != null)
                    {
                        if (existProductCode_ApiResponse.StatusCode == (int)HttpStatusCode.OK)
                        {
                            if (existProductCode_ApiResponse.Value)
                            {
                                Log.Warning("La Clave Producto ya existe.");
                                return Json(
                                            new
                                            {
                                                isError = existProductCode_ApiResponse.Value,
                                                message = "La clave ya existe."
                                            });
                            }
                        }
                        else
                        {
                            if (existProductCode_ApiResponse.Error != null)
                            {
                                Log.Error("Service:ExistProductCode | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message} | Error: {Error}", existProductCode_ApiResponse.StatusCode, existProductCode_ApiResponse.Message, existProductCode_ApiResponse.Error);
                                return Json(new { isError = true, message = existProductCode_ApiResponse.Error });
                            }
                            else
                            {
                                Log.Error("Service:ExistProductCode | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message}", existProductCode_ApiResponse.StatusCode, existProductCode_ApiResponse.Message);
                                return Json(new { isError = true, message = existProductCode_ApiResponse.Message });
                            }
                                
                        }
                    }
                    else
                    {
                        Log.Error("Ocurrio algo inesperado al validar la clave producto.");
                        return Json(new { isError = true, message = "Ocurrio algo inesperado al validar." });
                    }

                }

                return Json(new { isError = false, message = "" });
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return JsonResult_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return JsonResult_CustomerExceptionHandler(string.Empty, ex.HResult);
            }

        }


        [HttpPost]
        public async Task<JsonResult> ProductSupplierCodeFieldValidate([FromBody] ExistCodeParametersModel existCodeParametersModel)
        {
            try
            {
                if (string.IsNullOrEmpty(existCodeParametersModel.CodeValue))
                {
                    Log.Warning("El campo Clave Producto-Proveedor es requerido.");
                    return Json(new { isError = true, message = "El campo es requerido." });
                }
                else
                {
                    if (!Regex.IsMatch(existCodeParametersModel.CodeValue, @"^[a-zA-Z0-9\-]*$"))
                    {
                        Log.Warning("El campo Clave Producto-Proveedor sólo acepta caracteres alfanuméricos.");
                        return Json(new { isError = true, message = "Sólo acepta caracteres alfanuméricos." });
                    }

                    if (existCodeParametersModel.CodeValue.Length > 30)
                    {
                        Log.Warning("El campo Clave Producto-Proveedor no debe rebasar los 30 caracteres.");
                        return Json(new { isError = true, message = "No rebasar los 30 caracteres." });
                    }

                    var existProductSupplierCode_ApiResponse = await _productSupplierQueriesService
                                                                .ExistProductSupplierCodeAsync(
                                                                                        new ExistCodeParametersDto
                                                                                        {
                                                                                            ItemId = existCodeParametersModel.Item < 0 ? 0: existCodeParametersModel.Item,
                                                                                            CodeValue = existCodeParametersModel.CodeValue
                                                                                        });

                    if (existProductSupplierCode_ApiResponse != null)
                    {
                        if (existProductSupplierCode_ApiResponse.StatusCode == (int)HttpStatusCode.OK)
                        {
                            if (existProductSupplierCode_ApiResponse.Value)
                            {
                                Log.Warning("La Clave Producto-Proveedor ya existe.");
                                return Json(
                                            new
                                            {
                                                isError = existProductSupplierCode_ApiResponse.Value,
                                                message = "La clave ya existe."
                                            });
                            }
                        }
                        else
                        {
                            if (existProductSupplierCode_ApiResponse.Error != null)
                            {
                                Log.Error("Service:ExistProductSupplierCode | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message} | Error: {Error}", existProductSupplierCode_ApiResponse.StatusCode, existProductSupplierCode_ApiResponse.Message, existProductSupplierCode_ApiResponse.Error);
                                return Json(new { isError = true, message = existProductSupplierCode_ApiResponse.Error });
                            }
                            else
                            {
                                Log.Error("Service:ExistProductSupplierCode | HttpCodeApiResponse: {StatusCode} | MessageApiResponse: {Message}", existProductSupplierCode_ApiResponse.StatusCode, existProductSupplierCode_ApiResponse.Message);
                                return Json(new { isError = true, message = existProductSupplierCode_ApiResponse.Message });
                            }

                        }
                    }
                    else
                    {
                        Log.Error("Ocurrio algo inesperado al validar la clave producto-proveedor.");
                        return Json(new { isError = true, message = "Ocurrio algo inesperado al validar." });
                    }

                }

                return Json(new { isError = false, message = "" });
            }
            catch (HttpRequestException ex)
            {
                Log.Error("HttpRequestError: {HttpRequestError} | ErrorMessageDetails: {Message}", ex.HttpRequestError, ex.Message);
                return JsonResult_CustomerExceptionHandler(ex.HttpRequestError.ToString(), ex.HResult);
            }
            catch (Exception ex)
            {
                Log.Error("GenericMessage: Consulte al administrador del aplicativo | ErrorMessageDetails: {Message} | ErrorStackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return JsonResult_CustomerExceptionHandler(string.Empty, ex.HResult);
            }

        }


        [HttpGet]
        public JsonResult MultiSelectFieldValidate(int paramValue)
        {
            if (paramValue == 0)
            {
                return Json(new { isError = true, message = "Seleciona una opcion." });
            }

            return Json(new { isError = false, message = "" });
        }

        [HttpGet]
        public JsonResult ProductNameFieldValidate(string? paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                Log.Warning("El campo Nombre Producto es requerido");
                return Json(new { isError = true, message = "El campo es requerido." });
            }
            else
            {
                if (!Regex.IsMatch(paramValue, @"^[a-zA-Z0-9 ]*$"))
                {
                    Log.Warning("El campo Nombre Producto sólo acepta alfanumericos y espacio.");
                    return Json(new { isError = true, message = "Sólo acepta alfanumericos y espacio." });
                }

                if (paramValue.Length > 70)
                {
                    Log.Warning("El campo Nombre Producto no debe rebasar los 70 caracteres.");
                    return Json(new { isError = true, message = "No rebasar los 70 caracteres." });
                }
            }

            return Json(new { isError = false, message = "" });
        }

        //Errors Handlers
        [HttpGet]
        public IActionResult ProductError(CustomerErrorViewModel productErrorViewModel)
        {
            return View(productErrorViewModel);
        }

        private IActionResult RedirectToAction_CustomerExceptionHandler(
            string httpRequestError,
            int hResult,
            string errorMessage)
        {
            if (httpRequestError.Equals("ConnectionError") && hResult == -2147467259)
            {
                return RedirectToAction("ProductError", "Products", new CustomerErrorViewModel
                {
                    genericMessage = "Error de conexion Api: Consulte al administrador del aplicativo.",
                    ErrorMessage = errorMessage
                });
            }

            return RedirectToAction("ProductError", "Products", new CustomerErrorViewModel
            {
                genericMessage = "Consulte al administrador del aplicativo.",
                ErrorMessage = errorMessage
            });
        }

        private JsonResult JsonResult_CustomerExceptionHandler(
            string httpRequestError,
            int hResult)
        {
            if (httpRequestError.Equals("ConnectionError") && hResult == -2147467259)
            {
                return Json(new { isSuccess = false, message = "Error de conexion Api: Consulte al administrador del aplicativo." });
            }

            return Json(new { isSuccess = false, message = "Algo salió mal: Consulte al administrador del aplicativo." });
        }

    }
}

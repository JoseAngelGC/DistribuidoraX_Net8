using DistribuidoraX.Shared.Dtos.ProductSupplierDtos;
using FluentValidation;

namespace DistribuidoraX.Shared.Validators
{
    public class SupplierProductDtoValidator : AbstractValidator<SupplierProductDto>
    {
        public SupplierProductDtoValidator()
        {
            RuleFor(x => x.SupplierProductItem).GreaterThanOrEqualTo(0).WithMessage("El valor item producto-proveedor no es valido.")
                                       .NotNull().WithMessage("El item producto-proveedor es requerido.");
            RuleFor(x => x.SupplierProductCode).NotEmpty().WithMessage("El codigo del producto-proveedor es requerido.")
                                        .Matches(@"^[a-zA-Z0-9\-]*$").WithMessage("El codigo del producto-proveedor, sólo acepta caracteres alfanuméricos.")
                                        .MaximumLength(30).WithMessage("El codigo del producto-proveedor no debe rebasar los 30 caracteres.");
            RuleFor(x => x.SupplierProductCost).PrecisionScale(precision: 11, scale: 2, ignoreTrailingZeros: false)
                                        .WithMessage("El costo producto-proveedor, no coincide con el patron 000000000.00");
            RuleFor(x => x.SupplierItem).GreaterThan(-1).WithMessage("El item proveedor no es valido.")
                                            .NotEqual(0).WithMessage("El item proveedor es requerido.");
            RuleFor(x => x.SupplierName).NotEmpty().WithMessage("El nombre del proveedor es requerido.")
                                        .Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("El nombre del proveedor, sólo acepta caracteres alfanuméricos.")
                                        .MaximumLength(100).WithMessage("El nombre del proveedor no debe rebasar los 100 caracteres.");
        }
    }
}

using DistribuidoraX.Shared.Dtos.ProductDtos;
using FluentValidation;

namespace DistribuidoraX.Shared.Validators
{
    public class ProductFullDataDtoValidator : AbstractValidator<ProductFullDataDto>
    {
        public ProductFullDataDtoValidator()
        {
            RuleFor(x => x.ProductItem).GreaterThanOrEqualTo(0).WithMessage("El valor del item del producto no es valido.")
                                        .NotNull().WithMessage("El item del producto es requerido.");
            RuleFor(x => x.ProductCode).NotEmpty().WithMessage("El campo es requerido.")
                                        .Matches(@"^[a-zA-Z0-9\-]*$").WithMessage("Sólo acepta caracteres alfanuméricos.")
                                        .MaximumLength(30).WithMessage("No rebasar los 30 caracteres.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("El campo es requerido.")
                                        .Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Sólo acepta caracteres alfanuméricos.")
                                        .MaximumLength(70).WithMessage("No rebasar los 70 caracteres.");
            RuleFor(x => x.TypeProductItem).GreaterThan(-1).WithMessage("El item tipo de producto no es valido.")
                                            .NotEqual(0).WithMessage("El item tipo de producto es requerido.");
        }
    }
}

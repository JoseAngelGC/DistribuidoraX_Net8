using DistribuidoraX.Shared.Dtos.GenericDtos;
using FluentValidation;

namespace DistribuidoraX.Shared.Validators
{
    public class ExistProductCodeParametersDtoValidator : AbstractValidator<ExistCodeParametersDto>
    {
        public ExistProductCodeParametersDtoValidator()
        {
            RuleFor(x => x.ItemId).GreaterThanOrEqualTo(0).WithMessage("El valor del item del producto no es valido.")
                                        .NotNull().WithMessage("El item del producto es requerido.");
            RuleFor(x => x.CodeValue).NotEmpty().WithMessage("El campo es requerido.")
                                        .Matches(@"^[a-zA-Z0-9\-]*$").WithMessage("Sólo acepta caracteres alfanuméricos.");
        }
    }
}

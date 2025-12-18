using ConsultoriaSystem.Api.Dtos;
using FluentValidation;

namespace ConsultoriaSystem.Api.Validators
{
    public class PaqueteUpdateRequestValidator : AbstractValidator<PaqueteUpdateRequest>
    {
        public PaqueteUpdateRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.DuracionHoras)
                .GreaterThan(0)
                .LessThanOrEqualTo(200);

            RuleFor(x => x.Costo)
                .GreaterThan(0);

            RuleFor(x => x.Descripcion)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Descripcion));

            RuleFor(x => x.Activo)
                .NotNull().WithMessage("El estado Activo es requerido.");
        }
    }
}
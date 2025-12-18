using ConsultoriaSystem.Api.Dtos;
using FluentValidation;

namespace ConsultoriaSystem.Api.Validators
{
    public class PaqueteCreateRequestValidator : AbstractValidator<PaqueteCreateRequest>
    {
        public PaqueteCreateRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("Máximo 100 caracteres.");

            RuleFor(x => x.DuracionHoras)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a 0.")
                .LessThanOrEqualTo(200).WithMessage("La duración máxima es de 200 horas.");

            RuleFor(x => x.Costo)
                .GreaterThan(0).WithMessage("El costo debe ser mayor a 0.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Descripcion))
                .WithMessage("La descripción debe tener máximo 500 caracteres.");
        }
    }
}
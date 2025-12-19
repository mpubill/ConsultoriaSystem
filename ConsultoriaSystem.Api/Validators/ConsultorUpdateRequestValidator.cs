using ConsultoriaSystem.Api.Dtos;
using FluentValidation;

namespace ConsultoriaSystem.Api.Validators
{
    public class ConsultorUpdateRequestValidator : AbstractValidator<ConsultorUpdateRequest>
    {
        public ConsultorUpdateRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre debe tener máximo 150 caracteres.");

            RuleFor(x => x.AreaEspecializacion)
                .NotEmpty().WithMessage("El área de especialización es obligatoria.")
                .MaximumLength(150).WithMessage("El área debe tener máximo 150 caracteres.");

            RuleFor(x => x.TarifaHora)
                .InclusiveBetween(30, 200)
                .WithMessage("La tarifa por hora debe estar entre $30 y $200.");

            RuleFor(x => x.EmailCorporativo)
                .NotEmpty().WithMessage("El email corporativo es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.")
                .MaximumLength(150).WithMessage("El email debe tener máximo 150 caracteres.");
        }
    }
}

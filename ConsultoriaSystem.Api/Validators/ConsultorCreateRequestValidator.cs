using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Repositories;
using FluentValidation;

namespace ConsultoriaSystem.Api.Validators
{
    public class ConsultorCreateRequestValidator : AbstractValidator<ConsultorCreateRequest>
    {
        public ConsultorCreateRequestValidator(IConsultoresRepository consultorRepository)
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.AreaEspecializacion)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.TarifaHora)
                .InclusiveBetween(30, 200)
                .WithMessage("La tarifa por hora debe estar entre $30 y $200.");

            RuleFor(x => x.EmailCorporativo)
                .NotEmpty().WithMessage("El email corporativo es obligatorio.")
                .EmailAddress().WithMessage("Formato de correo inválido.")
                .MaximumLength(150);

        }
    }
}

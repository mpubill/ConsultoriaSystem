using ConsultoriaSystem.Api.Dtos;
using FluentValidation;

namespace ConsultoriaSystem.Api.Validators
{
    public class UsuariosDtoValidator : AbstractValidator<UsuariosDTO>
    {
        public UsuariosDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("Formato de email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}

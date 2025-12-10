using Core.Entities;
using FluentValidation;

namespace Infraestructure.Validators
{

    public class UsersValidator : AbstractValidator<Users>
    {
        public UsersValidator()
        {
            RuleFor(x => x.NameUser)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio");

            RuleFor(x => x.PasswordUser)
                .NotEmpty().WithMessage("La contraseña es obligatoria");

            // Agrega más reglas según necesites
        }
    }
}

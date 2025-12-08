using Core.Entities;
using FluentValidation;

namespace Infraestructure.Validators
{
    public class TaskUserValidator : AbstractValidator<TaskUser>
    {
        public TaskUserValidator()
        {
            RuleFor(attribute => attribute.idUsers).Custom((st, context) =>
            {
                if (st == 0)
                {
                    context.AddFailure("La tarea necesita un usuario");
                }
            });
            RuleFor(attribute => attribute.NameTask).Custom((st, context) =>
            {
                if (st == null)
                {
                    context.AddFailure("La tarea necesita un usuario");
                }
            });
            RuleFor(attribute => attribute.Complete).Custom((st, context) =>
            {
                if (st == null)
                {
                    context.AddFailure("Se debe confirmar si esta completado o no");
                }
            });
        }
    }
}

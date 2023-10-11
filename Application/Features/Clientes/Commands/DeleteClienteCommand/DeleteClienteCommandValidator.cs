
using FluentValidation;

namespace Application.Features.Clientes.Commands.DeleteClienteCommand
{
    public class DeleteClienteCommandValidator : AbstractValidator<DeleteClienteCommand>
    {
        public DeleteClienteCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} no puede ser nulo")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a cero");
        }
    }
}

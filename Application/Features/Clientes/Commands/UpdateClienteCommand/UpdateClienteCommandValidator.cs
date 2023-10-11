using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Commands.UpdateClienteCommand
{
    public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Apellidos)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Telefono)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .Matches(@"^\d{4}-\d{4}$").WithMessage("{PropertyName} debe cumplir el formato 0000-0000")
                .MaximumLength(10).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .EmailAddress().WithMessage("{PropertyName} debe ser una direccion de email valida")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Direccion)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Apellido)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .EmailAddress().WithMessage("{PropertyName} debe ser una direccion de email valida")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(15).WithMessage("{PropertyName} no debe de exceder {MaxLength}");


            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(15).WithMessage("{PropertyName} no debe de exceder {MaxLength}");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .MaximumLength(15).WithMessage("{PropertyName} no debe de exceder {MaxLength}")
                .Equal(p => p.Password).WithMessage("{PropertyName} debe ser igual al password");
        }
    }
}

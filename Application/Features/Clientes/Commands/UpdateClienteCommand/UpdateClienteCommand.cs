using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Commands.UpdateClienteCommand
{
    public class UpdateClienteCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }

    public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repository;
        public readonly IMapper _mapper;

        public UpdateClienteCommandHandler(IRepositoryAsync<Cliente> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<Response<int>> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
            }
            else
            {
                cliente.Nombre = request.Nombre;
                cliente.Apellidos = request.Apellidos;
                cliente.Direccion = request.Direccion;
                cliente.FechaNacimiento = request.FechaNacimiento;
                cliente.Telefono = request.Telefono;
                cliente.Email = request.Email;

                await _repository.UpdateAsync(cliente);
                return new Response<int>(cliente.Id);
            }
        }
    }
}

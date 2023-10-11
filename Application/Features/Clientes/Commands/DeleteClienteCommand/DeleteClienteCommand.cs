
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clientes.Commands.DeleteClienteCommand
{
    public class DeleteClienteCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        public readonly IMapper mapper;

        public DeleteClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            this._repositoryAsync = repositoryAsync;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {


            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
            }
            else
            {

                await _repositoryAsync.DeleteAsync(cliente);
                return new Response<int>(cliente.Id);
            }
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clientes.Queries.GetClienteById
{
    public class GetClienteByIdQuery : IRequest<Response<ClienteDTO>>
    {
        public int Id { get; set; }


        public GetClienteByIdQuery()
        {
            
        }
    }

    public class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, Response<ClienteDTO>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        public readonly IMapper mapper;

        public GetClienteByIdQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            this._repositoryAsync = repositoryAsync;
            this.mapper = mapper;
        }

        public async Task<Response<ClienteDTO>> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
            }
            var dto = this.mapper.Map<ClienteDTO>(cliente);
            return new Response<ClienteDTO>(dto);
        }
    }
}

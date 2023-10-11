
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clientes.Commands.CreatedClienteCommand
{
    public class CreatedClienteCommand : IRequest<Response<int>>
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }

    public class CreatedClienteCommandHandler : IRequestHandler<CreatedClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public CreatedClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryAsync = repositoryAsync;
        }

        public async Task<Response<int>> Handle(CreatedClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var nuevoRegistro = _mapper.Map<Cliente>(request);
                var data = await _repositoryAsync.AddAsync(nuevoRegistro);

                return new Response<int>(data.Id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

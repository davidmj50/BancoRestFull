using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IRequest<PagedResponse<List<ClienteDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class GetAllClientesQueryHandler : IRequestHandler<GetAllClientesQuery, PagedResponse<List<ClienteDTO>>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IDistributedCache _distributedCache;
        public readonly IMapper mapper;

        public GetAllClientesQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
        {
            this._repositoryAsync = repositoryAsync;
            this.mapper = mapper;
            _distributedCache = distributedCache;
        }
        public async Task<PagedResponse<List<ClienteDTO>>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"listadoClientes_{request.PageSize}_{request.PageNumber}_{request.Nombre}_{request.Apellido}";
            string serializedListadoClientes;
            var listadoClientes = new List<Cliente>();

            var redisListadoClientes = await _distributedCache.GetAsync(cacheKey);
            if (redisListadoClientes != null)
            {
                serializedListadoClientes = Encoding.UTF8.GetString(redisListadoClientes);
                listadoClientes = JsonConvert.DeserializeObject<List<Cliente>>(serializedListadoClientes);
            } 
            else
            {
                listadoClientes = await _repositoryAsync.ListAsync(new Specifications.PagedClientesSpecification(request.PageSize, request.PageNumber,request.Nombre, request.Apellido));
                serializedListadoClientes = JsonConvert.SerializeObject(listadoClientes);
                redisListadoClientes = Encoding.UTF8.GetBytes(serializedListadoClientes);

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisListadoClientes, options);
            }

            var clientesDTO = mapper.Map<List<ClienteDTO>>(listadoClientes);
            return new PagedResponse<List<ClienteDTO>> (clientesDTO, request.PageNumber, request.PageSize);
        }
    }
}

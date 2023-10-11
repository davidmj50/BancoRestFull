using Application.Features.Clientes.Commands.CreatedClienteCommand;
using Application.Features.Clientes.Commands.DeleteClienteCommand;
using Application.Features.Clientes.Commands.UpdateClienteCommand;
using Application.Features.Clientes.Queries.GetAllClientes;
using Application.Features.Clientes.Queries.GetClienteById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{

    [ApiVersion("1.0")]
    public class ClienteController : BaseApiController
    {
        //POST: api/<controller>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreatedClienteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        //El controlador solo esta enrutando, la logica esta en el core
        //POST: api/<controller>
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateClienteCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteClienteCommand { Id = id}));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetClienteByIdQuery { Id = id }));
        }

        //POST: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllClientesParameters filter)
        {
            return Ok(await Mediator.Send(new GetAllClientesQuery{ 
                PageNumber = filter.PageNumber, 
                PageSize = filter.PageSize, 
                Apellido = filter.Apellido, 
                Nombre = filter.Nombre 
            }));
        }

    }
}

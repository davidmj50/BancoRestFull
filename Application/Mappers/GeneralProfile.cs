using Application.DTOs;
using Application.Features.Clientes.Commands.CreatedClienteCommand;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Commands
            CreateMap<CreatedClienteCommand, Cliente>();
            #endregion

            #region DTOs
            CreateMap<Cliente, ClienteDTO>();
            #endregion

        }
    }
}

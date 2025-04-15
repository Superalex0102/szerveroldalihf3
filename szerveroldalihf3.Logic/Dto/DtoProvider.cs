using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szerveroldalihf3.Entities.Dto.Bug;
using szerveroldalihf3.Entities.Entity;

namespace szerveroldalihf3.Logic.Dto
{
    public class DtoProvider
    {
        public Mapper Mapper { get; }

        public DtoProvider()
        {
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketCreateDto, Ticket>();
                cfg.CreateMap<Ticket, TicketViewDto>();
            }));
        }
    }
}

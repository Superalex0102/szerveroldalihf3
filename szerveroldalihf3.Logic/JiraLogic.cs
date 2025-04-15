using AutoMapper;
using szerveroldalihf3.Data;
using szerveroldalihf3.Entities.Dto.Bug;
using szerveroldalihf3.Entities.Entity;
using szerveroldalihf3.Logic.Dto;

namespace szerveroldalihf3.Logic
{
    public class JiraLogic
    {
        public Repository<Ticket> repository;
        public Mapper mapper;

        public JiraLogic(Repository<Ticket> repository, DtoProvider provider)
        {
            this.repository = repository;
            this.mapper = provider.Mapper;
        }

        public IEnumerable<TicketViewDto> Read()
        {
            return repository.GetAll().Select(t => mapper.Map<TicketViewDto>(t));
        }

        public TicketViewDto Read(string slug)
        {
            return mapper.Map<TicketViewDto>(repository.GetAll());
        }

        public async Task Create(TicketCreateDto dto, string userId)
        {
            var forum = mapper.Map<Ticket>(dto);

            forum.AppUserId = userId;
            forum.Date = DateTime.Now;
            await repository.CreateAsync(forum);
        }
    }
}
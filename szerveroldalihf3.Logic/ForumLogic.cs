using AutoMapper;
using szerveroldalihf3.Data;
using szerveroldalihf3.Entities.Dto.Bug;
using szerveroldalihf3.Entities.Entity;
using szerveroldalihf3.Logic.Dto;

namespace szerveroldalihf3.Logic
{
    public class ForumLogic
    {
        public Repository<Bug> repository;
        public Mapper mapper;

        public ForumLogic(Repository<Bug> repository, DtoProvider provider)
        {
            this.repository = repository;
            this.mapper = provider.Mapper;
        }

        public IEnumerable<BugViewDto> Read()
        {
            return repository.GetAll().Select(t => mapper.Map<BugViewDto>(t));
        }

        public BugViewDto Read(string slug)
        {
            return mapper.Map<BugViewDto>(repository.GetAll());
        }

        public async Task Create(BugCreateDto dto)
        {
            var forum = mapper.Map<Bug>(dto);
            forum.AppUserId = "1";
            await repository.CreateAsync(forum);
        }
    }
}
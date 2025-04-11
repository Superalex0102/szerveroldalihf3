using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using szerveroldalihf3.Endpoint.Helpers;
using szerveroldalihf3.Entities.Dto.Bug;
using szerveroldalihf3.Entities.Entity;
using szerveroldalihf3.Logic;

namespace szerveroldalihf3.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BugController : ControllerBase
    {
        ForumLogic logic;
        UserManager<AppUser> userManager;
        IHubContext<ForumHub> forumHub;
        IBackgroundJobClient backgroundJobClient;
        public BugController(ForumLogic logic, UserManager<AppUser> userManager, IHubContext<ForumHub> forumHub, IBackgroundJobClient backgroundJobClient)
        {
            this.logic = logic;
            this.userManager = userManager;
            this.forumHub = forumHub;
            this.backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        public IEnumerable<BugViewDto> Get()
        {
            return logic.Read();
        }

        [HttpGet("{slug}")]
        public BugViewDto Get(string slug)
        {
            return logic.Read(slug);
        }

        [HttpPost("/createbug")]
        public async Task Post(BugCreateDto dto)
        {
            await logic.Create(dto);

            //backgroundJobClient.Enqueue(() => Console.WriteLine("Ez egy háttérfeladat!"));

            backgroundJobClient.Schedule(() => SendResponse(), TimeSpan.FromSeconds(15));

            //await movieHub.Clients.All.SendAsync("newMovie");
        }

        [HttpGet("job")]
        public void SendResponse()
        {
            forumHub.Clients.All.SendAsync("newMovie");
        }
    }
}

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<BugViewDto> Get()
        {
            return logic.Read();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{slug}")]
        public BugViewDto Get(string slug)
        {
            return logic.Read(slug);
        }

        [Authorize]
        [HttpPost("/createbug")]
        public async Task Post(BugCreateDto dto)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return;

            await logic.Create(dto, user.Id);

            backgroundJobClient.Schedule(() => SendResponse(), TimeSpan.FromSeconds(15));
        }

        [HttpGet("job")]
        public void SendResponse()
        {
            forumHub.Clients.All.SendAsync("newMovie");
        }
    }
}

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
    public class TicketController : ControllerBase
    {
        JiraLogic logic;
        UserManager<AppUser> userManager;
        IHubContext<JiraHub> forumHub;
        IBackgroundJobClient backgroundJobClient;
        public TicketController(JiraLogic logic, UserManager<AppUser> userManager, IHubContext<JiraHub> forumHub, IBackgroundJobClient backgroundJobClient)
        {
            this.logic = logic;
            this.userManager = userManager;
            this.forumHub = forumHub;
            this.backgroundJobClient = backgroundJobClient;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<TicketViewDto> Get()
        {
            return logic.Read();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{slug}")]
        public TicketViewDto Get(string slug)
        {
            return logic.Read(slug);
        }

        [Authorize]
        [HttpPost("/createticket")]
        public async Task Post(TicketCreateDto dto)
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

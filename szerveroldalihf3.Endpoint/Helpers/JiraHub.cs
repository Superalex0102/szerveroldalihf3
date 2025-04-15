using Microsoft.AspNetCore.SignalR;

namespace szerveroldalihf3.Endpoint.Helpers
{
    public class JiraHub : Hub
    {
        public async Task NewForumSignal()
        {
            await Clients.All.SendAsync("newJira");
        }
    }
}

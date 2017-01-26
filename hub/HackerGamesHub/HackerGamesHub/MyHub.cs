using Microsoft.AspNet.SignalR;

namespace HackerGamesHub
{
    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}
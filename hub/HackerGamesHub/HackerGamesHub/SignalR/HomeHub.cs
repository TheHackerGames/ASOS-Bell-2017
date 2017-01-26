using Microsoft.AspNet.SignalR;

namespace HackerGamesHub.SignalR
{
    public class HomeHub : Hub
    {

        public void RegisterBell()
        {
            Groups.Add(Context.ConnectionId, GroupNames.Bell);
        }

        public void RegisterHome()
        {
            Groups.Add(Context.ConnectionId, GroupNames.Home);
        }

        public void PressBell(string message, string imageId)
        {
            Clients.Group(GroupNames.Home).BellPressed(message, imageId);
        }

        public void AcceptHome(string message)
        {
            Clients.Group(GroupNames.Bell).HomeAccepted(message);
        }
    }

    public class GroupNames
    {
        public const string Bell = "Bell";
        public const string Home = "Home";
    }
}
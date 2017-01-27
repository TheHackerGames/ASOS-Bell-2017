using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace HackerGamesHub.SignalR
{
    public class HomeHub : Hub
    {
        private static readonly List<string> BellPressedSubscribers = new List<string> { GroupNames.Home, GroupNames.Hue };

        public void RegisterBell()
        {
            Groups.Add(Context.ConnectionId, GroupNames.Bell);
        }

        public void RegisterHome()
        {
            Groups.Add(Context.ConnectionId, GroupNames.Home);
        }

        public void RegisterHue()
        {
            Groups.Add(Context.ConnectionId, GroupNames.Hue);
        }

        public void PressBell(string message, string imageId)
        {
            Clients.Groups(BellPressedSubscribers).BellPressed(message, imageId);
        }

        public void AcceptHome(string message)
        {
            Clients.Group(GroupNames.Bell).HomeAccepted(message);
        }

        public void SendMessage(string message)
        {
            Clients.Group(GroupNames.Home).MessageSent(message);
        }

        public void RespondMessage(string message)
        {
            Clients.Group(GroupNames.Bell).MessageResponded(message);
        }

        public void OpenDoor(string message)
        {
            Clients.Group(GroupNames.Bell).DoorOpened(message);
        }

        public void End(string message)
        {
            Clients.Group(GroupNames.Bell).Ended(message);
        }
    }

    public class GroupNames
    {
        public const string Bell = "Bell";
        public const string Home = "Home";
        public const string Hue = "Hue";
    }
}
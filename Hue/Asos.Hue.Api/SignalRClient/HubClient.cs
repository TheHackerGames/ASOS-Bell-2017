using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using SignalRClient.Enum;

namespace SignalRClient
{
    public class HubClient
    {
        private readonly HubConnection _hub;
        private readonly IHubProxy _proxy;
        public HubClient(string uri)
        {
            _hub = new HubConnection(uri);
            _proxy = _hub.CreateHubProxy("HomeHub");
        }

        public IHubProxy CreateProxy(string name)
        {
            return _hub.CreateHubProxy(name);
        }

        public async Task Start() 
        {
            await _hub.Start();
        }
        public void Stop()
        {
            _hub.Stop();
        }
        public async Task RegisterClient(string name)
        {
            var operation = $"Register{name}";
             await _proxy.Invoke(operation);
        }
        public void RegisterHandler<T1>(SignalREvent eventName, Action<T1> handler)
        {
            _proxy.On<T1>(eventName.ToString(), handler);
        }
        public void RegisterHandler<T1,T2>(SignalREvent eventName, Action<T1, T2> handler)
        {
             _proxy.On<T1, T2>(eventName.ToString(),  handler);
        }
    }
}

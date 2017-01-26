using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asos.Hue.Api;
using Asos.Hue.Api.Interfaces;
using Asos.Hue.Api.Options;
using Microsoft.AspNet.SignalR.Client;
using SignalRClient.Config;
using SignalRClient.Enum;

namespace SignalRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Hue Api Client started ***");
            Console.WriteLine($"*** Listening to {GlobalConfig.SignalRHost}, Client name: {GlobalConfig.SignalRClientName}, Hub: {GlobalConfig.SignalRHub} ***");

            StartAsync().Wait();

            Console.ReadLine();
            Console.WriteLine("*** Exiting Hue Api Client started ***");
        }

        private static async Task StartAsync()
        {
            var client = new HubClient(GlobalConfig.SignalRHost);
            client.CreateProxy(GlobalConfig.SignalRHub);

            await client.Start();

            client.RegisterHandler<string, string>(SignalREvent.BellPressed, BellPressedHandler);

            await client.RegisterClient(GlobalConfig.SignalRClientName);
        }

        private static async void BellPressedHandler(string msg, string id)
        {
            Console.WriteLine($"Bell Pressed Received: {msg} Id: {id}, Hue Flashing...");

            IHue hue = new HueHub(new HueHubOptions()
            {
                Uri = GlobalConfig.HueApiUrl,
                UserKey = GlobalConfig.HueUserKey
            });

            var bulb = new Bulb { Id = 2 };

            await hue.Flash(bulb);
        }
    }
}


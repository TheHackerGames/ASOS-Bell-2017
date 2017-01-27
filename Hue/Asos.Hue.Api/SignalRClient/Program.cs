using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asos.Hue.Api;
using Asos.Hue.Api.Enums;
using Asos.Hue.Api.Interfaces;
using Asos.Hue.Api.Models;
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
            Console.WriteLine("*** Hue SignalR Client started ***");
            var initialiseSuccess = false;

            while (!initialiseSuccess)
            {
                initialiseSuccess = Initialise().Result;
            }

            StartAsync().Wait();

            Console.WriteLine("*** Press Enter to exit application***");
            Console.ReadLine();
            Console.WriteLine("*** Exiting Hue Api Client started ***");
        }

        private static async Task<bool> Initialise()
        {
            Console.WriteLine("\nInitialising connection to Hue Api\n");
            IHue hue = HueHub.Create(new HueHubOptions()
            {
                Uri = GlobalConfig.HueApiUrl,
                UserKey = GlobalConfig.HueUserKey
            });

            var bulbs = await hue.GetAllBulbs();

            if (bulbs.Count == 0)
            {
                Console.WriteLine($"Connecting to Hue Api: {GlobalConfig.HueApiUrl} failed, please enter endpoint url");
                GlobalConfig.HueApiUrl = Console.ReadLine();
                return false;
            };

            Console.WriteLine("The follow bulbs are registered: \n");
            Console.WriteLine($"{"Name",20} | {"Turned On",10} | {"Online",10}");
            foreach (var b in bulbs)
            {
                Console.WriteLine($"{b.Name,20} | {b.IsOn,10} | {b.Reachable,10}");
            }
            return true;
        }
        private static async Task StartAsync()
        {
            Console.WriteLine("\nConnecting to SignalR Server...");
            var client = new HubClient(GlobalConfig.SignalRHost);
            client.CreateProxy(GlobalConfig.SignalRHub);

            await client.Start();

            client.RegisterHandler<string, string>(SignalREvent.BellPressed, BellPressedHandler);
            client.RegisterHandler<string, string>(SignalREvent.FacesIdentified, FacesIdentifiedHandler);
            client.RegisterHandler<string, string>(SignalREvent.FacesUnknown, FacesUnknownHandler);

            await client.RegisterClient(GlobalConfig.SignalRClientName);

            Console.WriteLine($"Connected to client: {GlobalConfig.SignalRHost}, Client name: {GlobalConfig.SignalRClientName}, Hub: {GlobalConfig.SignalRHub}\n");
            Console.WriteLine("Waiting for events...\n");
        }

        private static async void FacesIdentifiedHandler(string msg, string id)
        {
            Console.WriteLine($"Faces Identified Received: {msg} Id: {id}, Hue Flashing...");
            await RunHue(HueColor.Green);
            Console.WriteLine("*** Handler action complete ***");
        }
        private static async void FacesUnknownHandler(string msg, string id)
        {
            Console.WriteLine($"Faces Unknown Received: {msg} Id: {id}, Hue Flashing...");
            await RunHue(HueColor.Red);
            Console.WriteLine("*** Handler action complete ***");
        }
        private static async void BellPressedHandler(string msg, string id)
        {
            Console.WriteLine($"Bell Pressed Received: {msg} Id: {id}, Hue Flashing...");
            await RunHue(HueColor.White);
            Console.WriteLine("*** Handler action complete ***");
        }
        private static async Task RunHue(HueColor color)
        {

            IHue hue = HueHub.Create(new HueHubOptions()
            {
                Uri = GlobalConfig.HueApiUrl,
                UserKey = GlobalConfig.HueUserKey
            });

            var bulbs = await hue.GetAllBulbs();

            if (bulbs.Any())
            {
                Console.WriteLine("No bulbs online to flash...");
                return;
            }
            foreach (var b in bulbs)
            {
                if (b.Reachable)
                {
                    Console.WriteLine($"{b.Name} is flashing {color}...");
                    await hue.Flash(b, color);
                }
            }
        }
    }
}


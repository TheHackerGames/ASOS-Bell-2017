using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asos.Hue.Api;
using Asos.Hue.Api.Enums;
using Asos.Hue.Api.Interfaces;
using Asos.Hue.Api.Models;
using Asos.Hue.Api.Options;
using SignalRClient.Config;

namespace ConsoleApp
{
    class Program
    {
        static  void Main(string[] args)
        {
            Console.WriteLine("*** Test Client Started ***");
            Console.WriteLine("*** Please enter: bulbs, on, off, toggle, flash, or q to exit");
            Console.WriteLine();
            var input = Console.ReadLine();

            while(!input.Equals("q", StringComparison.InvariantCultureIgnoreCase))
            {
                RunAsync(input).Wait();
                input = Console.ReadLine();
            }
         
            Console.ReadLine();

            Console.WriteLine("*** Exiting Test Client ***");
        }

        private static async Task RunAsync(string action)
        {
            Console.WriteLine($"*** Executing '{action}' ***");
            IHue hue = new HueHub(new HueHubOptions() { Uri = GlobalConfig.HueApiUrl, UserKey = GlobalConfig.HueUserKey });

            var bulb = new Bulb {  Id = 2 };
            List<Bulb> bulbs = null;
            switch (action.ToLower())
            {
                case "bulbs":
                    bulbs = await hue.GetAllBulbs();
                    foreach (var b in bulbs)
                    {
                        Console.WriteLine($"{b.Name} is on: {b.IsOn} and reachable: {b.Reachable}");
                    }
                    break;
                case "on":
                    await hue.TurnOn(bulb, HueColor.Green);
                    break;
                case "off":
                    await hue.TurnOff(bulb, HueColor.Green);
                    break;
                case "toggle":
                    await hue.Toggle(bulb, HueColor.Green);
                    break;
                case "flash":
                    await hue.Flash(bulb, HueColor.Green);
                    break;
                default:
                    await hue.Toggle(bulb, HueColor.Green);
                    break;
            }
            Console.WriteLine("*** Finished Executing ***");
        }
    }
}

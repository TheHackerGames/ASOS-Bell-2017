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
        static void Main(string[] args)
        {
            Console.WriteLine("*** Test Client Started ***");
            Console.WriteLine("*** Please enter: bulbs, on, off, toggle, flash, green, orange or q to exit");
            Console.WriteLine();
            var input = Console.ReadLine();

            while (!input.Equals("q", StringComparison.InvariantCultureIgnoreCase))
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

            List<Bulb> bulbs = await hue.GetAllBulbs();

            foreach (var bulb in bulbs)
            {
                if (bulb.Reachable)
                {
                    switch (action.ToLower())
                    {
                        case "bulbs":
                            bulbs = await hue.GetAllBulbs();
                            foreach (var b in bulbs)
                            {
                                Console.WriteLine($"{b.Name} is on: {b.IsOn} and reachable: {b.Reachable}");
                            }
                            return;
                        case "on":
                            await hue.TurnOn(bulb, HueColor.White);
                            break;
                        case "off":
                            await hue.TurnOff(bulb, HueColor.White);
                            break;
                        case "toggle":
                            await hue.Toggle(bulb, HueColor.Green);
                            break;
                        case "flash":
                            await hue.Flash(bulb, HueColor.White);
                            break;
                        case "green":
                            await hue.Flash(bulb, HueColor.Green);
                            break;
                        case "orange":
                            await hue.Flash(bulb, HueColor.Orange);
                            break;
                        default:
                            await hue.Toggle(bulb, HueColor.Green);
                            break;
                    }
                }
            }

            Console.WriteLine("*** Finished Executing ***");
        }
    }
}

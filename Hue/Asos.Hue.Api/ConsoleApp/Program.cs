using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asos.Hue.Api;
using Asos.Hue.Api.Interfaces;
using Asos.Hue.Api.Options;
using SignalRClient;
using SignalRClient.Config;

namespace ConsoleApp
{
    class Program
    {
        static  void Main(string[] args)
        {
            Console.WriteLine("*** Test Client Started ***");

            RunAsync().Wait();
         
            Console.ReadLine();

            Console.WriteLine("*** Exiting Test Client ***");
        }

        private static async Task RunAsync()
        {
            IHue hue = new HueHub(new HueHubOptions() { Uri = GlobalConfig.HueApiUrl, UserKey = GlobalConfig.HueUserKey });

            var bulb = new Bulb {  Id = 2 };

            await hue.Flash(bulb);
        }
    }
}

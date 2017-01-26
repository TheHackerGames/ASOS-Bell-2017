﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace HackerGamesHub.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = ConfigurationManager.AppSettings["Endpoint"];
            using (WebApp.Start<Startup>(url))
            {
                System.Console.WriteLine("Server running on {0}", url);
                System.Console.ReadLine();
            }
        }
    }
}

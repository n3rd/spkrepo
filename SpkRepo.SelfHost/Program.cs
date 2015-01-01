using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpkRepo.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["SpkRepo.SelfHost:url"];

            if (string.IsNullOrEmpty(baseAddress))
                throw new ArgumentNullException("SpkRepo.SelfHost:url appSetting");

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.ReadKey();
            }
        }
    }
}

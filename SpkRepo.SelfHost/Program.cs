using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpkRepo.SelfHost
{
    class Program
    {
        static ManualResetEvent mre = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                mre.Set();
                eArgs.Cancel = true;
            };

            string baseAddress = ConfigurationManager.AppSettings["SpkRepo.SelfHost:url"];

            if (string.IsNullOrEmpty(baseAddress))
                throw new ArgumentNullException("SpkRepo.SelfHost:url appSetting");

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                mre.WaitOne();
            }
        }
    }
}

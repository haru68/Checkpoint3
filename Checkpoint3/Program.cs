using System;
using Nancy.Hosting.Self;
using Nancy.Configuration;
using System.IO;

namespace Checkpoint3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @".\Root";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = @".\Root\File";
            for(int i = 0; i<4; i++)
            {
                if (!File.Exists(filepath+i))
                {
                    File.CreateText(filepath + i);
                }
            }

            HostConfiguration hostConfiguration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true },
            };
            using (var host = new NancyHost(hostConfiguration, new Uri("http://localhost:1234")))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:1234");
                Console.ReadLine();
                host.Stop();
            }
        }
    }
}

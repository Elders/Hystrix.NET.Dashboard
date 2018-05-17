using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Hystrix.NET.Dashboard
{
    public static class ConsoleApplication
    {
        public static void Main(string[] args)
        {
            var port = 9000;
            if (args != null && args.Length > 0)
            {
                port = int.Parse(args[0]);
            }
            HystrixDashboard.Selfhost($"http://+:{port}/");
            Console.WriteLine($"Dashboard started on http://127.0.0.1:{port}/dashboard/");
            Console.WriteLine($"Press enter to exit...");
            Console.ReadLine();
        }
    }
}

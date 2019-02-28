using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ReactDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
                // .UseKestrel(options: options => 
                // {
                //     options.Listen(IPAddress.Loopback, 5001, configure: listenOptions => 
                //     {
                //         listenOptions.UseHttps(@"C:\Users\PC-001\source\dotnet-core.pfx", "cchyjr2u");
                //     });
                // });
    }
}

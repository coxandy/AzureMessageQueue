using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MessagePublisherWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //To be installed as a Windows service
            CreateHostBuilder(args).Build().Run();            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

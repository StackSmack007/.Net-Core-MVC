namespace EventuresApp
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                                      WebHost.CreateDefaultBuilder(args)
                                    //  .ConfigureLogging(x => x.AddConsole())
                                      .UseStartup<Startup>();
    }
}

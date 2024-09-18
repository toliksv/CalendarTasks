using CalendarTasks.Core.Infrastructure;
using CalendarTasks.Host.Infrastructurfe;
using CalendarTasks.FileRepository.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CalendarTasks.Host.ConsoleCommands;
using Microsoft.Extensions.Logging;


var builder = CreateHostBuilder(args);
await builder.Build().RunAsync(); 


static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.json", optional: true);
            builder.AddCommandLine(args);
        })
        .ConfigureServices((context, services) =>
        {
            services.AddDefaultDateTimeProvider();
            services.AddDefaultCalendarTaskNumberGenerator();
            services.AddCalendarTaskFileRepository(context.Configuration);
            services.AddCoreConsoleServices();
            services.AddConsoleCommands(context.Configuration);
            services.AddLogging(builder => builder.AddConsole());
            services.AddHostedService<ConsoleCommandHandler>();                        
        })
        .UseConsoleLifetime();
}
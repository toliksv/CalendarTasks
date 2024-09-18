using System;
using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleCommands.Commands;
using CalendarTasks.Host.ConsoleCommands.Providers;
using CalendarTasks.Host.ConsoleCommands.Commands.Help;
using CalendarTasks.Host.ConsoleOputput;

namespace CalendarTasks.Host.Infrastructurfe;

/// <summary>
/// Расширения для регистрации консольных обработчиков.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация консольных команд.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddConsoleCommands(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConsoleCommandOptions>(configuration.GetSection(nameof(ConsoleCommandOptions)));
        services.AddSingleton<ICalendarTaskConsoleWriter, CalendarTaskConsoleWriter>();
        services.AddSingleton<IConsoleCommandProvider, ConsoleCommandProvider>();
        services.AddTransient<IConsoleCommand, AddCalendarTaskConsoleCommand>();
        services.AddTransient<IConsoleCommand, ListCalendarTasksConsoleCommand>();
        services.AddTransient<IConsoleCommand, ListActualCalendarTaskConsoleCommand>();
        services.AddTransient<IConsoleCommand, UpdateCalendarTaskConsoleCommand>();
        services.AddTransient<IHelpConsoleCommand, HelpConsoleCommand>();
        return services;
    }
}

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CalendarTasks.FileRepository.FileOutput;
using CalendarTasks.Defs.Repository;

namespace CalendarTasks.FileRepository.Infrastructure;

/// <summary>
/// Методы расширения для регистрации хранилища на основе файлов.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCalendarTaskFileRepository(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        services.AddOptions();        
        services.Configure<CalendarTaskFileRepositoryOptions>(configuration.GetSection(nameof(CalendarTaskFileRepositoryOptions)));
        services.AddSingleton<IFileOutputHandler, FileOutputHandler>();
        services.AddSingleton<ICalendarTaskRepository, CalendarTaskFileRepository>();        
        return services;
    }
}

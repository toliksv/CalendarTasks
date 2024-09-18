using System;
using Microsoft.Extensions.DependencyInjection;
using CalendarTasks.Core.Calendar;
using CalendarTasks.Core.CalendarTaskNumbers;
using CalendarTasks.Defs.Calendar;
using CalendarTasks.Defs.CalendarTaskNumbers;
using CalendarTasks.Defs;

namespace CalendarTasks.Core.Infrastructure;

/// <summary>
/// Методы расширения для обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавление провайдера по умолчанию для даты и времени.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> контейнер.</param>
    /// <returns><see cref="IServiceCollection"/> контейнер с зарегистрированым провайдером даты и времени.</returns>
    public static IServiceCollection AddDefaultDateTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();
        return services;
    }

    /// <summary>
    /// Добавление генератора номера задачи по умолчанию.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> контейнер.</param>
    /// <returns><see cref="IServiceCollection"/> контейнер с зарегистрированым провайдером даты и времени.</returns>
    public static IServiceCollection AddDefaultCalendarTaskNumberGenerator(this IServiceCollection services)
    {
        services.AddSingleton<ICalendarTaskNumberGenerator, DefaultCalendarTaskNumberGenerator>();
        return services;
    }

    /// <summary>
    ///  Регистрация основных обработчиков
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <returns>контейнер с зарегестрированными основными сервисами.</returns>
    public static IServiceCollection AddCoreConsoleServices(this IServiceCollection services)
        => services.AddSingleton<ICalendarTaskHandler, CalendarTaskHandler>();   
}

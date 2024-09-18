using System;
using System.Security.Cryptography;
using CalendarTasks.Defs.Data;

namespace CalendarTasks.Defs.Extensions;

/// <summary>
/// Методы расшиирения для <see cref="CalendarTaskStatus"/>.
/// </summary>
public static class CalendarTaskStatusExtensions
{
    /// <summary>
    /// Возвращает читаемое имя статуса задачи.
    /// </summary>
    /// <param name="taskStatus">статус задачи.</param>
    /// <returns>читаемое имя статуса задачи.</returns>
    public static string GetPublicName(this CalendarTaskStatus taskStatus)
        => (taskStatus) switch
        {
            CalendarTaskStatus.Planned => "Запланирована",
            CalendarTaskStatus.InProgress => "В работе",
            CalendarTaskStatus.Done => "Завершенная",
            _ => "Не задан",            
        };
}

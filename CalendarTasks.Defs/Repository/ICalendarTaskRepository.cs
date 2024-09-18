using System;
using System.Data;
using CalendarTasks.Defs.Data;

namespace CalendarTasks.Defs.Repository;

/// <summary>
/// Интерфейс для хранения и запросов задач.
/// </summary>
public interface ICalendarTaskRepository
{
    /// <summary>
    /// Получение списка задач по фильтру.
    /// </summary>
    /// <param name="filter"><see cref="CalendarTaskFilter" /></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Список задач.</returns>
    Task<Dictionary<string, CalendarTask>> GetList(CalendarTaskFilter filter, CancellationToken cancellationToken);       

    /// <summary>
    /// Добавление задачи или изменение.
    /// </summary>
    /// <param name="task">задача.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns><see cref="Task"/> на операцию с задачей.</returns>
    Task AddOrUpdateCalendarTask(CalendarTask task, CancellationToken cancellationToken);    

    /// <summary>
    /// Удаление задачи.
    /// </summary>
    /// <param name="calendarTaskNumber">номер удаляемой задачи.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Ожидание удаление задачи.</returns>
    Task RemoveCalendarTask (string calendarTaskNumber, CancellationToken cancellationToken);
}

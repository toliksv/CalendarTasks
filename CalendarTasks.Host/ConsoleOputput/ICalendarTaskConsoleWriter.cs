using System;
using CalendarTasks.Defs.Data;

namespace CalendarTasks.Host.ConsoleOputput;

/// <summary>
///  ОБработчик вывода на консоль.
/// </summary>
public interface ICalendarTaskConsoleWriter
{
    /// <summary>
    /// Выводит на консоль список задач.
    /// </summary>
    /// <param name="calendarList">список задач.</param>
    /// <returns>ожидание вывода на консоль.</returns>
    ValueTask WriteCalendarTasksList(Dictionary<string, CalendarTask> calendarList);

    /// <summary>
    /// Выводит на консоль <see cref="CalendarTask"/>.
    /// </summary>
    /// <param name="calendarTask">выводлимая задача.</param>
    /// <returns>ожидание вывода.</returns>
    ValueTask WriteCalendarTask(CalendarTask calendarTask);
}

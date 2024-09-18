using System;

namespace CalendarTasks.Defs.Calendar;

/// <summary>
/// Провайдер даты и времени.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Текущая дата.
    /// </summary>
    DateTime CurrentDate { get;}
}

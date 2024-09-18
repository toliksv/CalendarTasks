using System;
using CalendarTasks.Defs.Calendar;

namespace CalendarTasks.Core.Calendar;

/// <summary>
/// Провайдер даты / времени по-умолчанию.
/// </summary>
internal class DefaultDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc cref="IDateTimeProvider" />    
    public DateTime CurrentDate => DateTime.Now;
}

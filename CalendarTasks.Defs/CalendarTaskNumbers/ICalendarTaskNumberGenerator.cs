namespace CalendarTasks.Defs.CalendarTaskNumbers;

/// <summary>
/// Генерация номера задачи.
/// </summary>
public interface ICalendarTaskNumberGenerator
{
    /// <summary>
    /// Генерация номера для задачи.
    /// </summary>
    /// <returns>номер задачи.</returns>
    string GenerateCalendarTaskNumber();
}

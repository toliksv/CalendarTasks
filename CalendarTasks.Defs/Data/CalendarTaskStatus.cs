namespace CalendarTasks.Defs.Data;

/// <summary>
/// Статус задачи.
/// </summary>
[Flags]
public enum CalendarTaskStatus : int
{
    /// <summary>
    /// Не задан.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Запланирована.
    /// </summary>
    Planned = 1,

    /// <summary>
    /// Выполняется.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Выполнена.
    /// </summary>
    Done = 4,

    /// <summary>
    /// Незавершенные задачи.
    /// </summary>
    Incomplete = Planned | InProgress,
}
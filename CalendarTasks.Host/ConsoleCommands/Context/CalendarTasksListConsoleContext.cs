
using CalendarTasks.Defs.Data;

namespace CalendarTasks.Host.ConsoleCommands.Context;

/// <summary>
/// Контекст команды списка задач по фильтру.
/// </summary>
internal class CalendarTasksListConsoleContext : IConsoleCommandContext
{ 
        /// <summary>
    /// Номер задачи.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// Дата начала периода.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода.
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Статус задачи.
    /// </summary>
    public CalendarTaskStatus Status { get; set; }  = CalendarTaskStatus.Undefined;
}

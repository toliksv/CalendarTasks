
namespace CalendarTasks.Host.ConsoleCommands.Context;

/// <summary>
/// Контекст добавления задачи.
/// </summary>
internal class AddCalendarTaskConsoleCommandContext : IConsoleCommandContext
{  
    /// <summary>
    /// Заголовок. 
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Дата выполнения задачи.
    /// </summary>
    public DateTime? DueDate { get; set; }
}

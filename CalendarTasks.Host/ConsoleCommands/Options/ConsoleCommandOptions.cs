namespace CalendarTasks.Host.ConsoleCommands.Options;

/// <summary>
/// Опции выполнения консольной команды.
/// </summary>
public class ConsoleCommandOptions
{
    /// <summary>
    /// Таймаут консольной команды.
    /// </summary>
    public TimeSpan ConsoleCommandTimeout { get; set; } = TimeSpan.FromSeconds(15);
}

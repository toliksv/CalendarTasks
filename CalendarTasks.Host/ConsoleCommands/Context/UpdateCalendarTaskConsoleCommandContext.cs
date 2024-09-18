using System;

namespace CalendarTasks.Host.ConsoleCommands.Context;

internal class UpdateCalendarTaskConsoleCommandContext : IConsoleCommandContext
{      
    /// <summary>
    /// Номер задачи.
    /// </summary>
    public string Number { get; set;}   
}

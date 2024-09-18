using System;
using CalendarTasks.Host.ConsoleCommands.Commands;

namespace CalendarTasks.Host.ConsoleCommands.Providers;

/// <summary>
/// Провайдер команды.
/// </summary>
public interface IConsoleCommandProvider
{
    /// <summary>
    /// Возвразает консольную команду.
    /// </summary>
    /// <param name="commandName">имя команды.</param>
    /// <returns></returns>
    IConsoleCommand GetConsoleCommand(string commandName);
}

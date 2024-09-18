using System;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

/// <summary>
/// Представляет собой команду консоли.
/// </summary>
public interface IConsoleCommand
{
    /// <summary>
    /// Имя команды.
    /// </summary>
    string CommandName { get; }

    /// <summary>
    /// Вывод помощи по команде.
    /// </summary>
    void Help();

    /// <summary>
    /// Выполнение команды
    /// </summary>
    /// <returns>ожидание выполнения.</returns>
    Task Execute(CancellationToken cancellationToken);
}

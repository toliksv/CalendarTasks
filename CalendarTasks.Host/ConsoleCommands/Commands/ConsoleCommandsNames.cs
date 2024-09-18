using System;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

/// <summary>
/// Названия команд для консоли.
/// </summary>
internal class ConsoleCommandsNames
{
    /// <summary>
    /// Команда помощи.
    /// </summary>
    public const string Help = "help";

    /// <summary>
    /// Добавление задачи.
    /// </summary>
    public const string Add = "add";

    /// <summary>
    /// Получение списка задач.
    /// </summary>
    public const string List = "list";

    /// <summary>
    /// Список актуальных задач.
    /// </summary>
    public const string ActualList = "actual-list";

    /// <summary>
    /// Изменение задачию.
    /// </summary>
    public const string Update = "update";

    /// <summary>
    /// Комада отмены или выхода.
    /// </summary>
    public const string Exit = "exit";
}

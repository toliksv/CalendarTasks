using System;

namespace CalendarTasks.FileRepository;

/// <summary>
/// Настройки для файлового репозитория задач.
/// </summary>
public class CalendarTaskFileRepositoryOptions
{
    /// <summary>
    /// Пусть к директории с файлом задач.
    /// </summary>
    public string DirectoryPath { get; set; } = "..\\..\\";

    /// <summary>
    /// Имя файла содержащих задачи.
    /// </summary>
    public string FileName { get; set; } = "CalendarTasks.jsn";
}

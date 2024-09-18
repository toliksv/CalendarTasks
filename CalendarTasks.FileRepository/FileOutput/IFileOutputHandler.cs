using System;
using System.ComponentModel;
using CalendarTasks.Defs.Data;

namespace CalendarTasks.FileRepository.FileOutput;

/// <summary>
/// Абстракция работы с файлом.
/// </summary>
internal interface IFileOutputHandler
{
    /// <summary>
    /// Сохраняет задачи в файл.
    /// </summary>
    /// <param name="tasks">задачи.</param>
    /// <param name="token">токен отмены.</param>
    /// <returns>ожидание сохранения списка задач.</returns>
    Task SaveCalendarTasks(Dictionary<string, CalendarTask> tasks, CancellationToken token);

    /// <summary>
    /// Прочитать задачи из файла.
    /// </summary>
    /// <param name="token">токен отмены.</param>
    /// <returns>список прочитанных задач.</returns>
    Task<List<CalendarTask>> GetCalendarTasks(CancellationToken token);
}

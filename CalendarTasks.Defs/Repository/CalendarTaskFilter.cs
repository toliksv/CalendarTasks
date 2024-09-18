using CalendarTasks.Defs.Data;

namespace CalendarTasks.Defs.Repository;

/// <summary>
/// Фильтр для получения списка задач.
/// </summary>
public class CalendarTaskFilter
{
    /// <summary>
    /// Номер задачи.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// Статус задачи.
    /// </summary>
    public CalendarTaskStatus Status { get; set; }

    /// <summary>
    /// Период завершения задачи, дата начала периода.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Период завершения задачи, дата окончания периода.
    /// </summary>
    public DateTime? DateTo { get; set; }
}
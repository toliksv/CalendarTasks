namespace CalendarTasks.Defs.Data;

/// <summary>
/// Задача.
/// </summary>
public class CalendarTask
{
    /// <summary>
    /// Номер задачи.
    /// </summary>
    public string Number { get; set;} = string.Empty;

    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Header { get; set;} = string.Empty;

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string Description { get; set;} = string.Empty;

    /// <summary>
    /// Срок завершения задачи.
    /// </summary>
    public DateTime? DueDate { get; set;}

   /// <summary>
   /// Статус задачи.
   /// </summary>   
    public CalendarTaskStatus Status{ get; set;} 

    /// <inheritdoc />    
    public override string ToString()=>$"{Number} {Status} {Header} {Description} {DueDate?.ToShortDateString() ?? "без срока"}";
}
 
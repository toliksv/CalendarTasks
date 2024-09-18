using CalendarTasks.Defs.Data;


namespace CalendarTasks.Defs
{
    /// <summary>
    /// Работа с таском.
    /// </summary>
    public interface ICalendarTaskHandler
    {
        /// <summary>
        /// Возвращает задачу.
        /// </summary>
        /// <param name="number">номер задачи.</param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns><see cref="CalendarTask"/> - задача по заданному номеру.</returns>
        Task<CalendarTask> GetCalendarTask(string number, CancellationToken cancellationToken);

        /// <summary>
        /// Список актуальных задач.
        /// </summary>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns>ожидание вывода списка.</returns>
        Task<Dictionary<string, CalendarTask>> GetActualCalendarTaskList(CancellationToken cancellationToken);

        /// <summary>
        /// Отобразить список задач по параметрам.
        /// </summary>
        /// <param name="number">номер задачи.</param>
        /// <param name="header">заголовок.</param>
        /// <param name="dateFrom">Период завершения, дата начала.</param>
        /// <param name="dateTo">Период окончания, дата окончания.</param>
        /// <param name="calendarTaskStatus">статус задач.</param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns></returns>
        Task<Dictionary<string, CalendarTask>> GetCalendarTaskList(string number, string header, DateTime? dateFrom, DateTime? dateTo, CalendarTaskStatus calendarTaskStatus, CancellationToken cancellationToken);

        /// <summary>
        /// Добавить задачу.
        /// </summary>
        /// <param name="header">Заголовок.</param>
        /// <param name="description">Описание.</param>
        /// <param name="dueDate">дата выполнения.</param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns>Номер созданной задачи.</returns>
        Task<string> AddCalendarTask(string header, string description, DateTime? dueDate, CancellationToken cancellationToken);

        /// <summary>
        /// Изменение задачи по ее номеру.
        /// </summary>
        /// <param name="number">номер задачи.</param>
        /// <param name="header">заголовок.</param>
        /// <param name="description">описание.</param>
        /// <param name="dueDate">дата исполнения.</param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns><see cref="Task"/> ожидание изменения задачи.</returns>
        Task UpdateCalendarTask(string number, string header, string description, DateTime? dueDate, CancellationToken cancellationToken);

        /// <summary>
        /// Взять задачу в работу.
        /// </summary>
        /// <param name="number">номер задачи.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> токен отмены.</param>
        /// <returns><see cref="Task"/> ожидание выполнения процесса взятия задачи в работу.</returns>    
        Task GetToWork(string number, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление задачи.
        /// </summary>
        /// <param name="number">номер задачи.</param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns>ожидание отмены задачи.</returns>
        Task RemoveCalendarTask(string number, CancellationToken cancellationToken);
    }
}
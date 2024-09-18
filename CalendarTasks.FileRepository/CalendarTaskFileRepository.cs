using System.Linq;
using CalendarTasks.Defs.Data;
using CalendarTasks.Defs.Repository;
using CalendarTasks.FileRepository.FileOutput;
using Microsoft.Extensions.Logging;


namespace CalendarTasks.FileRepository;

/// <summary>
/// Хранение задачи в файле.
/// </summary>
internal class CalendarTaskFileRepository : ICalendarTaskRepository
{   
    private readonly Dictionary<string, CalendarTask> _storage;

    private readonly IFileOutputHandler _fileOutputHandler;
    private readonly ILogger<CalendarTaskFileRepository> _logger;

    public CalendarTaskFileRepository(ILogger<CalendarTaskFileRepository> logger, IFileOutputHandler fileOutputHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileOutputHandler = fileOutputHandler ?? throw new ArgumentNullException(nameof(fileOutputHandler));             
        _storage = new Dictionary<string, CalendarTask>();
    }

    

    public async Task AddOrUpdateCalendarTask(CalendarTask task, CancellationToken cancellationToken)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (string.IsNullOrWhiteSpace(task.Number))
        {
            // номер генерим в Core. Здесь для сохранения должна уже быть с номером.
            throw new InvalidOperationException("Сохранение задачи без номера невозможно!");
        }

        /// наполняем storage, если он пустой.
        if (_storage.Count == 0)
        {
            await LoadStorage(cancellationToken);
        }

        if (!_storage.TryAdd(task.Number, task))
        {
            // изменям задачу с сохранением статуса.
            var oldTask = _storage[task.Number];            
            task.Status = task.Status == CalendarTaskStatus.Undefined ? oldTask.Status : task.Status;
            _storage[task.Number] = task;
        }

        await SaveTasks(cancellationToken);
    }

    public async Task<Dictionary<string, CalendarTask>> GetList(CalendarTaskFilter filter, CancellationToken cancellationToken)
    {
        if (_storage.Count == 0)
        {
            var taskList = await _fileOutputHandler.GetCalendarTasks(cancellationToken);
        
             // сначала заполняем _storage;
            FillStorage(taskList);
        }

        // проверяем фильтр
        if (string.IsNullOrWhiteSpace(filter.Number) 
            && string.IsNullOrWhiteSpace(filter.Header)
            && filter.Status == CalendarTaskStatus.Undefined
            && !filter.DateFrom.HasValue
            && !filter.DateTo.HasValue)
        {
            throw new InvalidOperationException($"Должно быть задано хотя-бы одно свойство {nameof(filter)}");
        }

        // фильтруем
        var filtered = _storage.Values.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(filter.Number))
        {
            filtered = filtered.Where(x => x.Number.Contains(filter.Number, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Header))
        {
            filtered = filtered.Where(x => x.Header.Contains(filter.Header, StringComparison.InvariantCultureIgnoreCase));
        }

        if (filter.DateFrom.HasValue)
        {
            filtered = filtered.Where(x => x.DueDate >= filter.DateFrom);
        }

        if (filter.DateTo.HasValue)
        {
            filtered = filtered.Where(x => x.DueDate <= filter.DateTo);
        }

        if (filter.Status != CalendarTaskStatus.Undefined)
        {
            filtered = filtered.Where(x => filter.Status.HasFlag(x.Status));
        }

        /// почему-то в только в таком виде... должно быть filtered.ToDictionary, нет времени разбираться.
        return Enumerable.ToDictionary(filtered, itm => itm.Number);        
    }

    public async Task RemoveCalendarTask(string calendarTaskNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(calendarTaskNumber))
        {
            throw new ArgumentException($"'{nameof(calendarTaskNumber)}' cannot be null or whitespace.", nameof(calendarTaskNumber));
        }

        if (_storage.Count == 0)
        {
            await LoadStorage(cancellationToken);
        }

        if (_storage.Remove(calendarTaskNumber))
        {
            _logger.LogInformation("Задача номер {TaskNumber} была удалена из списка", calendarTaskNumber);
            await SaveTasks(cancellationToken);
        }
    }

    private async Task SaveTasks(CancellationToken cancellationToken)
    {
        await _fileOutputHandler.SaveCalendarTasks(_storage, cancellationToken);
        _logger.LogInformation("Список задач был сохранен");
    }

    private async Task LoadStorage(CancellationToken cancellationToken)
    {

        var fileList =  await _fileOutputHandler.GetCalendarTasks(cancellationToken);
        _storage.Clear();
        FillStorage(fileList);
        _logger.LogInformation("Задачи загружены из  файла");        
    }

    private void FillStorage(List<CalendarTask> tasks)
    {
        if (tasks is null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        foreach (var task in tasks)
        {
            AddOrUpdateTaskInStorage(task);
        }
    }

    private void AddOrUpdateTaskInStorage(CalendarTask task)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (!_storage.TryAdd(task.Number, task))
        {
            _storage[task.Number] = task;
        }
    }
}

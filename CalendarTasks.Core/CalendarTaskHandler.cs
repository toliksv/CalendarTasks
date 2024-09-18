using CalendarTasks.Defs;
using CalendarTasks.Defs.Calendar;
using CalendarTasks.Defs.CalendarTaskNumbers;
using CalendarTasks.Defs.Data;
using CalendarTasks.Defs.Repository;


namespace CalendarTasks.Core;

internal class CalendarTaskHandler : ICalendarTaskHandler
{
    private readonly ICalendarTaskRepository _calendarTaskRepository;

    private readonly ICalendarTaskNumberGenerator _calendarTaskNumberGenerator;

    private readonly IDateTimeProvider _dateTimeProvider;

    public CalendarTaskHandler(ICalendarTaskRepository calendarTaskRepository, ICalendarTaskNumberGenerator calendarTaskNumberGenerator, IDateTimeProvider dateTimeProvider)
    {
        _calendarTaskRepository = calendarTaskRepository ?? throw new ArgumentNullException(nameof(calendarTaskRepository));
        _calendarTaskNumberGenerator = calendarTaskNumberGenerator ?? throw new ArgumentNullException(nameof(calendarTaskNumberGenerator));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(IDateTimeProvider));
    }

    public async Task<CalendarTask> GetCalendarTask(string number, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException($"'{nameof(number)}' не может быть пустым и равным null");
        }
        
        var filter = new CalendarTaskFilter
        {
            Number = number,
        };
        var dictionary = await _calendarTaskRepository.GetList(filter, cancellationToken);
        if (dictionary.TryGetValue(number, out var calendarTask))
        {
            return calendarTask;
        }
        
        return null;
    }

    public async Task<string> AddCalendarTask(string header, string description, DateTime? dueDate, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(header))        
        {
            throw new ArgumentException($"{nameof(header)} не может быть пустым и должен содержать заголовок задачи!");
        }

        var number = _calendarTaskNumberGenerator.GenerateCalendarTaskNumber(); 
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new InvalidOperationException("Не удалось сгенерить номер для задачи! Номер получился пустым");
        }

        var task = new CalendarTask
        {
            Number = number,
            Header = header,
            Description = description,
            DueDate = dueDate,
            Status = CalendarTaskStatus.Planned, 
        };
        await _calendarTaskRepository.AddOrUpdateCalendarTask(task, cancellationToken);
        return number;
    }

    public Task RemoveCalendarTask(string number, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException($"'{nameof(number)}' не может быть пустым или равным null.", nameof(number));
        }

        return _calendarTaskRepository.RemoveCalendarTask(number, cancellationToken);
    }

    public Task<Dictionary<string, CalendarTask>> GetActualCalendarTaskList(CancellationToken cancellationToken = default)
    {
        // возращаем список незаконченных задач и список задач с датой окончания в ближайщий месяц
        var maxDueDate = _dateTimeProvider.CurrentDate.AddMonths(1);       
        return GetCalendarTaskList(null, null, null, maxDueDate, CalendarTaskStatus.Incomplete, cancellationToken);        
    }

    public Task<Dictionary<string, CalendarTask>> GetCalendarTaskList(string number, string header, DateTime? dateFrom, DateTime? dateTo, CalendarTaskStatus calendarTaskStatus, CancellationToken cancellationToken = default)
    {
        var filter = new CalendarTaskFilter
        {
            Number = number,
            Header = header, 
            DateFrom = dateFrom,
            DateTo = dateTo,
            Status = calendarTaskStatus,
        };        
        return _calendarTaskRepository.GetList(filter, cancellationToken);
    }

    public Task GetToWork(string number, CancellationToken cancellationToken = default)
    {        
        return UpdateCalendarTask(number, null, null, null, CalendarTaskStatus.InProgress, cancellationToken);
    }

    public Task UpdateCalendarTask(string number, string header, string description, DateTime? dueDate, CancellationToken cancellationToken = default)
    {
        return UpdateCalendarTask(number, header, description, dueDate, CalendarTaskStatus.Undefined, cancellationToken);
    }

    private Task UpdateCalendarTask(string number, string header, string description, DateTime? dueDate, CalendarTaskStatus status, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException($"'{nameof(number)}' не может быть пустым или равным null.", nameof(number));
        }

        if (string.IsNullOrWhiteSpace(header))
        {
            throw new ArgumentException($"'{nameof(header)}' не может быть пустым или равным null.", nameof(header));
        }

        var updatedTask = new CalendarTask
        {
            Number = number,
            Description = description,
            Header = header,
            DueDate = dueDate,
            Status = status,
        };
        return _calendarTaskRepository.AddOrUpdateCalendarTask(updatedTask, cancellationToken);
    }
}

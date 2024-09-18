using CalendarTasks.Defs.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CalendarTasks.FileRepository.FileOutput;

internal class FileOutputHandler : IFileOutputHandler
{
    private readonly CalendarTaskFileRepositoryOptions _repositoryOptions;
    private readonly ILogger<FileOutputHandler> _logger;

    public FileOutputHandler(IOptions<CalendarTaskFileRepositoryOptions> repositoryOptions, ILogger<FileOutputHandler> logger)
    {
        _repositoryOptions = repositoryOptions?.Value ?? throw new ArgumentNullException(nameof(repositoryOptions));
        if (string.IsNullOrWhiteSpace(repositoryOptions.Value.FileName))
        {
            throw new InvalidOperationException("Не задан файл для сохранения задач!");
        }

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<CalendarTask>> GetCalendarTasks(CancellationToken token)
    {   
        var calendarTasksFile = GetCalendarTaskFile();

        if (!File.Exists(calendarTasksFile))
        {
            var emptyDictionary = new Dictionary<string, CalendarTask>();
            await SaveCalendarTasks(emptyDictionary, token);
            _logger.LogDebug("Создан файл {FileName}", calendarTasksFile);
            return new List<CalendarTask>();
        }

        using(var stream = File.OpenText(calendarTasksFile))
        using(var reader = new JsonTextReader(stream))
        {
            var json = await JArray.LoadAsync(reader, token);
            var array = json.ToObject<List<CalendarTask>>();
            _logger.LogDebug("Успешно прочитан файл {FileName}", calendarTasksFile);
            return array;    
        }        
    }

    public async Task SaveCalendarTasks(Dictionary<string, CalendarTask> tasks, CancellationToken token)
    {         
        var fileName = GetCalendarTaskFile();

        using var stream = File.CreateText(fileName);
        using var writer = new JsonTextWriter(stream);
        var array = JArray.FromObject(tasks.Values.ToArray());
        await array.WriteToAsync(writer, token);
        // метод асинхронный только из-за логирования. так-бы просто задачу вернул.
        _logger.LogDebug("Успешно перезаписан файл {FileName}", fileName);
    }

    private string GetCalendarTaskFile()
        => Path.Combine(_repositoryOptions.DirectoryPath, _repositoryOptions.FileName);

}

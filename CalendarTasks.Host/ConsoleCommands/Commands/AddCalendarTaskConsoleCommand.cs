using System;
using System.Net.Http.Headers;
using System.Text;
using CalendarTasks.Defs;
using CalendarTasks.Host.ConsoleCommands.Arguments;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleCommands.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

/// <summary>
/// Команда добавления задачи в файл.
/// </summary>
internal class AddCalendarTaskConsoleCommand : ConsoleCommandBase<AddCalendarTaskConsoleCommandContext>
{
    private readonly ILogger<AddCalendarTaskConsoleCommand> _logger;

    public AddCalendarTaskConsoleCommand(ICalendarTaskHandler handler,  IOptions<ConsoleCommandOptions> options, ILogger<AddCalendarTaskConsoleCommand> logger)
    :base(handler, options, logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string CommandName => ConsoleCommandsNames.Add;

    protected override AddCalendarTaskConsoleCommandContext CreateContextFromConsole()
    {      

        var context = new AddCalendarTaskConsoleCommandContext();        
        Console.WriteLine("Добавление новой задачи");
        if (!ConsoleArgumentsExtensions.TryRequestRequriedArgument("Введите заголовок задачи:", out var header))
        {
            Console.WriteLine("Не получено обязательное значение или введена команда отмены. Выход из команды {0}", ConsoleCommandsNames.Add);
            return null;    
        }

        context.Header = header;
        context.Description = ConsoleArgumentsExtensions.RequestArgument("Введите описание задачи:");
        if (!ConsoleArgumentsExtensions.TryRequestDateTimeArgument("Введите дату завершения задачи:", out var dueDate))
        {
            Console.WriteLine("Не указана дата в корректном формате или получены команда отмены. Выход из комадны {0}", ConsoleCommandsNames.Add);
            return null;
        }

        context.DueDate = dueDate;
        return context;      
    }

    protected override async Task ExcetuteInternal(ICalendarTaskHandler hander, AddCalendarTaskConsoleCommandContext commandContext, CancellationToken cancellationToken)
    {
        var taskNumber = await hander.AddCalendarTask(commandContext.Header, commandContext.Description, commandContext.DueDate, cancellationToken);
        _logger.LogInformation("Создана задача с номером {CalendarTaskNumber}", taskNumber);   
    }

    public override void Help() 
        => Console.Write(HelpText);
    

    private const string HelpText = @$"Команда {ConsoleCommandsNames.Add} добавление новой задачи
        Аргументы:
            - Заголовок задачи, обязательный;
            - Описание задачи, не обязательно;
            - Дата завершения задачи. Формат {{гггг-ММ-дд}}, не обязательно";
}

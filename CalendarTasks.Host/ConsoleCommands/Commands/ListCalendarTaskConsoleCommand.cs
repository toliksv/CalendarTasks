using System;
using CalendarTasks.Defs;
using CalendarTasks.Defs.Data;
using CalendarTasks.Defs.Extensions;
using CalendarTasks.Host.ConsoleCommands.Arguments;
using CalendarTasks.Host.ConsoleCommands.Context;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleOputput;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

internal class ListCalendarTasksConsoleCommand : ConsoleCommandBase<CalendarTasksListConsoleContext>
{
    private readonly ICalendarTaskConsoleWriter _calendarTaskConsoleWriter;

    public ListCalendarTasksConsoleCommand(ICalendarTaskHandler calendarTaskHandler, IOptions<ConsoleCommandOptions> options, ILogger<ListCalendarTasksConsoleCommand> logger, ICalendarTaskConsoleWriter calendarTaskConsoleWriter)
    : base(calendarTaskHandler, options, logger)
    {
        if (calendarTaskHandler is null)
        {
            throw new ArgumentNullException(nameof(calendarTaskHandler));
        }

        this._calendarTaskConsoleWriter = calendarTaskConsoleWriter;
    }

    public override string CommandName => ConsoleCommandsNames.List;

    public override void Help()
        => Console.WriteLine(HelpText);

    protected override CalendarTasksListConsoleContext CreateContextFromConsole()
    {
        var context = new CalendarTasksListConsoleContext();        
        Console.WriteLine("Список задач согласно фильтру");
        context.Number = ConsoleArgumentsExtensions.RequestArgument("Укажите номер задачи:");         
        context.Header = ConsoleArgumentsExtensions.RequestArgument("Укажите заголовок задачи:");
        if (ConsoleArgumentsExtensions.TryRequestDateTimeArgument("Укажите дату начала периода:", out var dateFrom))
        {
            context.DateFrom = dateFrom;
        }

        if (ConsoleArgumentsExtensions.TryRequestDateTimeArgument("Укажите дату окончания периода:", out var dateTo))
        {
            context.DateTo = dateTo;
        }

        var retryCount = 5;
        var counter = 0;
        int? argumentValue = null;
        while (counter < retryCount && !argumentValue.HasValue)
        {
            if (ConsoleArgumentsExtensions.RequestIntegerArgument("Укажите статус задачи (1 - планируется, 2-Выполняется, 4-Выполнена)", out argumentValue))
            {
                if (!argumentValue.HasValue)
                {
                    // если ничего не ввели
                    break;
                }   

                if (!Enum.IsDefined(typeof(CalendarTaskStatus), argumentValue) || argumentValue == 0)
                {
                    Console.WriteLine("Указан неизвестный статус задачи! Значение учитываться не будет");
                    argumentValue = null;
                    continue;
                }                   

                context.Status = (CalendarTaskStatus)argumentValue;
                break;
            }

            counter++;
        }

        return context;
    }

    protected override async Task ExcetuteInternal(ICalendarTaskHandler hander, CalendarTasksListConsoleContext commandContext, CancellationToken cancellationToken)
    {
        var tasksList = await hander.GetCalendarTaskList(commandContext.Number, commandContext.Header, commandContext.DateFrom, commandContext.DateTo, commandContext.Status, cancellationToken);
        if (tasksList != null)
        {
            await _calendarTaskConsoleWriter.WriteCalendarTasksList(tasksList);
        }        
    }

    private const string HelpText =$@"Вывод список задач по фильтру:
        Номер задачи - номер задачи
        Заголовок - заголовок задачи, команда ищет вхождения, поэтому заголовок может быть н полным
        Дата начала - Формат {{гггг-ММ-дд}}, Дата начала периода в который задачи должны быть выполнены
        Дата окончания - Формат {{гггг-ММ-дд}}, Дата окончания периода в который задачи должны быть выполнены
        Статус задачи:
             1 - Planned - запланировано,
             2 - InProgress - в работе,
             4 - Выполнена.";    
}

using CalendarTasks.Defs;
using CalendarTasks.Host.ConsoleCommands.Context;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleOputput;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

internal class ListActualCalendarTaskConsoleCommand : ConsoleCommandBase<DefaultConsoleCommandContext>
{
    public override string CommandName => ConsoleCommandsNames.ActualList;
    private readonly ICalendarTaskConsoleWriter _calendarTaskConsoleWriter;

    public ListActualCalendarTaskConsoleCommand(ICalendarTaskHandler calendarTaskHandler, IOptions<ConsoleCommandOptions> options, ILogger<ListActualCalendarTaskConsoleCommand> logger, ICalendarTaskConsoleWriter calendarTaskConsoleWriter) : base(calendarTaskHandler, options, logger)
    {
        _calendarTaskConsoleWriter = calendarTaskConsoleWriter ?? throw new ArgumentNullException(nameof(calendarTaskConsoleWriter));
    }

    public override void Help()
     => Console.WriteLine(HelpText);

    protected override DefaultConsoleCommandContext CreateContextFromConsole()
     => new DefaultConsoleCommandContext();

    protected override async Task ExcetuteInternal(ICalendarTaskHandler hander, DefaultConsoleCommandContext commandContext, CancellationToken cancellationToken)
    {
        var taskList = await hander.GetActualCalendarTaskList(cancellationToken);
        if (taskList != null)
        {
            await _calendarTaskConsoleWriter.WriteCalendarTasksList(taskList);
        }
    }

    private const string HelpText = $@"Список актуальных задач
     Выводит незавершенные задачи, которые должны быть исполнены в ближайщий месяц.";    
}

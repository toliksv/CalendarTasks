using System;
using CalendarTasks.Defs;
using CalendarTasks.Host.ConsoleCommands.Context;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleCommands.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace CalendarTasks.Host.ConsoleCommands.Commands.Help;

internal class HelpConsoleCommand : ConsoleCommandBase<HelpConsoleCommandContext>, IHelpConsoleCommand
{
    
    private readonly IConsoleCommandProvider _consoleCommandProvider;

    public HelpConsoleCommand(IConsoleCommandProvider consoleCommandProvider, ICalendarTaskHandler calendarTaskHandler, IOptions<ConsoleCommandOptions> options, ILogger<HelpConsoleCommand> logger) : base(calendarTaskHandler, options, logger)
    {        
        _consoleCommandProvider = consoleCommandProvider ?? throw new ArgumentNullException(nameof(consoleCommandProvider));    
    }

    public override string CommandName => ConsoleCommandsNames.Help;

    public override void Help()
     => Console.Write(HelpText);
    

    protected override HelpConsoleCommandContext CreateContextFromConsole()
    {
        Help();
        return new HelpConsoleCommandContext();
    }
        

    protected override Task ExcetuteInternal(ICalendarTaskHandler hander, HelpConsoleCommandContext commandContext, CancellationToken cancellationToken)
    {
        var command = string.Empty;
        while (!ConsoleCommandsNames.Exit.Equals(command, StringComparison.InvariantCultureIgnoreCase))        
        {
            Console.WriteLine("Укажите команду для получения помощи:");
            command = Console.ReadLine();
            if (ConsoleCommandsNames.Help.Equals(command, StringComparison.InvariantCultureIgnoreCase))
            {
                Help();
                continue;
            }               

            var consoleCommand  = _consoleCommandProvider.GetConsoleCommand(command);
            consoleCommand?.Help();        
        }

        return Task.CompletedTask;        
    }

    private const string HelpText = $@"Консольная утилита для работы с пользовательскими задачами
        Для получения помощи по каждой команде наберите имя_команды. Для отображения этого сообщения наберите {ConsoleCommandsNames.Help}
        Для выхода из помощи наберите {ConsoleCommandsNames.Exit}
        Список возможных команд:
        {ConsoleCommandsNames.Add} -  добавление новой задачи
        {ConsoleCommandsNames.List} - список задач по фильтру
        {ConsoleCommandsNames.ActualList} - список актуальных задач
        {ConsoleCommandsNames.Update} - изменение задачи" ;
}

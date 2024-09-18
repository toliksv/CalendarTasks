using System;
using CalendarTasks.Host.ConsoleCommands.Commands;
using CalendarTasks.Host.ConsoleCommands.Commands.Help;
using CalendarTasks.Host.ConsoleCommands.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CalendarTasks.Host.ConsoleCommands;

public class ConsoleCommandHandler : BackgroundService
{
    private readonly IConsoleCommandProvider _consoleCommandProvider;
    private readonly IHelpConsoleCommand _helpConsoleCommand;
    private readonly ILogger<ConsoleCommandHandler> _logger;

    public ConsoleCommandHandler(IConsoleCommandProvider consoleCommandProvider, ILogger<ConsoleCommandHandler> logger, IHelpConsoleCommand helpConsoleCommand)
    {
        _consoleCommandProvider = consoleCommandProvider ?? throw new ArgumentNullException(nameof(consoleCommandProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _helpConsoleCommand = helpConsoleCommand ?? throw new ArgumentNullException(nameof(helpConsoleCommand));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {      
        var consoleCommand = string.Empty;
        while (!(cancellationToken.IsCancellationRequested || ConsoleCommandsNames.Exit.Equals(consoleCommand, StringComparison.InvariantCultureIgnoreCase)))
        {
            try
            {
                consoleCommand = Console.ReadLine();

                if (_helpConsoleCommand.CommandName.Equals(consoleCommand, StringComparison.InvariantCultureIgnoreCase))
                {
                    await _helpConsoleCommand.Execute(cancellationToken);
                    continue;
                }

                var command = _consoleCommandProvider.GetConsoleCommand(consoleCommand);               

                if (command == null)
                {
                    _logger.LogWarning("Не найдена команда {ConsoleCommand}", consoleCommand);
                    continue;
                }   

                await command.Execute(cancellationToken);             
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при выполнении команды");                
            }
        }    
    }    
}

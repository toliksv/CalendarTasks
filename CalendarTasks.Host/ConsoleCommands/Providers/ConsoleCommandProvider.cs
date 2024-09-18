using System;
using CalendarTasks.Host.ConsoleCommands.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CalendarTasks.Host.ConsoleCommands.Providers;

internal class ConsoleCommandProvider : IConsoleCommandProvider
{
    private readonly IEnumerable<IConsoleCommand> _consoleCommands;
    private readonly ILogger<ConsoleCommandProvider> _logger;

    public ConsoleCommandProvider(IEnumerable<IConsoleCommand>  consoleCommands, ILogger<ConsoleCommandProvider> logger)
    {
        _consoleCommands = consoleCommands ?? throw new ArgumentNullException(nameof(consoleCommands));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IConsoleCommand GetConsoleCommand(string commandName)
    {
        if (string.IsNullOrWhiteSpace(commandName))
        {
            throw new ArgumentException($"Имя команды не может быть пустым!");
        }                           
        
        return _consoleCommands.FirstOrDefault(comm => comm.CommandName.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));        
    }  
    

}

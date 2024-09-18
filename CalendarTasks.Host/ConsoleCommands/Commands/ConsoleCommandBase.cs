using System;
using CalendarTasks.Defs;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleCommands.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

/// <summary>
/// Консольная команда.
/// </summary>
public abstract class ConsoleCommandBase<TCommandContext> : IConsoleCommand 
    where TCommandContext : IConsoleCommandContext
{
    private readonly ICalendarTaskHandler _calendarTaskHandler;
    private readonly ILogger _logger;
    private readonly ConsoleCommandOptions _options;
    
    protected ConsoleCommandBase(ICalendarTaskHandler calendarTaskHandler, IOptions<ConsoleCommandOptions> options, ILogger logger)
    {
        _calendarTaskHandler = calendarTaskHandler ?? throw new ArgumentNullException(nameof(calendarTaskHandler));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public abstract string CommandName { get; }

    /// <summary>
    /// Выполнение консольной команды.
    /// </summary>
    /// <param name="cancellationToken">токе отмены.</param>
    /// <returns></returns>
    public async Task Execute(CancellationToken cancellationToken)
    {      
        try
        {
            var context = CreateContextFromConsole();
            if (context == null)
            {
                    return;
            }
                
            await ExcetuteInternal(_calendarTaskHandler, context, cancellationToken); 
            
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при выполнении команды {ConsoleCommandName}", this.GetType().Name);
        } 
    }

    /// <summary>
    ///  Помощь по комаде.
    /// </summary>
    public abstract void Help();

    protected abstract Task ExcetuteInternal(ICalendarTaskHandler hander, TCommandContext commandContext, CancellationToken cancellationToken);

    protected abstract TCommandContext CreateContextFromConsole();    
}

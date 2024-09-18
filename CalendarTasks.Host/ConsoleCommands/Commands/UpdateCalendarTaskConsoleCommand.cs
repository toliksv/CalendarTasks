using System;
using CalendarTasks.Defs;
using CalendarTasks.Host.ConsoleCommands.Arguments;
using CalendarTasks.Host.ConsoleCommands.Context;
using CalendarTasks.Host.ConsoleCommands.Options;
using CalendarTasks.Host.ConsoleOputput;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalendarTasks.Host.ConsoleCommands.Commands;

internal class UpdateCalendarTaskConsoleCommand : ConsoleCommandBase<UpdateCalendarTaskConsoleCommandContext>
{
    private readonly ICalendarTaskConsoleWriter _calendarTaskConsoleWriter;

    public UpdateCalendarTaskConsoleCommand(ICalendarTaskHandler calendarTaskHandler, IOptions<ConsoleCommandOptions> options, ILogger<UpdateCalendarTaskConsoleCommand> logger, ICalendarTaskConsoleWriter calendarTaskConsoleWriter) : base(calendarTaskHandler, options, logger)
    {
        _calendarTaskConsoleWriter = calendarTaskConsoleWriter ?? throw new ArgumentNullException(nameof(calendarTaskConsoleWriter));
    }

    public override string CommandName => ConsoleCommandsNames.Update;

    public override void Help()
        => Console.WriteLine(HelpText);

    protected override UpdateCalendarTaskConsoleCommandContext CreateContextFromConsole()
    {
        var context = new UpdateCalendarTaskConsoleCommandContext();
        Console.WriteLine("Команда изменения задачи");
        if (!ConsoleArgumentsExtensions.TryRequestRequriedArgument("Введите номер задачи:", out var taskNumber))
        {
            Console.WriteLine("Не указан номер задачи.");
        }

        context.Number = taskNumber;
        // для удобства пользователя, нарушим шаблон. остальное дозапросим в ExecuteInternal.
        // зато там можно запросить задачу и проверить ее существование, а тут метод не асинхронный и нет cancellationtoken
        return context;   
    }

    protected override async Task ExcetuteInternal(ICalendarTaskHandler hander, UpdateCalendarTaskConsoleCommandContext commandContext, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(commandContext.Number))
        {
            throw new ArgumentNullException(nameof(commandContext.Number));
        }

        var updatedTask = await hander.GetCalendarTask(commandContext.Number, cancellationToken);
        if (updatedTask == null)
        {
            Console.WriteLine($"Задачи с номером {commandContext.Number} не существует. Команда прервана.");
            return;
        }

        await _calendarTaskConsoleWriter.WriteCalendarTask(updatedTask);

        var header = GetUpdatedValue("Укажите новый заголовок (Enter оставить старый):", updatedTask.Header);
        var description = GetUpdatedValue("Укажите новое описание (Enter оставить старое):", updatedTask.Description);
        var newDate = updatedTask.DueDate;

        if (ConsoleArgumentsExtensions.TryRequestDateTimeArgument("Укажите новую дату выполнения (Enter оставить старую)", out var updatedDate) && updatedDate.HasValue)
        {
            newDate = updatedDate.Value;
        }

        Console.WriteLine($"Будут произведены следующие изменения в задаче {commandContext.Number}:");       
        Console.WriteLine($"Заголовок:{header}");
        Console.WriteLine($"Дата завершения: {newDate:dd.MM.yyyy}");
        Console.WriteLine($"Описание:{description}");
        Console.WriteLine("Произвести изменения Y/N?");

        var key = Console.ReadLine();

        if (key.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
        {
            await hander.UpdateCalendarTask(commandContext.Number, header, description, newDate, cancellationToken);
            Console.WriteLine("Изменения успешно применены.");
        }
        else
        {
            Console.WriteLine($"Команда {CommandName} отменена");
        }        
    }

    private static string GetUpdatedValue(string caption, string oldValue)
    {
        var newValue = ConsoleArgumentsExtensions.RequestArgument(caption);
        return string.IsNullOrWhiteSpace(newValue) ? oldValue : newValue;
    }

    private const string HelpText = $@"Команда изменения задачи
        Необходимо ввести номер задачи, после чего команда последовательно спросит
        новые значения для каждого из свойст команды. Если свойство изменять не нужно
        необходимо нажать Enter.";
}

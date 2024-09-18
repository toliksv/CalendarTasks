using System;
using System.Globalization;
using CalendarTasks.Host.ConsoleCommands.Commands;
using Microsoft.VisualBasic;

namespace CalendarTasks.Host.ConsoleCommands.Arguments;

/// <summary>
/// Расширения для парсинга аргументов консоли.
/// </summary>
internal static class ConsoleArgumentsExtensions
{ 

    /// <summary>
    /// Количество неверных попыток, при вводе значения.
    /// </summary>
    private const int RetryCounts = 10;

    /// <summary>
    /// Запрашивает аргумент консоли, до тех пор, пока его не введут или отменят.
    /// </summary>
    /// <param name="caption"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool TryRequestRequriedArgument(string caption, out string requriedArgument)
    {
        if (string.IsNullOrWhiteSpace(caption))
        {
            throw new ArgumentException($"'{nameof(caption)}' cannot be null or whitespace.", nameof(caption));            
        }

        Console.WriteLine(caption);
        Console.WriteLine("Или введите {0} для отмены", ConsoleCommandsNames.Exit);

        requriedArgument = null;
        int counter = 0;
        while (string.IsNullOrWhiteSpace(requriedArgument) && counter < RetryCounts)
        {
           requriedArgument = Console.ReadLine();      
           counter++;
        }

        if (string.IsNullOrWhiteSpace(requriedArgument))
        {            
            return false;
        }

        if (ConsoleCommandsNames.Exit.Equals(requriedArgument, StringComparison.OrdinalIgnoreCase))
        {
            // получена команда выхода.
            requriedArgument = null;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Запрашиваем аргумент с консоли.
    /// </summary>
    /// <param name="caption">Текст запроса.</param>
    /// <returns>введенное значение аргумента.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string RequestArgument(string caption)
    {
        if (string.IsNullOrWhiteSpace(caption))
        {            
            throw new ArgumentException($"'{nameof(caption)}' cannot be null or whitespace.", nameof(caption));
        }

        Console.WriteLine(caption);
        return Console.ReadLine();
    }

    /// <summary>
    /// Получает дату из консоли, поддерживает команду отмены.
    /// </summary>
    /// <param name="caption"></param>
    /// <param name=""></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool TryRequestDateTimeArgument(string caption, out DateTime? dateTimeArgument)
    {
        if (string.IsNullOrWhiteSpace(caption))
        {
            throw new ArgumentException($"'{nameof(caption)}' cannot be null or whitespace.", nameof(caption));
        }

        Console.WriteLine(caption);
        Console.WriteLine("Формат ввода данных: гггг-ММ-дд");

        dateTimeArgument = null;
        int counter = 0;

        while (!dateTimeArgument.HasValue && counter < RetryCounts)
        {
            var stringValue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                // дата может быть не обязатеной.
                return true;
            }

            // поддержка команды выхода    
            if (ConsoleCommandsNames.Exit.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
            {            
                return false;
            }

            if (DateTime.TryParseExact(stringValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                dateTimeArgument = parsedDate;
                return true;
            }

            Console.WriteLine("Введенное значение \"{0}\" не является не является датой или дата введена в неверном формате!", stringValue);
            Console.WriteLine("Введите значение в формате: гггг-ММ-ДД");
            counter++;
        }

        return false;
    }

    public static bool RequestIntegerArgument(string caption, out int? argumentValue)
    {
        if (string.IsNullOrWhiteSpace(caption))
        {
            throw new ArgumentException($"'{nameof(caption)}' cannot be null or whitespace.", nameof(caption));
        }

        Console.WriteLine(caption);        

        argumentValue = null;
        int counter = 0;


        while (!argumentValue.HasValue && counter < RetryCounts)
        {
            var stringValue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                // дата может быть не обязатеной.
                return true;
            }

            // поддержка команды выхода    
            if (ConsoleCommandsNames.Exit.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
            {            
                return false;
            }

            if (int.TryParse(stringValue, CultureInfo.InvariantCulture, out var parsedValue))
            {
                argumentValue = parsedValue;
                return true;
            }

            Console.WriteLine("Введенное значение \"{0}\" не является не является типом int!");
            counter++;            
        }

        return false;
    }    
}

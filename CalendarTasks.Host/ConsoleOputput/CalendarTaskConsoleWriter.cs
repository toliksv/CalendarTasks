using System;
using CalendarTasks.Defs.Data;
using CalendarTasks.Defs.Extensions;

namespace CalendarTasks.Host.ConsoleOputput;

internal class CalendarTaskConsoleWriter : ICalendarTaskConsoleWriter
{
    public ValueTask WriteCalendarTask(CalendarTask calendarTask)
    {
        if (calendarTask is null)
        {
            throw new ArgumentNullException(nameof(calendarTask));
        }

        Console.WriteLine($"Задача номер:{calendarTask.Number}");
        Console.WriteLine($"Заголовок:{calendarTask.Header}");
        Console.WriteLine($"Статус:{calendarTask.Status.GetPublicName()}");
        Console.WriteLine($"Дата завершения:{calendarTask.DueDate:dd.MM.yyyy}");
        Console.WriteLine($"Описание: {calendarTask.Description}");
        return ValueTask.CompletedTask;
    }

    public ValueTask WriteCalendarTasksList(Dictionary<string, CalendarTask> calendarTaskList)
    {
        if (calendarTaskList is null)
        {
            throw new ArgumentNullException(nameof(calendarTaskList));
        }

        Console.WriteLine("Список задач:");
        foreach (var task in calendarTaskList.Values)
        {
            Console.WriteLine($"Номер:{task.Number} Заголовок:{task.Header} Статус:{task.Status.GetPublicName()} Дата завершения:{task.DueDate:dd.MM.yyyy}");
            
            if (!string.IsNullOrWhiteSpace(task.Description))
            {
                Console.WriteLine($"Описание: {task.Description}");                
            }
        }

        return ValueTask.CompletedTask;
    }
}

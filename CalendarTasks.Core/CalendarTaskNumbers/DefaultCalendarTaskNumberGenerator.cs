using CalendarTasks.Defs.CalendarTaskNumbers;

namespace CalendarTasks.Core.CalendarTaskNumbers;

internal class DefaultCalendarTaskNumberGenerator : ICalendarTaskNumberGenerator
{ 
    private readonly Random _random;

    public DefaultCalendarTaskNumberGenerator()
    {
        _random = new Random();
    }

    public string GenerateCalendarTaskNumber()
    {
          var randomNumber = _random.Next(0,10000000);
          return $"{randomNumber:0000000}";
    }
}

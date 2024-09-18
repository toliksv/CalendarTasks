using CalendarTasks.FileRepository.FileOutput;
using CalendarTasks.Defs.Data;
using Microsoft.Extensions.Options;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;
using Microsoft.Extensions.Logging;


namespace CalendarTasks.FileRepository.Tests;


public class FileRepositoryTests : IDisposable
{    
    private readonly IOptions<CalendarTaskFileRepositoryOptions> _options;
    private readonly Fixture _fixture;
    private readonly string _fileName;

    bool _disposed;


    public FileRepositoryTests()
    {
        _fileName = $"{Guid.NewGuid().ToString()}.jsn";
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        var opt = new CalendarTaskFileRepositoryOptions
        {
            DirectoryPath = "",
            FileName = _fileName,
        }; 
        _options = _fixture.Freeze<IOptions<CalendarTaskFileRepositoryOptions>>();
        _options.Value.ReturnsForAnyArgs(opt);    
    }

    [Fact]
    public async Task NoFile_CreateFile_WithEmptyArray()
    {
        var fileName = "NewTestFile.jsn";                

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var optValue = new CalendarTaskFileRepositoryOptions { FileName = fileName, DirectoryPath = "" };
        var options = _fixture.Freeze<IOptions<CalendarTaskFileRepositoryOptions>>();
        options.Value.ReturnsForAnyArgs(optValue);
        var reader = new FileOutputHandler(options, _fixture.Freeze<ILogger<FileOutputHandler>>());

        var dictionary = await reader.GetCalendarTasks(CancellationToken.None);

        Assert.True(File.Exists(fileName));         
    }
    
    [Fact]
    public async Task WriteArray_ArraySaved()
    {
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }

        var reader = new FileOutputHandler(_options, _fixture.Freeze<ILogger<FileOutputHandler>>());
        var dictionary = _fixture.CreateMany<CalendarTask>().ToDictionary(x =>x.Number);

        await reader.SaveCalendarTasks(dictionary, CancellationToken.None);
 
        var savedDictionary = await reader.GetCalendarTasks(CancellationToken.None);        

        Assert.NotNull(savedDictionary);
        Assert.Equal(savedDictionary.Count, dictionary.Count);    
        foreach (var key in savedDictionary)
        {
            Assert.True(dictionary.ContainsKey(key.Number));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed) // Если очистка не была выполнена явно
        {
            if (disposing) // В деструкторе при сборке мусора disposing = false
            {
               
            }

        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }         
            _disposed = true; // очистка была выполнена
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FileRepositoryTests()
    {
        Dispose(false);
    }   

}
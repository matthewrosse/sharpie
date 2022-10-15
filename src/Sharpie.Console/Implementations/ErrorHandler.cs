using Sharpie.Abstractions;

namespace Sharpie.Implementations;

public class ErrorHandler : IErrorHandler
{
    public bool HadError { get; set; }

    public void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        HadError = true;
    }
}
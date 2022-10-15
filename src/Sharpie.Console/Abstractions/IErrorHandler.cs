namespace Sharpie.Abstractions;

public interface IErrorHandler
{
    bool HadError { get; set; }
    void Error(int line, string message);
    void Report(int line, string where, string message);
}
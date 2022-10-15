using Sharpie.Abstractions;

namespace Sharpie.Implementations;

public sealed class SharpieRunner
{
    private readonly ITokenScanner _tokenScanner;
    private readonly IErrorHandler _errorHandler;
    public SharpieRunner(ITokenScanner tokenScanner, IErrorHandler errorHandler)
    {
        _tokenScanner = tokenScanner;
        _errorHandler = errorHandler;
    }
    public async Task RunFile(string path)
    {
        string source = await File.ReadAllTextAsync(path);
        Run(source);

        if (_errorHandler.HadError)
        {
            Environment.Exit(65);
        }
    }

    public void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line is null)
            {
                break;
            }

            Run(line);
            _errorHandler.HadError = false;
        }
    }

    public void Run(string source)
    {
        _tokenScanner.InitializeSource(source);
        var tokens = _tokenScanner.ScanTokens();

        foreach (var token in tokens)
        {
            // later some output provider (output to file, via http etc...)
            Console.WriteLine(token);
        }
    }
    
}
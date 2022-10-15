using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sharpie.Abstractions;
using Sharpie.Implementations;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<SharpieRunner>();
        services.AddTransient<ITokenScanner, TokenScanner>();
        services.AddSingleton<IErrorHandler, ErrorHandler>();
    })
    .Build();

var runner = host.Services.GetRequiredService<SharpieRunner>();

if (args.Length > 1)
{
    Console.WriteLine("Usage: sharpie [script]");
    Environment.Exit(64);
}
else if (args.Length == 1)
{
    await runner.RunFile(args[0]);
}
else
{
    runner.RunPrompt();
}

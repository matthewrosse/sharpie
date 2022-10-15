using Sharpie.Models;

namespace Sharpie.Abstractions;

public interface ITokenScanner
{
    IList<Token> ScanTokens();
    void InitializeSource(string source);
}
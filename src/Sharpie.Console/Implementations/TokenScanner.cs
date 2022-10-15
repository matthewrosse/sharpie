using Sharpie.Abstractions;
using Sharpie.Models;

namespace Sharpie.Implementations;

public class TokenScanner : ITokenScanner
{
    private readonly IErrorHandler _errorHandler;
    public string Source { get; set; }
    private readonly IList<Token> _tokens = new List<Token>();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public TokenScanner(IErrorHandler errorHandler)
    {
        _errorHandler = errorHandler;
    }

    public void InitializeSource(string source)
    {
        Source = source;
    }

    public IList<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            // we are at the beginning of the next lexeme
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.Eof, "", null, _line));

        return _tokens;
    }

    private bool IsAtEnd()
    {
        return _current >= Source.Length;
    }

    public void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            case '(':
                AddToken(TokenType.LeftParen);
                break;
            case ')':
                AddToken(TokenType.RightParen);
                break;
            case '{':
                AddToken(TokenType.LeftBrace);
                break;
            case '}':
                AddToken(TokenType.RightBrace);
                break;
            case ',':
                AddToken(TokenType.Comma);
                break;
            case '.':
                AddToken(TokenType.Dot);
                break;
            case '-':
                AddToken(TokenType.Minus);
                break;
            case '+':
                AddToken(TokenType.Plus);
                break;
            case ';':
                AddToken(TokenType.Semicolon);
                break;
            case '*':
                AddToken(TokenType.Star);
                break;

            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;

            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;

            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;

            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;

            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                }
                else
                {
                    AddToken(TokenType.Slash);
                }

                break;

            case ' ':
            case '\r':
            case '\t':
                break;

            case '\n':
                _line++;
                break;

            case '"':
                String();
                break;

            default:
                _errorHandler.Error(_line, "Unexpected character.");
                break;
        }
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
            {
                _line++;
            }

            Advance();
        }

        if (IsAtEnd())
        {
            _errorHandler.Error(_line, "Unterminated string.");
            return;
        }

        Advance();

        var value = Source.Substring(_start + 1, _current - 1);
        AddToken(TokenType.String, value);
    }

    private char Peek()
    {
        if (IsAtEnd())
        {
            return '\0';
        }

        return Source[_current];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
        {
            return false;
        }

        if (Source[_current] != expected)
        {
            return false;
        }

        _current++;

        return true;

    }

    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object literal)
    {
        var text = Source.Substring(_start, _current);
        _tokens.Add(new Token(tokenType, text, literal, _line));
    }

    private char Advance()
    {
        return Source[_current++];
    }
}
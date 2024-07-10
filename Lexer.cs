using System.Text;

namespace MTLang;
internal static class Lexer {
    internal enum TokenType {
        IDENTIFIER,
        NUMBER,
        EQUALS,
        OPEN_PAREN,
        CLOSE_PAREN,
        DECLARATION,
        BINARY_OPERATOR,
    }
    private static readonly Dictionary<String, TokenType> keywords = new(){
        { "set", TokenType.DECLARATION },
    };
    internal record class Token(String Chunk, TokenType Type) {
        public override String ToString() {
            return $"{this.Type}: `{this.Chunk}`";
        }
        internal static IEnumerable<Token> Tokenize(String chunk) {
            Stack<Token> tokens = new Stack<Token>();
            Queue<Char> sourceChars = new(chunk.Length);
            foreach (Char c in chunk)
                sourceChars.Enqueue(c);
            while (sourceChars.Count > 0) {
                switch (sourceChars.ElementAt(0)) {
                    case ' ':
                    case ';':
                    case '\t':
                    case '\n':
                        _ = sourceChars.Dequeue();
                        continue;
                    case '(':
                        tokens.Push(new Token(sourceChars.Dequeue().ToString(), TokenType.OPEN_PAREN));
                        continue;
                    case ')':
                        tokens.Push(new Token(sourceChars.Dequeue().ToString(), TokenType.CLOSE_PAREN));
                        continue;
                    // Binary operators
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        tokens.Push(new Token(sourceChars.Dequeue().ToString(), TokenType.BINARY_OPERATOR));
                        continue;
                    case '=':
                        tokens.Push(new Token(sourceChars.Dequeue().ToString(), TokenType.EQUALS));
                        continue;
                    default:
                        break;
                }
                // Check for Multi-Char Keywords / Indentifier / Token
                if (Char.IsDigit(sourceChars.ElementAt(0))) {
                    StringBuilder num = new();
                    while (sourceChars.Count > 0 && Char.IsDigit(sourceChars.ElementAt(0))) {
                        _ = num.Append(sourceChars.Dequeue());
                    }
                    tokens.Push(new Token(num.ToString(), TokenType.NUMBER));
                    continue;
                }

                static Boolean IsClearText(Char c) {
                    return Char.IsAsciiLetterOrDigit(c) &&
                        !Char.IsWhiteSpace(c) &&
                        !Char.IsSymbol(c) &&
                        !Char.IsPunctuation(c) &&
                        !Char.IsControl(c);
                }
                if (IsClearText(sourceChars.ElementAt(0))) {
                    StringBuilder ident = new();
                    while (sourceChars.Count > 0 && IsClearText(sourceChars.ElementAt(0))) {
                        _ = ident.Append(sourceChars.Dequeue());
                    }
                    String identifier = ident.ToString();
                    if (keywords.TryGetValue(identifier, out TokenType t)) {
                        tokens.Push(new Token(identifier, t));
                    } else {
                        tokens.Push(new Token(identifier, TokenType.IDENTIFIER));
                    }
                    continue;
                }
                if (Char.IsWhiteSpace(sourceChars.ElementAt(0))) {
                    _ = sourceChars.Dequeue().ToString();
                    continue;
                }
                throw new InvalidDataException(sourceChars.ElementAt(0).ToString());
            }
            return tokens;
        }
    };
}

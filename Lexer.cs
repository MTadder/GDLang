using System.Text;

namespace MTLang;
internal static class Lexer {
    internal enum TokenType {
        IDENTIFIER, // NAME
        NUMBER, // .[0-9]?.?[0-9]+ || C#: (\d*.?\d+)

        EQUALS, // =

        ACCESSOR, // .
        LIST_SEPERATOR, // ,
        INVOKE, // !

        SINGLE_QUOTE, // '
        QUOTE, // "

        OPEN_PAREN, // (
        CLOSE_PAREN, // )
        OPEN_BRACK, // [
        CLOSE_BRACK, // ]
        OPEN_CURLY, // {
        CLOSE_CURLY, // }

        HOIST, // ^

        END_OF_STATEMENT, // ;

        DECLARATION, // set
        BINARY_OPERATOR, // [+-*/]
    }
    private static readonly Dictionary<String, TokenType> keywords = new(){
        { "set", TokenType.DECLARATION },
    };
    internal record class Token(String Chunk, TokenType Type) {
        internal Token(Char chunk, TokenType type) : this(chunk.ToString(), type) { }
        internal Token(StringBuilder chunk, TokenType type) : this(chunk.ToString(), type) { }
        internal Token(Queue<Char> queue, TokenType type) : this(queue.Dequeue().ToString(), type) { }
        public override String ToString() {
            return $"{this.Type}: `{this.Chunk}`";
        }
        internal static IEnumerable<Token> Tokenize(String chunk) {
            Stack<Token> tokens = new Stack<Token>();
            Queue<Char> sourceChars = new(chunk.Length);
            foreach (Char c in chunk)
                sourceChars.Enqueue(c);
            while (sourceChars.Count > 0) {
                // Check for Multi-Char Keywords / Indentifier / Quote / Number
                if (Char.IsDigit(sourceChars.ElementAt(0)) || ( // probably some side
                    sourceChars.ElementAt(0).Equals('.') && // effects to account for in this block. TODO.
                    Char.IsDigit(sourceChars.ElementAtOrDefault(1))
                    )) { // Number literal
                    StringBuilder num = new();
                    while (sourceChars.Count > 0 &&
                        (Char.IsDigit(sourceChars.ElementAt(0)) ||
                        // allow '.' to denote a decimal, and carry on.
                        // DO NOT allow multiple decimals.
                        sourceChars.ElementAt(0).Equals('.')
                        )
                        ) {
                        if (sourceChars.ElementAt(0).Equals('.')) {
                            if (num.ToString().Contains('.'))
                                throw new InvalidDataException(sourceChars.ElementAt(0).ToString());
                        }
                        _ = num.Append(sourceChars.Dequeue());
                    }
                    if (num.ToString().StartsWith('.'))
                        _ = num.Insert(0, 0);
                    tokens.Push(new(num, TokenType.NUMBER));
                    continue;
                }
                // Handle whitespace, parentheses, arithmetic & logic.
                switch (sourceChars.ElementAt(0)) {
                    case '\n': // Statement terminators
                    case ';':
                        tokens.Push(new Token(sourceChars, TokenType.END_OF_STATEMENT));
                        continue;
                    case ' ': // Skip whitespace and tabs.
                    case '\t':
                        _ = sourceChars.Dequeue();
                        continue;
                    case '.': // Member accessor
                        tokens.Push(new(sourceChars, TokenType.ACCESSOR));
                        continue;
                    case '^': // Hoist
                        tokens.Push(new(sourceChars, TokenType.HOIST));
                        continue;
                    case ',': // Comma
                        tokens.Push(new(sourceChars, TokenType.LIST_SEPERATOR));
                        continue;
                    case '!': // Function invocation
                        tokens.Push(new(sourceChars, TokenType.INVOKE));
                        continue;
                    case '(': // Function accessor (with params)
                        tokens.Push(new(sourceChars, TokenType.OPEN_PAREN));
                        continue;
                    case ')':
                        tokens.Push(new(sourceChars, TokenType.CLOSE_PAREN));
                        continue;
                    case '[': // Array accessor
                        tokens.Push(new(sourceChars, TokenType.OPEN_BRACK));
                        continue;
                    case ']':
                        tokens.Push(new(sourceChars, TokenType.CLOSE_BRACK));
                        continue;
                    case '{': // Array literal
                        tokens.Push(new(sourceChars, TokenType.OPEN_CURLY));
                        continue;
                    case '}':
                        tokens.Push(new(sourceChars, TokenType.CLOSE_CURLY));
                        continue;
                    // Binary operators
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        tokens.Push(new(sourceChars, TokenType.BINARY_OPERATOR));
                        continue;
                    case '=':
                        tokens.Push(new(sourceChars, TokenType.EQUALS));
                        continue;
                    default:
                        break;
                }
                static Boolean IsClearText(Char c) {
                    return Char.IsAsciiLetterOrDigit(c) &&
                        !Char.IsWhiteSpace(c) &&
                        !Char.IsSymbol(c) &&
                        !Char.IsPunctuation(c) &&
                        !Char.IsControl(c);
                }
                if (IsClearText(sourceChars.ElementAt(0))) { // Identifier
                    StringBuilder ident = new();
                    while (sourceChars.Count > 0 && IsClearText(sourceChars.ElementAt(0))) {
                        _ = ident.Append(sourceChars.Dequeue());
                    }
                    // Check if identifier is keyword
                    String identifier = ident.ToString();
                    if (keywords.TryGetValue(identifier, out TokenType t)) {
                        tokens.Push(new(identifier, t)); // Push keyword
                    } else {
                        tokens.Push(new(identifier, TokenType.IDENTIFIER)); // Push Identifier
                    }
                    continue;
                }
                // Check for Quote
                if (sourceChars.ElementAt(0).Equals('\'')) {
                    _ = sourceChars.Dequeue(); // Dequeue open quote mark
                    StringBuilder quote = new();
                    while (sourceChars.Count > 0 && !sourceChars.ElementAt(0).Equals('\'')) {
                        _ = quote.Append(sourceChars.Dequeue());
                    }
                    _ = sourceChars.Dequeue(); // Dequeue closing quote mark
                    tokens.Push(new(quote, TokenType.SINGLE_QUOTE));
                    continue;
                } else if (sourceChars.ElementAt(0).Equals('\"')) {
                    _ = sourceChars.Dequeue(); // Dequeue open quote mark
                    StringBuilder quote = new();
                    while (sourceChars.Count > 0 && !sourceChars.ElementAt(0).Equals('\"')) {
                        _ = quote.Append(sourceChars.Dequeue());
                    }
                    _ = sourceChars.Dequeue(); // Dequeue closing quote mark
                    tokens.Push(new(quote, TokenType.QUOTE));
                    continue;
                }
                // Check for whitespace
                if (Char.IsWhiteSpace(sourceChars.ElementAt(0))) {
                    _ = sourceChars.Dequeue();
                    continue;
                }
                throw new InvalidDataException(sourceChars.ElementAt(0).ToString());
            }
            return tokens;
        }
    };
}

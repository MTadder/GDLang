namespace MTLang;

internal class Program {
    internal static MTLib.Terminal.Style errStyle = new(ConsoleColor.Red);
    internal static MTLib.Terminal.Style mainStyle = new(ConsoleColor.White);
    internal static MTLib.Terminal.Style inputStyle = new(ConsoleColor.Green);
    internal static MTLib.Terminal.TypewriterConsoleWriter nowaitWriter = new(2);
    internal static MTLib.Terminal.TypewriterConsoleWriter writer = new(15);
    static void Main(String[] args) {
        mainStyle.WriteLine("MTLang v0.0.0 (REPL v0.1.0)", writer);

        while (true) {
            mainStyle.Write("~> ", writer);
            var old = inputStyle.Push();
            String? input = Console.ReadLine();
            _ = old.Push();
            if (input is null || input.Equals("exit"))
                break;
            try {
                mainStyle.WriteLine(String.Join("\n", Lexer.Token.Tokenize(input)), writer);
            } catch (InvalidDataException e) {
                errStyle.WriteLine($"DataException: `{e.Message}`", nowaitWriter);
            }
            mainStyle.Write("\n", writer);
        }
    }
}

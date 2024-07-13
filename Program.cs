namespace MTLang;

internal class Program {
    internal static String mtlibVersion = "0.1.0";
    internal static String replVersion = "0.1.0";

    internal static MTLib.Terminal.Style errStyle = new(ConsoleColor.Red);
    internal static MTLib.Terminal.Style mainStyle = new(ConsoleColor.White);
    internal static MTLib.Terminal.Style inputStyle = new(ConsoleColor.Green);
    internal static MTLib.Terminal.TypewriterConsoleWriter nowaitWriter = new(2);
    internal static MTLib.Terminal.TypewriterConsoleWriter writer = new(15);
    static void Main(String[] args) {
        mainStyle.WriteLine($"MTLang v{mtlibVersion} (REPL v{replVersion})", writer);

        while (true) {
            mainStyle.Write("~> ", writer);
            var old = inputStyle.Push();
            String? input = Console.ReadLine();
            _ = old.Push();
            if (input is null || input.Equals("exit"))
                break;
            try {
                mainStyle.WriteLine(String.Join("\t\n", Lexer.Token.Tokenize(input)), writer);
            } catch (InvalidDataException e) {
                errStyle.WriteLine($"Unexpected character: `{e.Message}`", nowaitWriter);
            }
            mainStyle.Write("\n", writer);
        }
    }
}

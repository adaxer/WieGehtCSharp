using LocalizeHelper;
using System.Globalization;

namespace LocalizeHelper;

public class CommandInfo
{
    private static CommandInfo _none = new(Command.None, string.Empty, string.Empty, []);
    private static CommandInfo _invalid = new(Command.Invalid, string.Empty, string.Empty, []);
    private string _filePath=string.Empty;

    private CommandInfo(Command command, string inputText, string outputPath, string[] languageCodes)
    {
        Command = command;
        LanguageCodes = languageCodes;
        InputText = inputText;
        OutputPath = outputPath;
    }

    public Command Command { get; }
    public string InputText { get; }
    public string OutputPath { get; }
    public string FilePath => _filePath;
    public string[] LanguageCodes { get; }

    public static CommandInfo None => _none;
    public static CommandInfo Invalid => _invalid;

    public static CommandInfo Create(string[] args)
    {
        if (args.Length == 0)
        {
            return None;
        }
        try
        {
            Command command = Enum.Parse<Command>(args[0]);
            string content = File.ReadAllText(args[1]);
            var codes = args
                .Skip(2)
                .Where(code => CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Any(c => c.TwoLetterISOLanguageName.Equals(code, StringComparison.OrdinalIgnoreCase)))
                .Select(code => code.ToUpperInvariant()) // oder ToUpperInvariant(), wenn du uppercase willst
                .Distinct();
            string outputPath = (command == Command.ResxToCsv) ? args[1].Replace("resx", "csv") : args[1].Replace("csv", "resx");
            return new CommandInfo(command, content, outputPath, codes.ToArray()) { _filePath = args[1] };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return CommandInfo.Invalid;
        }
    }
}

public enum Command
{
    None,
    ResxToCsv,
    CsvToResx,
    Invalid
}

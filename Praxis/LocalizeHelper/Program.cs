using System.Windows.Input;
using System.Xml.Linq;

namespace LocalizeHelper;

class Program
{
    static void Main(string[] args)
    {
        var commandInfo = CommandInfo.Create(args);
        Func<CommandInfo, Result> action = GetActionForCommand(commandInfo.Command);
        Console.WriteLine(action(commandInfo).Message);

        bool canClose = action == MakeCsv || action == MakeResx || action == ShowUsage;
        Command newCommand = Command.None;
        string filePath = string.Empty;
        string[] languageCodes = [];

        while (!canClose)
        {
            var showMenu = true;
            var key = Console.ReadKey(true).Key.ToString();
            switch (key)
            {
                case "C":
                    newCommand = newCommand == Command.CsvToResx ? Command.ResxToCsv : Command.CsvToResx;
                    break;
                case "F":
                    Console.CursorVisible = true;
                    Console.SetCursorPosition(startX + 13, startY + 2);
                    filePath = Console.ReadLine() ?? string.Empty;
                    break;
                case "S":
                    Console.CursorVisible = true;
                    Console.SetCursorPosition(startX + 12, startY + 3);
                    languageCodes = (Console.ReadLine() ?? string.Empty).ToUpper().Split(' ', ',');
                    break;
                case "A":
                    string[] newArgs = [newCommand.ToString(), filePath];
                    commandInfo = CommandInfo.Create(newArgs.Concat(languageCodes).ToArray());
                    var result = GetActionForCommand(newCommand).Invoke(commandInfo);
                    if (!result.Succeeded)
                    {
                        ShowMenu(Command.Invalid, string.Empty, []);
                    }
                    PrintAt(0, 7 + menuItems.Length, result.Message);
                    showMenu = false;
                    canClose = result.Succeeded;
                    break;
                case "Q":
                    canClose = true;
                    break;
            }
            if (showMenu)
            {
                ShowMenu(newCommand, filePath, languageCodes);
            }
        }
        Console.CursorVisible = true;
    }

    private static Func<CommandInfo, Result> GetActionForCommand(Command command)
    {
        return command switch
        {
            Command.ResxToCsv => MakeCsv,
            Command.CsvToResx => MakeResx,
            Command.None => c =>
            {
                ShowMenu(c.Command, c.FilePath, c.LanguageCodes);
                return Result.Success();
            }
            ,
            _ => ShowUsage,
        };
    }

    private static Result MakeCsv(CommandInfo info)
    {
        try
        {
            XElement xml = XElement.Load(new StringReader(info.InputText));
            var data = from element in xml.Elements("data")
                       where element.Attribute("name") is { } && element.Element("value") is { }
                       select new KeyValuePair<string, string>(element.Attribute("name")!.Value.ToString(), element.Element("value")!.Value.ToString());

            string langTitles = string.Join(",", info.LanguageCodes.Select(c => $"\"{c}\""));
            string langEmpties = string.Join(",", info.LanguageCodes.Select(c => $"\"\""));
            Func<string, string, string, string> createLine = (key, value, rest) => $"\"{key}\",\"{value}\",{rest}";

            List<string> csv = [createLine("Key", "DE", langTitles)];
            csv = csv.Concat(data.Select(kvp => createLine(kvp.Key, kvp.Value, langEmpties))).ToList();

            File.WriteAllLines(info.OutputPath, csv);
            return Result.Success($"Csv {info.OutputPath} erzeugt");
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private static Result MakeResx(CommandInfo info)
    {
        try
        {
            var csv = info.InputText
                .Trim('"')
                .Replace("\",\"", "||")
                .Replace("\"\n\"", "|z|")
                .Split("|z|", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split("||", StringSplitOptions.None));

            var languages = csv
                .First()
                .Skip(2)
                .ToList();

            for (int i = 0; i < languages.Count; i++)
            {
                var fileName = info.OutputPath.Replace("translation", languages[i]);
                var data = csv.Skip(1).ToDictionary(a => a[0], a => a[i + 2]);
                CreateResx(fileName, data);
            }
            return Result.Success($"ResX zu {info.FilePath} erzeugt");
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private static void CreateResx(string fileName, Dictionary<string, string> data)
    {
        var content = data.Select(kvp =>
            new XElement("data",
                new XAttribute("name", kvp.Key),
                new XAttribute(XNamespace.Xml + "space", "preserve"),
                new XElement("value", kvp.Value)));
        var xml = new XElement("root", _resxHeaders, content);
        xml.Save(fileName);
    }

    private static readonly IReadOnlyList<XElement> _resxHeaders = new List<XElement>
    {
        new("resheader",
            new XAttribute("name", "resmimetype"),
            new XElement("value", "text/microsoft-resx")),
        new("resheader",
            new XAttribute("name", "version"),
            new XElement("value", "2.0")),
        new("resheader",
            new XAttribute("name", "reader"),
            new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")),
        new("resheader",
            new XAttribute("name", "writer"),
            new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"))
    }.AsReadOnly();
    private static Result ShowUsage(CommandInfo info)
    {
        var usage = "\nVerwendung:\nLocalizeHelper [ResxToCsv|Csv2Resx] <filePath> <languages>\n";
        usage += "\nBeispiel:\nLocalizeHelper ResxToCsv my.de.resx en fr\n";
        usage += "Erzeugt aus my.de.resx die Datei my.de.csv\n\n";
        usage += "LocalizeHelper CsvToResx my.translation.csv en fr\n";
        usage += "Erzeugt aus my.translation.csv die Dateien my.EN.resx und my.FR.resx";
        usage += "\n\nLocalizeHelper\nzeigt interaktives Menü.";
        Console.WriteLine(usage);
        return Result.Success();
    }

    private static void ShowMenu(Command command, string filePath, string[] languages)
    {
        Console.Clear();

        PrintAt(0, 0, "LocalizeHelper");
        PrintAt(2, 1, $"Command: {command}");
        PrintAt(2, 2, $"File-Pfad: {filePath}");
        PrintAt(2, 3, $"Sprachen: {string.Join(',', languages)}");
        for (int i = 0; i < menuItems.Length; i++)
        {
            Console.SetCursorPosition(startX, startY + 5 + i);
            Console.Write(menuItems[i]);
        }

        Console.CursorVisible = false; // Cursor aus dem Weg
    }

    private static void PrintAt(int relX, int relY, string message)
    {
        Console.SetCursorPosition(startX + relX, startY + relY);
        Console.Write(message);
        Console.SetCursorPosition(0, Console.WindowHeight - 1); // Cursor aus dem Weg
    }

    static string[] menuItems = {
        "C - Command umschalten",
        "F - File-Pfad eingeben (z.B. 'my.de.resx')",
        "S - Sprachen eingeben (z.B. en fr)",
        "A - Ausführen",
        "",
        "Q - Beenden"
    };

    static int startX = Console.WindowWidth / 2 - 6; // leicht nach links versetzt für Ausgewogenheit
    static int startY = Console.WindowHeight / 2 - (menuItems.Length + 5) / 2;
}

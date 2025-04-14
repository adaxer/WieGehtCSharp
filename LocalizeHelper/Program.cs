using System.Xml.Linq;

namespace LocalizeHelper;

class Program
{
    static void Main(string[] args)
    {
        var commandInfo = CommandInfo.Create(args);
        Action<CommandInfo> action = commandInfo.Command switch
        {
            Command.ResxToCsv => MakeCsv,
            Command.CsvToResx => MakeResx,
            Command.None => ShowMenu,
            _ => ShowUsage,
        };
        action(commandInfo);
    }

    private static void MakeCsv(CommandInfo info)
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"MakeCsv nicht erfolgreich, Fehlermeldung: {ex.Message}");
        }
    }

    private static void MakeResx(CommandInfo info)
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"MakeCsv nicht erfolgreich, Fehlermeldung: {ex.Message}");
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
    private static void ShowUsage(CommandInfo info)
    {
        var usage = "\nVerwendung:\nLocalizeHelper [ResxToCsv|Csv2Resx] <filePath> <languages>\n";
        usage += "\nBeispiel:\nLocalizeHelper ResxToCsv my.de.resx en fr\n";
        usage += "Erzeugt aus my.de.resx die Datei my.de.csv\n\n";
        usage += "LocalizeHelper CsvToResx my.translation.csv en fr\n";
        usage += "Erzeugt aus my.translation.csv die Dateien my.EN.resx und my.FR.resx";
        usage += "\n\nLocalizeHelper\nzeigt interaktives Menü.";
        Console.WriteLine(usage);
    }

    private static void ShowMenu(CommandInfo info)
    {
        Console.WriteLine("Menu: ...");
    }
}

using System.Text.Json;

namespace ProjektStruktur;

class Program
{
    static void Main(string[] args)
    {
        string name;
        int age;

        // Eingabe
        if (args.Length == 1)
        {
            Console.WriteLine("Lese aus Datei " + args[0]);
            var json = File.ReadAllText(args[0]);
            var person = JsonSerializer.Deserialize<Person>(json);
            name=person.Name;
            age=person.Age;
        }
        else if (args.Length == 2)
        {
            Console.WriteLine("Lese aus Args " + args[0]+ " " + args[1]);
            name = args[0];
            age = int.Parse(args[1]);
        }
        else
        {
            Console.WriteLine("Hallo - wie ist der werte Name");
            name = Console.ReadLine();
            Console.WriteLine("Und das Alter, wenn die Frage erlaubt ist");
            age = int.Parse(Console.ReadLine());
        }

        // Verarbeitung
        var greeting = "Hallo " + name + ", herzlichen Glückwunsch zum " + age.ToString() + ".";

        // Ausgabe
        Console.WriteLine(greeting);
    }
}

namespace ProjektStruktur;

class Program
{
    static void Main(string[] args)
    {
        string name;
        int age;

        // Eingabe
        if (args.Length == 2)
        {
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

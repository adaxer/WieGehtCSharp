namespace ProjektStruktur;

class Program
{
    static void Main(string[] args)
    {
        // Eingabe
        Console.WriteLine("Hallo - wie ist der werte Name");
        var name = Console.ReadLine();
        Console.WriteLine("Und das Alter, wenn die Frage erlaubt ist");
        var age = int.Parse(Console.ReadLine());

        // Verarbeitung
        var greeting = "Hallo " + name + ", herzlichen Glückwunsch zum " + age.ToString()+".";

        // Ausgabe
        Console.WriteLine(greeting);
    }
}

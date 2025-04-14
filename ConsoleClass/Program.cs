using System;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Die Klasse \"Konsole\"";

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Blue;
        // while (true)
        // {
        //     var key = Console.ReadKey();
        //     Console.WriteLine($"Key ist {key.Key} (Modifier(s): {key.Modifiers})");
        // }

        while (!Console.KeyAvailable)
        {
            Console.WriteLine("Keine Taste - mach halt!");
            Thread.Sleep(1000);
        }
        Console.ResetColor();
        var key = Console.ReadKey();
        Console.WriteLine($"Key ist {key.Key} (Modifier(s): {key.Modifiers})");
        ShowMenu();
    }
    static void ShowMenu()
    {
        Console.Clear();

        string[] menuItems = {
        "O - Öffnen",
        "S - Speichern",
        "L - Laden",
        "H - Hilfe",
        "",
        "X - Beenden"
    };

        int height = Console.WindowHeight;
        int startY = height / 2 - menuItems.Length / 2;
        int startX = Console.WindowWidth / 2 - 6; // leicht nach links versetzt für Ausgewogenheit

        for (int i = 0; i < menuItems.Length; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            Console.Write(menuItems[i]);
        }

        Console.SetCursorPosition(0, height - 1); // Cursor aus dem Weg
    }

}
namespace ShowMenu;

class Program
{
    static void Main(string[] args)
    {
        ShowMenu();
        bool isDone = false;
        while (!isDone)
        {
        }
    }

    private static void Print(string message)
    {
        Console.SetCursorPosition(startX, printY);
        Console.Write(message);
        Console.SetCursorPosition(0, Console.WindowHeight - 1); // Cursor aus dem Weg
    }

    static void ShowMenu()
    {
        Console.Clear();

        int height = Console.WindowHeight;
        int startY = height / 2 - menuItems.Length / 2;

        for (int i = 0; i < menuItems.Length; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            Console.Write(menuItems[i]);
        }

        Console.SetCursorPosition(0, height - 1); // Cursor aus dem Weg
    }

    static string[] menuItems = {
        "O - Öffnen",
        "S - Speichern",
        "L - Laden",
        "H - Hilfe",
        "",
        "X - Beenden"
    };

    static int startX = Console.WindowWidth / 2 - 6; // leicht nach links versetzt für Ausgewogenheit
    static int printY = 2 + Console.WindowHeight / 2 + menuItems.Length / 2;
}





//var keyInfo = Console.ReadKey(true);
//var key = keyInfo.Key.ToString();
//var selection = menuItems.FirstOrDefault(i => i.StartsWith(key)) ?? "Falsche Eingabe";
//Print(new String(' ', Math.Max(menuItems.Max(i => i.Length) + "Ihre Eingabe: ".Length, "Ihre Eingabe: Falsche Eingabe".Length)));
//Print($"Ihre Eingabe: {selection}");
//isDone = key == "X";

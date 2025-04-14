namespace HoldTheLine;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start");
        Console.CursorVisible = false;
        for(int zeile=0; zeile<100; zeile++)
        {
            await Task.Delay(200);
            string output=$"\rZeile: {zeile.ToString().PadLeft(4, '0')}";
            Console.Write(output);
        }
    }
}

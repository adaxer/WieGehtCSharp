namespace PrintArgs;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Ich habe folgende Argumente erhalten, alphabetisch sortiert:");
        foreach (var arg in args.Order())
        {
            Console.WriteLine(arg);
        }
    }
}

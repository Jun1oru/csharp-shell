class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            string userInput = Console.ReadLine();
            Console.WriteLine($"{userInput}: command not found");
        }
    }
}

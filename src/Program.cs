class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            string userInput = Console.ReadLine();
            if (userInput == "exit")
            {
                break;
            }
            Console.WriteLine($"{userInput}: command not found");
        }
    }
}

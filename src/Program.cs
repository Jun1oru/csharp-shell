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
            else if (userInput.StartsWith("echo "))
            {
                Console.WriteLine(userInput[5..]);
            }
            else
            {
                Console.WriteLine($"{userInput}: command not found");
            }
        }
    }
}

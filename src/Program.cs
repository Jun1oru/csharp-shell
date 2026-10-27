class Program
{
    static void Main()
    {
        List<string> builtIn = ["exit", "echo", "type"];
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
            else if (userInput.StartsWith("type "))
            {
                string arg = userInput.Split(' ')[1].Trim();
                if (builtIn.Contains(arg))
                {
                    Console.WriteLine($"{arg} is a shell builtin");
                }
                else
                {
                    Console.WriteLine($"{arg}: not found");
                }
            }
            else
            {
                Console.WriteLine($"{userInput}: command not found");
            }
        }
    }
}

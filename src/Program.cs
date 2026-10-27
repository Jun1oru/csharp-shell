class Program
{
    static void Main()
    {
        List<string> builtIn = ["exit", "echo", "type"];
        string PATH = Environment.GetEnvironmentVariable("PATH");
        string[] folders = PATH.Split(Path.PathSeparator) ?? [];

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
                    bool found = false;
                    foreach (string folderPath in folders)
                    {
                        string fullPath = Path.Combine(folderPath, arg);

                        if (File.Exists(fullPath) &&
                            File.GetUnixFileMode(fullPath).HasFlag(UnixFileMode.GroupExecute | UnixFileMode.UserExecute | UnixFileMode.OtherExecute))
                        {
                            found = true;
                            Console.WriteLine($"{arg} is {fullPath}");
                        }
                    }
                    if (found == false)
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

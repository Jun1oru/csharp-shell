class Program
{
    static void Main()
    {
        List<string> builtIn = ["exit", "echo", "type"];
        string PATH = Environment.GetEnvironmentVariable("PATH");
        IEnumerable<string> folders = PATH.Split(Path.PathSeparator);

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
                    foreach (string folderPath in folders)
                    {
                        DirectoryInfo directory = new DirectoryInfo(folderPath);
                        try
                        {
                            var files = directory.GetFiles("");
                            foreach (var file in files)
                            {
                                var filePath = Path.Combine(folderPath, file.Name);
                                if ((File.GetUnixFileMode(filePath) & (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0)
                                {
                                    Console.WriteLine($"{arg} is {filePath}");
                                }
                            }
                        }
                        catch (DirectoryNotFoundException ex)
                        {

                        }
                    }
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

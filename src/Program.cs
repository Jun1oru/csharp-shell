using System.Diagnostics;

class Program
{
    static void Main()
    {
        List<string> builtIn = ["exit", "echo", "type", "pwd"];
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
                    var result = FindExecutableInPath(arg, folders);

                    if (result == null)
                    {
                        Console.WriteLine($"{arg}: not found");
                    }
                    else
                    {
                        Console.WriteLine($"{arg} is {result}");
                    }
                }
            }
            else if (userInput == "pwd")
            {
                string folder = Directory.GetCurrentDirectory();
                Console.WriteLine($"{folder}");
            }
            else
            {
                // bool found = false;
                string[] inputSplit = userInput.Split(" ");
                string command = inputSplit[0].Trim();

                var result = FindExecutableInPath(command, folders);

                if (result == null)
                {
                    Console.WriteLine($"{userInput}: command not found");
                }
                else
                {
                    Process.Start(command, inputSplit[1..]).WaitForExit();
                }
            }
        }
    }

    public static string? FindExecutableInPath(string command, string[] folders)
    {
        foreach (string folderPath in folders)
        {
            string fullPath = Path.Combine(folderPath, command);

            if (File.Exists(fullPath) &&
                        File.GetUnixFileMode(fullPath).HasFlag(UnixFileMode.GroupExecute | UnixFileMode.UserExecute | UnixFileMode.OtherExecute))
            {
                return fullPath;
            }
        }
        return null;
    }
}

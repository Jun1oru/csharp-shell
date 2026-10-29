using System.Diagnostics;

class Program
{
    static void Main()
    {
        List<string> builtIn = ["exit", "echo", "type", "pwd", "cd"];
        string PATH = Environment.GetEnvironmentVariable("PATH");
        string[] folders = PATH.Split(Path.PathSeparator) ?? [];

        while (true)
        {
            Console.Write("$ ");

            string userInput = Console.ReadLine();

            string[] tokens = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = tokens[0];
            string[] args = tokens[1..];
            if (command == "exit")
            {
                break;
            }
            else if (command == "echo")
            {
                Console.WriteLine(string.Join(" ", args));
            }
            else if (command == "type")
            {
                string arg = args[0];
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
            else if (command == "pwd")
            {
                string folder = Directory.GetCurrentDirectory();
                Console.WriteLine($"{folder}");
            }
            else if (command == "cd")
            {
                string arg = args[0];
                if (Directory.Exists(arg))
                {
                    Directory.SetCurrentDirectory(arg);
                }
                else
                {
                    Console.WriteLine($"cd: {arg}: No such file or directory");
                }
            }
            else
            {

                var result = FindExecutableInPath(command, folders);

                if (result == null)
                {
                    Console.WriteLine($"{userInput}: command not found");
                }
                else
                {
                    Process.Start(command, args).WaitForExit();
                }
            }
        }
    }

    private static string? FindExecutableInPath(string command, string[] folders)
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

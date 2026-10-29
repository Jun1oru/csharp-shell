using System.Diagnostics;

class Program
{
    static readonly List<string> builtIn = ["exit", "echo", "type", "pwd", "cd"];
    static readonly string PATH = Environment.GetEnvironmentVariable("PATH");
    static readonly string[] folders = PATH.Split(Path.PathSeparator) ?? [];

    static void Main()
    {


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
                HandleEcho(args);
            }
            else if (command == "type")
            {
                HandleType(args[0]);
            }
            else if (command == "pwd")
            {
                HandlePwd();
            }
            else if (command == "cd")
            {
                HandleCd(args[0]);
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

    private static void HandleEcho(string[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }

    private static void HandleType(string arg)
    {
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

    private static void HandlePwd()
    {
        string folder = Directory.GetCurrentDirectory();
        Console.WriteLine($"{folder}");
    }

    private static void HandleCd(string arg)
    {
        if (Directory.Exists(arg))
        {
            Directory.SetCurrentDirectory(arg);
        }
        else
        {
            Console.WriteLine($"cd: {arg}: No such file or directory");
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

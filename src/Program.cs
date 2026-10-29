using System.Diagnostics;

class Program
{
    static readonly Dictionary<string, Action<string[]>> commands = new()
    {
        { "echo",   HandleEcho },
        { "type",   HandleType },
        { "pwd",    HandlePwd },
        { "cd",     HandleCd }
    };
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

            if (commands.TryGetValue(command, out var handler))
            {
                handler(args);
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

    private static void HandleType(string[] args)
    {
        string arg = args[0];

        if (commands.ContainsKey(arg) || arg == "exit")
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

    private static void HandlePwd(string[] args)
    {
        string folder = Directory.GetCurrentDirectory();
        Console.WriteLine($"{folder}");
    }

    private static void HandleCd(string[] args)
    {
        var targetPath = (args[0] == "~") ? Environment.GetEnvironmentVariable("HOME") : args[0];
        if (Directory.Exists(targetPath))
        {
            Directory.SetCurrentDirectory(targetPath);
        }
        else
        {
            Console.WriteLine($"cd: {targetPath}: No such file or directory");
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
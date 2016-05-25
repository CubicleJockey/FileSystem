using System.IO;
using static System.Console;
using static System.Environment;
using System.Security.Permissions;

namespace Watcher
{
    public class Program
    {
        public static void Main()
        {
            Execute();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Execute()
        {
            var args = GetCommandLineArgs();

            if (args.Length != 2)
            {
                WriteLine("Usage: Watcher.exe [directory]");
                return;
            }

            var directory = new DirectoryInfo(args[1]);
            directory.Refresh();
            if (!directory.Exists)
            {
                WriteLine($"Directory {directory.Name} at {directory.FullName.Replace(directory.Name, string.Empty)} doesn't exists, creating it...");
                directory.Create();
                WriteLine($"{directory.FullName} created.");
            }

            var watcher = new FileSystemWatcher
            {
                Path  = directory.FullName,
                Filter = "*.txt",
                EnableRaisingEvents = true
            };

            watcher.Created += (src, fileSystemEventArgs) => { WriteLine($"{fileSystemEventArgs.Name} was created."); };
            watcher.Renamed += (src, renamedEventArgs)    => { WriteLine($"{renamedEventArgs.OldName} renamed to {renamedEventArgs.Name}."); };
            watcher.Deleted += (src, fileSystemEventArgs) => { WriteLine($"{fileSystemEventArgs.Name} was deleted."); };
            watcher.Changed += (src, fileSystemEventArgs) => { WriteLine($"Contents of {fileSystemEventArgs.Name} have changed."); };

            //Hold until user wants to quit.
            WriteLine("Press 'q' to quit the watcher.");
            while (Read() != 'q')
            {
                //Wait on user.
            }
        }
    }
}

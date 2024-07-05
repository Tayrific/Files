using System.IO;
using static System.Console;

namespace Files
{
    class Program
    {

        static void Main(string[] args)
        {
            WriteLine("Parsing command line options");
            //var command = args[0];

            var directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
            {
                WriteLine($"ERROR: the directory {directoryToWatch} does not exist");
                return;
            }

            WriteLine($"Watching directory {directoryToWatch} for changes");
            using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
            {
                inputFileWatcher.IncludeSubdirectories = false;
                inputFileWatcher.InternalBufferSize = 32_768;
                inputFileWatcher.Filter = "*.*";
                inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

                inputFileWatcher.Created += FileCreated;
                inputFileWatcher.Changed += FileChanged;
                inputFileWatcher.Deleted += FileDeleted;
                inputFileWatcher.Renamed += FileRenamed;
                inputFileWatcher.Error += WatcherError;

                inputFileWatcher.EnableRaisingEvents = true;

                // To keep the application running
                WriteLine("Press 'q' to quit the sample.");
                while (Read() != 'q') ;
            }


            /*
            if (command == "--file")
            {
                var filePath = args[1];

                // Check if path is absolute
                if (!(Path.IsPathRooted(filePath) && !filePath.StartsWith(@"\\")))
                {
                    WriteLine($"ERROR: path '{filePath}' must be fully qualified");
                    ReadLine();
                    return;

                }

                WriteLine($"Single file {filePath} selected");
                ProcessSingleFile(filePath);
            }
            else if (command == "--dir")
            {
                var directoryPath = args[1];
                var fileType = args[2];
                WriteLine($"Directory {directoryPath} selected for {fileType} files");
                ProcessDirectory(directoryPath, fileType);
            }
            else
            {
                WriteLine("invalid");
            }
            */

            ReadLine();

            WriteLine("------------------------");
            string path = @"C:\programs\file.txt";
            // Get file name.
            string filename = Path.GetFileName(path);

            WriteLine("PATH:     {0}", path);
            WriteLine("FILENAME: {0}", filename);
            string filename2 = Path.GetFileNameWithoutExtension(path);

            WriteLine("PATH:         {0}", path);
            WriteLine("NO EXTENSION: {0}", filename2);
            string value1 = @"C:\perls\word.txt";
            string value2 = @"C:\file.excel.dots.xlsx";

            // ... Get extensions.
            string ext1 = Path.GetExtension(value1);
            string ext2 = Path.GetExtension(value2);
            WriteLine(ext1);
            WriteLine(ext2);

            WriteLine("------------------------");

            // Put all file names in root directory into array.
            string[] array1 = Directory.GetFiles(@"C:\Users\tayyi\Downloads\");

            // Put all bin files in root directory into array.
            // ... This is case-insensitive.
            string[] array2 = Directory.GetFiles(@"C:\", "*.BIN");

            // Display all files.
            WriteLine("--- Files: ---");
            foreach (string name in array1)
            {
                WriteLine(name);
            }

            // Display all BIN files.
            WriteLine("--- BIN Files: ---");
            foreach (string name in array2)
            {
                WriteLine(name);
            }

            WriteLine("------------------------");
            ReadLine();
        }

        static void ProcessSingleFile(string filePath)
        {
            var fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }

        static void ProcessDirectory(string DirectoryPath, string fileType)
        {
            switch (fileType)
            {
                case "TEXT":
                    string[] textFiles = Directory.GetFiles(DirectoryPath, "*.txt");
                    foreach (string textFilePath in textFiles)
                    {
                        var fileProcessor = new FileProcessor(textFilePath);
                        fileProcessor.Process();
                    }
                    break;
                default:
                    WriteLine($"ERROR: {fileType} is not supported");
                    return;
            }
        }

        static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");
        }

        static void FileChanged(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");
        }

        static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
        }

        static void FileRenamed(object sender, RenamedEventArgs e)
        {
            WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        static void WatcherError(object sender, ErrorEventArgs e)
        {
            WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
        }
    }
}

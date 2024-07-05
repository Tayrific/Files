using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Files
{
    internal class FileProcessor
    {
        private const string BackUpDirectoryName = "backup";
        private const string InProgressDirectoryName = "processing";
        private const string CompletedDirectoryName = "complete";

        public string InputFilePath { get; }
        public FileProcessor(string filePath) => InputFilePath = filePath;

        public void Process()
        {
            WriteLine($"Begin process of {InputFilePath}");

            // Check if file exist
            if (!File.Exists(InputFilePath))
            {
                WriteLine($"ERROR: file {InputFilePath} does not exist.");
                return;
            }

            // Parent path name
            string rootDirectoryPath = new DirectoryInfo(InputFilePath).Parent.FullName;
            if (rootDirectoryPath == null )
            {
                throw new InvalidOperationException($"Cannot determine root directory path");
            }
            WriteLine($"Root data path is {rootDirectoryPath}");

            // string rootsDirectoryPath = new DirectoryInfo(InputFilePath).Parent.Parent.FullName;
            // WriteLine($"Next root data path is {rootsDirectoryPath}");

            // Check if backup dir exists
            string backupDirectoryPath = Path.Combine(rootDirectoryPath, BackUpDirectoryName);

            if (!Directory.Exists(backupDirectoryPath))
            {
                WriteLine($"Creating: {backupDirectoryPath}");
                Directory.CreateDirectory(backupDirectoryPath);
            }

            // Copy file to backup dir
            string inputFileName = Path.GetFileName(InputFilePath);
            string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
            WriteLine($"Copying {InputFilePath} to {backupFilePath}");
            File.Copy(InputFilePath, backupFilePath, true);

            // Move to in progress dir
            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InProgressDirectoryName));
            string inProgressFilePath = Path.Combine(rootDirectoryPath, InProgressDirectoryName, inputFileName);

            if (File.Exists(inProgressFilePath))
            {
                WriteLine($"ERROR: file {inProgressFilePath} already exist"); //delete file already within to not get this error
                return;
            }

            WriteLine($"Moving {inputFileName} to {inProgressFilePath}");
            File.Move(InputFilePath, inProgressFilePath);

            // Determine type of file
            string extension = Path.GetExtension(InputFilePath);
            switch(extension)
            {
                case ".txt":
                    WriteLine($"{extension} file type");
                    ProcessTextFile(inProgressFilePath);
                    break;
                default:
                    WriteLine($"{extension} is an unsupported file type.");
                    break;
            }

            // Move file after processing is complete
            string completedDirectoryPath = Path.Combine(rootDirectoryPath, CompletedDirectoryName);
            Directory.CreateDirectory(completedDirectoryPath);

            string fileNameWithCompletedExtention = Path.ChangeExtension(inputFileName, ".complete");
            string completedFileName = $"{Guid.NewGuid()}_{fileNameWithCompletedExtention}";

            string completedFilePath = Path.Combine(completedDirectoryPath, completedFileName);

            WriteLine($"Moving {inProgressFilePath} to {completedFilePath}");
            File.Move(inProgressFilePath, completedFilePath);

            // Deleting a directory
            string inProgressDirectoryPath = Path.GetDirectoryName(inProgressFilePath);
            Directory.Delete(inProgressDirectoryPath, true);


        }

        private void ProcessTextFile(string inProgressFilePath)
        {
            WriteLine($"Processing text file {inProgressFilePath}");
            // Read in and process
        }
    }
}

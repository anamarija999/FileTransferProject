using FileTransferProject.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FileTransferProject.Services
{
    public class UserInputService : IUserInputService
    {
        private readonly ILogger<UserInputService> _logger;
        public UserInputService(ILogger<UserInputService> logger)
        {
            _logger = logger;
        }

        public string GetSourceFilePath()
        {
            _logger.LogInformation("Prompting user for source file path.");
            Console.Write("Enter source file path: ");
            string filePath = Console.ReadLine();
            _logger.LogInformation("User entered source file path: {FilePath}", filePath);
            return filePath;
        }

        public string GetDestinationFolder()
        {
            _logger.LogInformation("Prompting user for destination folder path.");
            Console.Write("Enter destination folder path: ");
            string folderPath = Console.ReadLine();
            _logger.LogInformation("User entered destination folder path: {FolderPath}", folderPath);
            return folderPath;
        }
    }
}

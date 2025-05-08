using FileTransferApp.Services;
using FileTransferProject.Interfaces;
using FileTransferProject.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileTransferApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Add logging services
                    services.AddLogging(configure => configure.AddConsole());

                    // Register services with logging dependency injection
                    services.AddSingleton<IFileChunkReader, FileChunkReader>();
                    services.AddSingleton<IFileChunkWriter, FileChunkWriter>();
                    services.AddSingleton<IChunkHasher, ChunkHasher>();
                    services.AddSingleton<IFileHasher, FileHasher>();
                    services.AddSingleton<ITransferManager, TransferManager>();
                    services.AddSingleton<UserInputService>();
                })
                .Build();

            var transferManager = host.Services.GetRequiredService<ITransferManager>();
            var userInputService = host.Services.GetRequiredService<UserInputService>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting file transfer application...");

                string sourceFilePath = userInputService.GetSourceFilePath();

                if (!File.Exists(sourceFilePath))
                {
                    logger.LogError("Source file does not exist: {SourceFilePath}", sourceFilePath);
                    return;
                }

                string destinationFolderPath = userInputService.GetDestinationFolder();

                if (!Directory.Exists(destinationFolderPath))
                {
                    logger.LogError("Destination folder does not exist: {DestinationFolderPath}", destinationFolderPath);
                    return;
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                await transferManager.StartTransferAsync(sourceFilePath, destinationFilePath);
                logger.LogInformation("File transfer completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during file transfer.");
            }
        }
    }
}

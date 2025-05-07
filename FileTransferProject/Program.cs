using FileTransferApp.Services;
using FileTransferProject.Interfaces;
using FileTransferProject.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileTransferApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IFileChunkReader, FileChunkReader>();
                    services.AddSingleton<IFileChunkWriter, FileChunkWriter>();
                    services.AddSingleton<IChunkHasher, ChunkHasher>();
                    services.AddSingleton<IFileHasher, FileHasher>();
                    services.AddSingleton<ITransferManager, TransferManager>();
                })
                .Build();

            var transferManager = host.Services.GetRequiredService<ITransferManager>();

            try
            {
                Console.Write("Enter source file path (e.g. c:\\source\\my_large_file.bin): ");
                string sourceFilePath = Console.ReadLine();

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Error: Source file does not exist.");
                    return;
                }

                Console.Write("Enter destination folder path (e.g. d:\\destination\\): ");
                string destinationFolderPath = Console.ReadLine();

                if (!Directory.Exists(destinationFolderPath))
                {
                    Console.WriteLine("Error: Destination folder does not exist.");
                    return;
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                await transferManager.StartTransferAsync(sourceFilePath, destinationFilePath);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }
    }
}

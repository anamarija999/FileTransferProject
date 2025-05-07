using FileTransferProject.Interfaces;
using FileTransferProject.Models;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace FileTransferApp.Services
{
    public class TransferManager : ITransferManager
    {
        private readonly IFileChunkReader _reader;
        private readonly IFileChunkWriter _writer;
        private readonly IFileHasher _hasher;

        public TransferManager(
            IFileChunkReader reader,
            IFileChunkWriter writer,
            IFileHasher hasher)
        {
            _reader = reader;
            _writer = writer;
            _hasher = hasher;
        }

        public async Task StartTransferAsync(string sourcePath, string destinationPath)
        {
            var channel = Channel.CreateBounded<FileChunk>(new BoundedChannelOptions(250)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
            await Task.WhenAll(
                        Task.Run(() => _reader.ReadChunksAsync(sourcePath, channel.Writer)),
                        Task.Run(() => _writer.WriteChunks(destinationPath, channel.Reader))
                               );
            string srcHash = _hasher.ComputeFileHash(sourcePath, "SHA256");
            string dstHash = _hasher.ComputeFileHash(destinationPath, "SHA256");

            Console.WriteLine($"\nSource SHA256: {srcHash}");
            Console.WriteLine($"Destination SHA256:   {dstHash}");
            if (srcHash == dstHash)
            {
                Console.WriteLine("\n hashes match. The file transfer is successful.");
            }
            else
            {
                Console.WriteLine("\n hashes do not match. The file transfer failed.");
            }
        }
    }
}

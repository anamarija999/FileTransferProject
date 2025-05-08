using FileTransferProject.Interfaces;
using FileTransferProject.Models;
using System;
using System.Buffers;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FileTransferApp.Services
{
    public class FileChunkWriter : IFileChunkWriter
    {
        private readonly IChunkHasher _hasher;

        public FileChunkWriter(IChunkHasher hasher)
        {
            _hasher = hasher;
        }

        public async Task WriteChunks(string filePath, ChannelReader<FileChunk> reader)
        {
            await using var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1024 * 1024, useAsync: true);

            await foreach (var chunk in reader.ReadAllAsync())
            {
                bool verified = false;
                int attempt = 0;

                while (!verified && attempt < 3)
                {
                    fs.Seek(chunk.Position, SeekOrigin.Begin);
                    await fs.WriteAsync(chunk.Data, 0, chunk.Length);
                    await fs.FlushAsync();

                    byte[] readBack = ArrayPool<byte>.Shared.Rent(chunk.Length);
                    try
                    {
                        fs.Seek(chunk.Position, SeekOrigin.Begin);
                        await fs.ReadAsync(readBack, 0, chunk.Length);

                        string checkHash = _hasher.ComputeMd5(readBack.AsSpan(0, chunk.Length).ToArray());

                        if (checkHash == chunk.Md5Hash)
                        {
                            Console.WriteLine($"Verified: position={chunk.Position}, hash={checkHash}");
                            verified = true;
                        }
                        else
                        {
                            Console.WriteLine($"Retry: position={chunk.Position}, attempt {++attempt}");
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(readBack);
                    }
                }

                if (!verified)
                {
                    throw new IOException($"Failed to verify chunk at {chunk.Position} after 3 retries.");
                }
            }
        }
    }
}

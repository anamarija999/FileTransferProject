using FileTransferProject.Interfaces;
using FileTransferProject.Models;
using System;
using System.Buffers;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FileTransferProject.Services
{
    public class FileChunkReader : IFileChunkReader
    {
        private const int BlockSize = 1024 * 1024;
        private readonly IChunkHasher _hasher;

        public FileChunkReader(IChunkHasher hasher)
        {
            _hasher = hasher;
        }

        public async Task ReadChunksAsync(string filePath, ChannelWriter<FileChunk> writer)
        {
            await using var fs = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BlockSize,
                useAsync: true);
            try
            {
                while (true)
                {
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(BlockSize);
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = await fs.ReadAsync(buffer, 0, BlockSize);
                        if (bytesRead == 0)
                            break;

                        string md5 = _hasher.ComputeMd5(buffer.AsSpan(0, bytesRead).ToArray());

                        var chunk = new FileChunk
                        {
                            Position = fs.Position - bytesRead,
                            Data = buffer[..bytesRead],
                            Length = bytesRead,
                            Md5Hash = md5
                        };

                        await writer.WriteAsync(chunk);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }

                writer.Complete();
            }
            catch (OperationCanceledException ex)
            {
                writer.Complete(ex);
            }

        }
    }
}

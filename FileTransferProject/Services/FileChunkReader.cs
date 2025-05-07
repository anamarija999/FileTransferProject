using FileTransferProject.Interfaces;
using FileTransferProject.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task ReadChunksAsync(string filePath, ChannelWriter<FileChunk> writer, CancellationToken cancellationToken = default)
        {
            await using var fs = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BlockSize,
                useAsync: true);

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                byte[] buffer = new byte[BlockSize];
                int bytesRead = await fs.ReadAsync(buffer, cancellationToken);

                if (bytesRead == 0)
                    break;

                if (bytesRead < BlockSize)
                {
                    Array.Resize(ref buffer, bytesRead);
                }

                string md5 = _hasher.ComputeMd5(buffer);

                var chunk = new FileChunk
                {
                    Position = fs.Position - bytesRead,
                    Data = buffer,
                    Length = bytesRead,
                    Md5Hash = md5
                };

                await writer.WriteAsync(chunk, cancellationToken);
            }

            writer.Complete();
        }
    }

}
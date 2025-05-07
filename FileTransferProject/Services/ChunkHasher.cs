using FileTransferProject.Interfaces;
using System.Security.Cryptography;

namespace FileTransferProject.Services
{
    public class ChunkHasher : IChunkHasher
    {
        public string ComputeMd5(byte[] data)
        {
            try
            {
                var hash = MD5.HashData(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error computing hash: {ex.Message}");
                throw;
            }
           
        }
    }
}

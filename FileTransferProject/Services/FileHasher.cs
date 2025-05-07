using FileTransferProject.Interfaces;
using System.Security.Cryptography;

namespace FileTransferProject.Services
{
    public class FileHasher : IFileHasher
    {
        public string ComputeFileHash(string filePath, string algorithm = "SHA256")
        {
            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using HashAlgorithm hasher = algorithm.ToUpper() switch
                {
                    "SHA1" => SHA1.Create(),
                    "SHA256" => SHA256.Create(),
                    _ => throw new ArgumentException("Unsupported algorithm", nameof(algorithm))
                };

                byte[] hash = hasher.ComputeHash(fs);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error computing hash: {ex.Message}");
                throw; 
            }

        }
    }

}

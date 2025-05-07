using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferProject.Interfaces
{
    public interface IFileHasher
    {
        /// <summary>
        /// Computes a cryptographic hash (SHA1 or SHA256) of the entire file.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="algorithm">Hash algorithm ("SHA1" or "SHA256"). Defaults to "SHA256".</param>
        /// <returns>Hex-encoded hash string.</returns>
        string ComputeFileHash(string filePath, string algorithm);
    }
}

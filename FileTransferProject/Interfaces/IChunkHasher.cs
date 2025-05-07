using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferProject.Interfaces
{
    public interface IChunkHasher
    {
        /// <summary>
        /// Computes the MD5 hash of a data block (chunk).
        /// </summary>
        /// <param name="data">The byte array to hash.</param>
        /// <returns>Hex-encoded MD5 hash string.</returns>
        string ComputeMd5(byte[] data);
    }
}

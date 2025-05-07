using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferProject.Interfaces
{
    public interface ITransferManager
    {
        /// <summary>
        /// Starts an asynchronous file transfer from sourcePath to destinationDir,
        /// including chunk-based copy and final SHA256 integrity verification.
        /// </summary>
        /// <param name="sourcePath">The full path of the source file to transfer.</param>
        /// <param name="destinationDir">The directory where the file should be copied to.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task StartTransferAsync(string sourcePath, string destinationDir);
    }
}

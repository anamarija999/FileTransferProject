using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferProject.Models
{
    public class FileChunk
    {
        public long Position { get; set; }
        public byte[] Data { get; set; }
        public int Length { get; set; }
        public string Md5Hash { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferProject.Interfaces
{
    public interface IUserInputService
    {
        string GetSourceFilePath();
        string GetDestinationFolder();
    }
}

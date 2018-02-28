using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.FileManagement;

namespace WIM.Core.Service.FileManagement
{
    public interface IFileService : IService
    {
        IEnumerable<File_MT> GetFiles();
        File_MT GetFilesByFileIDSys(string id);
        void GetFile(string fileIDSys);
        string CreateFile(File_MT file);
        IEnumerable<File_MT> CreateFiles(IEnumerable<File_MT> files);
        bool UpdateFile(File_MT file);
        bool DeleteFile(int id);
    }
}

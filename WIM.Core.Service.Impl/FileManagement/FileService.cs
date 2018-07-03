using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.FileManagement;
using WIM.Core.Repository.FileManagement;
using WIM.Core.Repository.Impl.FileManagement;
using WIM.Core.Service.FileManagement;

namespace WIM.Core.Service.Impl.FileManagement
{
    public class FileService : Service, IFileService
    {
        public FileService()
        {

        }

        public IEnumerable<File_MT> GetFiles()
        {
            IEnumerable<File_MT> files;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IFileRepository repo = new FileRepository(Db);
                files = repo.Get();
            }
            return files;
        }

        public File_MT GetFilesByFileIDSys(string id)
        {
            File_MT file;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IFileRepository repo = new FileRepository(Db);
                file = repo.GetByID(id);
            }
            return file;
        }

        public void GetFile(string fileIDSys)
        {
            string root = @"D:\Uploads\WIM";
            IFileService fileService = new FileService();
            File_MT file;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IFileRepository repo = new FileRepository(Db);
                file = repo.GetSingle(a => a.FileRefID == fileIDSys);
            }
            string fileName = file.FileName;
            string[] temp = fileName.Split('.');
            string extension = temp[temp.Length - 1];
            root += @"\" +extension.ToUpper()+@"\"+file.LocalName;

            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.ContentType = ReturnExtension("."+extension);
            response.AddHeader("Content-Disposition", "attachment;filename="+fileName);
            byte[] data = req.DownloadData(root);
            response.BinaryWrite(data);
            response.End();
        }

        public string CreateFile(File_MT file)
        {
            using (var scope = new TransactionScope())
            {
                File_MT newFile = new File_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IFileRepository repo = new FileRepository(Db);
                        newFile = repo.Insert(file);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return newFile.FileRefID;
            }
        }

        public IEnumerable<File_MT> CreateFiles(IEnumerable<File_MT> files)
        {
            using (var scope = new TransactionScope())
            {
                List<File_MT> newFile = new List<File_MT>();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        foreach (var file in files)
                        {
                            file.FileRefID = Guid.NewGuid().ToString();
                            IFileRepository repo = new FileRepository(Db);
                            newFile.Add(repo.Insert(file));
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();

                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return newFile;
            }
        }

        public bool UpdateFile(File_MT file)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(int id)
        {
            throw new NotImplementedException();
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.SqlLog
{
    public class LogWriter
    {
        public static void WritetoFile(string project, string method, string data)
        {
            DateTime today = DateTime.Now;
            string path = @"D:\SqlLogs\" + project.Trim() + "\\" + method.Trim() + "\\" + today.ToString("yyyy") + "\\" + today.ToString("MM");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string pathFile = path + "\\FileLogs_d" + today.ToString("dd") + ".txt";

            using (var f = File.AppendText(pathFile))
            {
                data = "At=> " + today.ToString("hh:mm:ss  dd-MM-yyyy ")
                   + " ,Path=> " + method
                   + " ,Sql=> " + data.Replace(System.Environment.NewLine, "");
                f.WriteLine(data);
            }

        }
    }
}

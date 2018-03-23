using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.UtilityHelpers
{
    public static class FileHelper
    {
        public static List<string> ReadTextFileBySplit(string path,bool isRemove = false,char split = ',',string backupPath = "")
        {
            List<string> ret = new List<string>();

            if(File.Exists(path))
            {
                string texts = File.ReadAllText(path);
                ret = texts.Split(split).Distinct().ToList();
                ret.RemoveAll(w => string.IsNullOrEmpty(w));
                if(!string.IsNullOrEmpty(backupPath))
                {
                    string fileName = Path.GetFileName(path);
                    File.Move(Path.Combine(backupPath, fileName), path);
                }
                else
                {
                    File.Delete(path);
                }
              
            }
            

            return ret;
        }
        public static List<T> ReadJsonFileToListJsonObj<T>(string path)
        {
            List<T> ret = new List<T>();

            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    var json = r.ReadToEnd();
                    ret = JsonConvert.DeserializeObject<List<T>>(json);
                }
            }

            return ret;
        }
        public static T ReadJsonFileToJsonObj<T>(string path)
        {
            T ret = default(T);

            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    var json = r.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<T>>(json);
                    if(list.Count > 0)
                    {
                        ret = list.FirstOrDefault();
                    }
                }
            }

            return ret;
        }
    }
}

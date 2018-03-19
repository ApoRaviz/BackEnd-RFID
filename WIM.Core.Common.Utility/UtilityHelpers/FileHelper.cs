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
        public static List<string> ReadTextFileBySplit(string path,char split = ',')
        {
            List<string> ret = new List<string>();

            if(File.Exists(path))
            {
                string texts = File.ReadAllText(path);
                ret = texts.Split(split).ToList();
                ret.RemoveAll(w => string.IsNullOrEmpty(w));
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

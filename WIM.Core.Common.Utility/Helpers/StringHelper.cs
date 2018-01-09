using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Helpers
{
    public class StringHelper
    {
        public static string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                string x = Encoding.UTF8.GetString(buffer);
                return Encoding.UTF8.GetString(buffer);
            }

        }

        public static string String2Hex(string InputText)
        {
            byte[] b = Encoding.Default.GetBytes(InputText);
            var hexString = BitConverter.ToString(b);
            return hexString.Replace("-", "");
        }

        public static string Hex2String(string InputText)
        {
            byte[] bb = Enumerable.Range(0, InputText.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(InputText.Substring(x, 2), 16))
                             .ToArray();
            return Encoding.ASCII.GetString(bb);
        }

        public static string GetRequestUrl(string fullUrl)
        {
            fullUrl = fullUrl.Replace("wimapi/", "");
            string[] fullUrlSplit = fullUrl.Split('?');
            string reqUrl = fullUrlSplit[0];          

            if (reqUrl.Last() == '/')
            {
                reqUrl = reqUrl.Substring(0, reqUrl.Length - 1);

            }

            int indexFirstslash = reqUrl.IndexOf('/');
            int indexapiv1 = reqUrl.IndexOf("/api/v1");
            if (indexFirstslash != -1 && indexapiv1 != -1)
            {
                string first = reqUrl.Substring(0, indexFirstslash);
                string second = reqUrl.Substring(indexapiv1);
                //reqUrl = reqUrl.Substring(indexFirstslash, indexapiv1 - indexFirstslash);
                reqUrl = first + second;
            }
            return reqUrl;
        }


    }
}

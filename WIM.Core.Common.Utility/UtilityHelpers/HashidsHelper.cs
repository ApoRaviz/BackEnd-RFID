using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.UtilityHelpers
{
    public class HashidsHelper
    {
        public static readonly string HashKey = "yut";

        public static string EncodeHex(string input)
        {
            Hashids hashids = new Hashids(HashKey);
            return hashids.EncodeHex(StringHelper.String2Hex(input));
        }

        public static string DecodeHex(string input)
        {
            Hashids hashids = new Hashids(HashKey);            
            return StringHelper.Hex2String(hashids.DecodeHex(input));
        }

        

    }
}

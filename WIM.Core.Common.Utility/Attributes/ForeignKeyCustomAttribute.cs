using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ForeignKeyCustomAttribute : Attribute
    {
        public string TableName { get; }

        public ForeignKeyCustomAttribute(string tableName)
        {
            TableName = tableName;
        }


        
    }

}

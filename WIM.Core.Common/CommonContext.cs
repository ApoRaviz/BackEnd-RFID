using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common
{
    partial class CommonContext
    {
        public CommonContext(string name) : base(name) { }
        static public CommonContext Create()
        {
            var entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.ProviderConnectionString = ConfigurationManager.ConnectionStrings["CORE"].ConnectionString;
            entityBuilder.Provider = "System.Data.SqlClient";
            //entityBuilder.Metadata = @"res://*/CommonDataModel.csdl|res://*/CommonDataModel.ssdl|res://*/CommonDataModel.msl";

            return new CommonContext(entityBuilder.ConnectionString);
        }
    }
}

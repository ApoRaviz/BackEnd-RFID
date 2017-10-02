using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master
{
    partial class MasterContext
    {
        public MasterContext(string name) : base(name)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        static public MasterContext Create()
        {            
            var entityBuilder = new EntityConnectionStringBuilder()
            {
                ProviderConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                Provider = "System.Data.SqlClient",
                Metadata = @"res://*/MasterDataModel.csdl|res://*/MasterDataModel.ssdl|res://*/MasterDataModel.msl"
            };
            return new MasterContext(entityBuilder.ConnectionString);
        }
    }
}

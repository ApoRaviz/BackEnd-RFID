using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Master.Context
{

    public class MasterDbContext : DbContext
    {

        public MasterDbContext() : base("name=DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static MasterDbContext Create()
        {
            return new MasterDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public string ProcGetNewID(string prefixes)
        {
            var prefixesParameter = new SqlParameter
            {
                ParameterName = "Prefixes",
                Value = prefixes
            };

            return Database.SqlQuery<string>("exec ProcGetNewID @Prefixes", prefixesParameter).SingleOrDefault();
        }        
    }
}

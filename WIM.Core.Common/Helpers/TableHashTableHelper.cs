using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WMS.Context;

namespace WIM.Core.Common.Helpers
{
    public class TableHashTableHelper
    {
        public static Hashtable tableTable;

        public static void Initialize()
        {
            tableTable = new Hashtable();
            using (CoreDbContext db = new CoreDbContext())
            {
                var table = db.GetType().GetProperties();

                foreach (var atibute in table)
                {
                    if (atibute.CanWrite)
                    {
                        TableDescription temp = new TableDescription();
                        temp.PrimaryKey = new List<string>();
                        temp.ForeignKey = new List<string>();
                        var classobject = Activator.CreateInstance(atibute.PropertyType.GetGenericArguments()[0]);
                        temp.TableName = classobject.GetType().CustomAttributes.ToList()[0].ConstructorArguments[0].Value.ToString();

                        var properties = classobject.GetType().GetProperties();
                        foreach(var prop in properties)
                        {
                            var property = prop.CustomAttributes.ToList();
                            if (property.Count > 0)
                            {
                                if(property[0].ConstructorArguments.Count > 0)
                                {
                                    foreach(var fk in property[0].ConstructorArguments)
                                    {
                                        temp.ForeignKey.Add(fk.Value.ToString());
                                    }
                                }
                                else
                                {
                                    temp.PrimaryKey.Add(prop.Name.ToString());
                                }
                            }
                        }
                        tableTable.Add(temp.TableName,temp);
                    }
                }
            }

            using (WMSDbContext db = new WMSDbContext())
            {

                var table = db.GetType().GetProperties();

                foreach (var atibute in table)
                {
                    if (atibute.CanWrite)
                    {
                        TableDescription temp = new TableDescription();
                        temp.PrimaryKey = new List<string>();
                        temp.ForeignKey = new List<string>();
                        var classobject = Activator.CreateInstance(atibute.PropertyType.GetGenericArguments()[0]);
                        temp.TableName = classobject.GetType().CustomAttributes.ToList()[0].ConstructorArguments[0].Value.ToString();

                        var properties = classobject.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            var property = prop.CustomAttributes.ToList();
                            if (property.Count > 0)
                            {
                                if (property[0].ConstructorArguments.Count > 0)
                                {
                                    foreach (var fk in property[0].ConstructorArguments)
                                    {
                                        temp.ForeignKey.Add(fk.Value.ToString());
                                    }
                                }
                                else
                                {
                                    temp.PrimaryKey.Add(prop.Name);
                                }
                            }
                        }
                        if (!tableTable.ContainsKey(temp.TableName))
                        {
                            tableTable.Add(temp.TableName, temp);
                        }
                        
                    }
                }
            }
        }
    }

    public class TableDescription
    {
        public string TableName { get; set; }
        public List<string> PrimaryKey { get; set; }
        public List<string> ForeignKey { get; set; }
    }
}

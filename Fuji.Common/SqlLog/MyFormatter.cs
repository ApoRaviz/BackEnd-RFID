using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.SqlLog
{
    public class MyFormatter : DatabaseLogFormatter
    {
        public MyFormatter(DbContext context, Action<string> writeAction)
           : base(context, writeAction)
        {
        }

        public override void LogCommand<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
        }

        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            string err = interceptionContext.Exception?.Message.Replace(Environment.NewLine, "") ?? string.Empty;
            err = string.IsNullOrEmpty(err) ? "" : "Exception : " + err + " ,";
            string cmd = command.CommandText.Replace(Environment.NewLine, "");
            string param = "";
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                if (command.Parameters[i].Value != null)
                {
                    cmd = cmd.Replace("@" + i, "\'" + command.Parameters[i].Value.ToString() + "\'");
                    param += command.Parameters[i].Value.ToString() + ",";
                }
            }
            param = param.Length > 0 ? string.Format("Params : '{0}'", param.Trim(',')) : "";



            Write(string.Format(
            "{1}Command :'{0}' {2}"
             , cmd
             , err
             , param));
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        public DemoController()
        {

        }

        [HttpPost]
        [Route("func1")]
        public HttpResponseMessage Func1([FromBody]List<LabelConfig> projectConfig)
        {
            LabelControl labelResponse = new LabelControl();
            ResponseData<LabelControl> response = new ResponseData<LabelControl>();


            using (CoreDbContext db = new CoreDbContext())
            {
                LabelControl label1 = db.LabelControl.SingleOrDefault(p => p.LabelIDSys == 1);



                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(@"d:\Web\ftproot\lang\en.json"))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, projectConfig);

                    }
                }               

                label1.LabelConfig = projectConfig;
                db.SaveChanges();

                var labels = (
                      from p in db.LabelControl
                      select p
                ).ToList();

                try
                {
                    labelResponse = labels.FirstOrDefault(p => p.LabelConfig.FirstOrDefault().Key == "ItemCode");
                    response.SetData(labelResponse);
                }
                catch (NullReferenceException)
                {

                }
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}
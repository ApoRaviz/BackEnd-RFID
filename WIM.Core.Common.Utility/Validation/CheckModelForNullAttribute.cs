using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Common.Utility.Validation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public CheckModelForNullAttribute() : this(arguments =>
     arguments.ContainsValue(null))
        { }

        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                string rawRequest;
                if (!_validate(actionContext.ActionArguments))
                {

                    using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
                    {
                        stream.BaseStream.Position = 0;
                        rawRequest = stream.ReadToEnd();
                        Dictionary<string, dynamic> jobject = (Dictionary<string, dynamic>)new JavaScriptSerializer().Deserialize<object>(rawRequest);
                        //var m = jobject.Value;

                        var a = (Dictionary<string, dynamic>)actionContext.ActionArguments;
                        
                        bool found = false;
                        foreach (var classname in a)
                        { 
                           
                            if(classname.Value.GetType().Name == "Project_MT")
                            {
                                found = true;
                                
                                if (classname.Value == null)
                                {
                                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                                }
                                var z = classname.Value;
                            var y = z.GetType();
                            var w = y.GetProperties();
                            foreach (var propname in w)
                            {
                                    if (propname.Name == "ProjectConfig")
                                    {
                                        if (jobject.ContainsKey(propname.Name))
                                        {
                                            dynamic data;
                                            jobject.TryGetValue(propname.Name, out data);
                                            var x = data.GetType();
                                            var send = propname.GetValue(z, null);
                                            CheckNullProperties(data, send);
                                        }
                                        else
                                        {
                                            throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                                        }
                                    }
                                   
                                }
                            }
                        }
                        if (!found)
                        {
                            throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                        }
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, "The argument cannot be null");
                }
            }
            catch (AppValidationException)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, "The argument cannot be null");
            }

        }

        public void CheckNullProperties(Dictionary<string, dynamic> dictionary, dynamic motherclass)
        {


            var a = motherclass;
            var b = a.GetType();
            var c = b.GetProperties();
            foreach (var propname in c)
            {
                if (dictionary.ContainsKey(propname.Name))
                {
                    dynamic data;
                    dictionary.TryGetValue(propname.Name, out data);
                    var x = data.GetType();
                    var send = propname.GetValue(motherclass, null);
                    if (propname.PropertyType.IsGenericType)
                    {
                        if(propname.PropertyType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                        {
                            CheckNullProperties(data, send);
                        }
                        
                    }

                }
                else
                {
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
            }

        }


    }
}

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Isuzu.WebApi.Providers.Firebase
{
    public class Firebase
    {
        public IRestResponse SendNotificationsToMobile(FirebaseModelSand paramSend)
        {
            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "7ac54771-f78f-ec6a-d696-13ed30e5a78a");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "key=AAAAIHhGBdc:APA91bFQgxNf-tnbs6arV02RsZ5m5aZsAPGdI4Yao3icMuhXeG70cVn07S56kgUrAw_gKJY87hfzdPjCkKPCMo5MyjbRxcRUmy87AN5w58BOEZH_swGMYUdFyVD4IOAU7KJZfVq2iAXe");
            request.AddParameter("application/json", JsonConvert.SerializeObject(paramSend), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;

        }
    }
}
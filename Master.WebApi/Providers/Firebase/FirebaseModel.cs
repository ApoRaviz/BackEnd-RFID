using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Master.WebApi.Providers.Firebase.Model
{
    public class FirebaseModelSand
    {
        public FirebaseModelSand()
        {
            this.notification = new Notification();
        }
        public string to { get; set; }
        public string priority { get; set; }
        public Notification notification { get; set; }
        public class Notification
        {
            public object custom { get; set; }
            public string icon { get; set; }
            public string sound { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }
    }


}
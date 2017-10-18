using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasProducerTest
{
    public class Chat2BrandPayload
    {
        public string message_id { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string transport { get; set; }
        public string client_id { get; set; }
        public object operator_id { get; set; }
        public object dialog_id { get; set; }
        public string channel_id { get; set; }
        public Chat2BrandClient client { get; set; }
    }

    public class Chat2BrandClient
    {
        public string id { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public object assigned_name { get; set; }
    }

}

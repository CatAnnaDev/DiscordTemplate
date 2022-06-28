using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTemplate.Payload
{
    internal class payloadData
    {
        public string LoginPayload = @"{'op': 2,'d': {'token': '','properties': {'$os': 'windows','$browser': 'chrome','$device': 'pc'}}}";

        public string RPCPayload = @"{'op': 3, 'd': {'since': 91879201,'activities': [{'name': '','type': 3,'url': ''}],'status': '','afk': false}}";

        public string heartbeatPayload = @"{'op': 1,'d': 'null'}";
    }

    public class Properties
    {
        [JsonProperty("$os")]
        public string Os { get; set; }

        [JsonProperty("$browser")]
        public string Browser { get; set; }

        [JsonProperty("$device")]
        public string Device { get; set; }
    }

    public class A
    {
        public string token { get; set; }
        public Properties properties { get; set; }
    }

    public class PayloadEdit
    {
        public int op { get; set; }
        public A d { get; set; }
    }
}

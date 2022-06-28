using DiscordTemplate.Payload;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using DiscordTemplate.Logger;

namespace DiscordTemplate.Connection
{
    internal class Connect
    {
        public PayloadEdit PayloadEdit;
        public RichLogger Logger = new();
        private static dynamic parsedJson;
        private WebSocketReceiveResult result;

        public void Connection()
        {
            
            var socket = new ClientWebSocket();
            
            Logger.Log("Connecting !");
            socket.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=9&encording=json"), CancellationToken.None).Wait();

            var buffer = new byte[1024];

            Logger.Log("Receiving Data");
            result = socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;

            var jsonParse = Encoding.UTF8.GetString(buffer, 0, result.Count);
            dynamic jsons = JToken.Parse(jsonParse);
            int hb = jsons["d"]["heartbeat_interval"] / 50;

            var _heartbeat = new HeartBeat.HeartBeat();

            new Thread(() => _heartbeat.Heartbeat(socket, hb)).Start();

            new Thread(() => PayLoad(socket)).Start();

        }

        void PayLoad(ClientWebSocket socket)
        {
            Thread.Sleep(1000);

            PayloadEdit = JsonConvert.DeserializeObject<PayloadEdit>(new payloadData().LoginPayload);

            TextBox? t = Application.OpenForms["Form1"].Controls["textBox4"] as TextBox;

            if (t.Text != null)
                PayloadEdit.d.token = t.Text;

            PayloadEdit payloadCustom = new PayloadEdit()
            {
                op = PayloadEdit.op,
                d = PayloadEdit.d,
            };

            //Logger.Log("Sending Payload");
            var details = JsonConvert.SerializeObject(payloadCustom);
            socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(details)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            Thread.Sleep(1000);

            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[2048]);

                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        do
                        {
                            result = socket.ReceiveAsync(buffer, CancellationToken.None).Result;
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Close)
                            break;

                        ms.Seek(0, SeekOrigin.Begin);

                        string jsonss = Encoding.UTF8.GetString(ms.ToArray());
                        parsedJson = JsonConvert.DeserializeObject(jsonss);

                        if (parsedJson.t == "SESSIONS_REPLACE")
                            return;

                        if (parsedJson.op == 3)
                            Logger.Log(format_json(parsedJson));

                        if (parsedJson.t != null)
                        {
                            if (parsedJson.t == "READY")
                            {
                                // Ready Test
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        static string format_json(string json)
        {
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch { return json; }
        }
    }
}

using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using DiscordTemplate.Payload;


namespace DiscordTemplate.HeartBeat
{
    internal class HeartBeat
    {
        public void Heartbeat(ClientWebSocket socket, int hb)
        {
            Thread.CurrentThread.IsBackground = false;
            
            var details = JObject.Parse(new payloadData().heartbeatPayload);
            while (socket.State == WebSocketState.Open)
            {
                socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(details.ToString())), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                Thread.Sleep(hb);
            }
        }
    }
}

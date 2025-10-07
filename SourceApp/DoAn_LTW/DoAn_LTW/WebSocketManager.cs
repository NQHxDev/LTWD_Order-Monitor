using System;
using WebSocketSharp;

namespace DoAn_LTW
{
    public class WebSocketManager
    {
        private static WebSocket ws;
        public static event Action<string> OnMessageReceived;

        public static void Connect(string url = "ws://localhost:3000")
        {
            if (ws != null && ws.IsAlive) return;

            ws = new WebSocket(url);
            ws.OnOpen += (s, e) => Console.WriteLine("WebSocket Connected");
            ws.OnMessage += (s, e) =>
            {
                OnMessageReceived?.Invoke(e.Data);
            };
            ws.OnError += (s, e) => Console.WriteLine("Error: " + e.Message);
            ws.OnClose += (s, e) => Console.WriteLine("Closed");
            ws.Connect();
        }

        public static void Send(string msg)
        {
            if (ws != null && ws.IsAlive)
            {
                ws.Send(msg);
            }
        }

        public static void Close()
        {
            ws?.Close();
        }
    }
}

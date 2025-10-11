using System;
using System.Threading;
using WebSocketSharp;

namespace DoAn_LTW
{
    public class WebSocketManager
    {
        private static WebSocket ws;
        private static string currentUrl;
        private static Timer reconnectTimer;
        private static int reconnectAttempts = 0;
        private const int MAX_RECONNECT_ATTEMPTS = 5;
        private const int RECONNECT_INTERVAL_MS = 3000;

        public static event Action<string> OnMessageReceived;
        public static event Action<bool> OnConnectionStatusChanged;

        public static void Connect(string url = "ws://localhost:8081")
        {
            if (ws != null && ws.IsAlive) return;

            currentUrl = url;
            CreateWebSocket();
            ws.Connect();
        }

        private static void CreateWebSocket()
        {
            if (ws != null)
            {
                ws.OnOpen -= Ws_OnOpen;
                ws.OnMessage -= Ws_OnMessage;
                ws.OnError -= Ws_OnError;
                ws.OnClose -= Ws_OnClose;
                ws = null;
            }

            ws = new WebSocket(currentUrl);
            ws.OnOpen += Ws_OnOpen;
            ws.OnMessage += Ws_OnMessage;
            ws.OnError += Ws_OnError;
            ws.OnClose += Ws_OnClose;
        }

        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket Connected");
            reconnectAttempts = 0;
            StopReconnectTimer();
            OnConnectionStatusChanged?.Invoke(true);
        }

        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            OnMessageReceived?.Invoke(e.Data);
        }

        private static void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        private static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($"Closed - Code: {e.Code}, Reason: {e.Reason}");
            OnConnectionStatusChanged?.Invoke(false);

            // Nếu tự đóng sẽ trả 1000 và kết thúc
            if (e.Code != 1000)
            {
                ScheduleReconnect();
            }
        }

        private static void ScheduleReconnect()
        {
            if (reconnectAttempts >= MAX_RECONNECT_ATTEMPTS)
            {
                Console.WriteLine("Max reconnect attempts reached. Giving up.");
                return;
            }

            reconnectAttempts++;
            Console.WriteLine($"Attempting to reconnect in {RECONNECT_INTERVAL_MS / 1000} seconds... (Attempt {reconnectAttempts}/{MAX_RECONNECT_ATTEMPTS})");

            reconnectTimer = new Timer(_ =>
            {
                try
                {
                    Console.WriteLine("Reconnecting...");
                    CreateWebSocket();
                    ws.Connect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Reconnect failed: " + ex.Message);
                    ScheduleReconnect();
                }
            }, null, RECONNECT_INTERVAL_MS, Timeout.Infinite);
        }

        private static void StopReconnectTimer()
        {
            reconnectTimer?.Dispose();
            reconnectTimer = null;
        }

        public static void Send(string msg)
        {
            if (ws != null && ws.IsAlive)
            {
                ws.Send(msg);
            }
            else
            {
                Console.WriteLine("Cannot send message - WebSocket is not connected");
            }
        }

        public static void Close()
        {
            StopReconnectTimer();
            // Ngăn không cho tự động kết nối lại
            reconnectAttempts = MAX_RECONNECT_ATTEMPTS; 
            ws?.Close(1000, "Normal closure");
        }

        public static bool IsConnected => ws?.IsAlive == true;
    }
}
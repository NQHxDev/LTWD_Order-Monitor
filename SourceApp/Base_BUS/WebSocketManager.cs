using System;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;

using WebSocketClient = WebSocketSharp.WebSocket;

namespace Base_BUS
{
    public class WebSocketManager
    {
        private static WebSocketManager _instance;

        public static WebSocketManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WebSocketManager();
                return _instance;
            }
        }

        private WebSocketManager() { }

        private WebSocketClient ws;
        private string currentUrl;
        private Timer reconnectTimer;
        private int reconnectAttempts = 0;
        private const int MAX_RECONNECT_ATTEMPTS = 10;
        private const int RECONNECT_INTERVAL_MS = 3000;

        public event Action<string> OnMessageReceived;
        public event Action<bool> OnConnectionStatusChanged;

        public bool IsConnected => ws?.IsAlive == true;

        public void Connect(string url = "ws://localhost:8081")
        {
            if (ws != null && ws.IsAlive) return;

            currentUrl = url;
            CreateWebSocket();
            ws.Connect();
        }

        public void SendOrderStatus(int orderId, string status, string reason = "")
        {
            try
            {
                var data = new
                {
                    type = "updateOrderStatus",
                    payload = new
                    {
                        orderId = orderId,
                        status = status,
                        reason = reason
                    }
                };

                string json = JsonConvert.SerializeObject(data);
                Send(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendOrderStatus error: {ex.Message}");
            }
        }

        private void CreateWebSocket()
        {
            if (ws != null)
            {
                ws.OnOpen -= Ws_OnOpen;
                ws.OnMessage -= Ws_OnMessage;
                ws.OnError -= Ws_OnError;
                ws.OnClose -= Ws_OnClose;
                ws = null;
            }

            ws = new WebSocketClient(currentUrl);
            ws.OnOpen += Ws_OnOpen;
            ws.OnMessage += Ws_OnMessage;
            ws.OnError += Ws_OnError;
            ws.OnClose += Ws_OnClose;
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket Connected");
            reconnectAttempts = 0;
            StopReconnectTimer();
            OnConnectionStatusChanged?.Invoke(true);
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            OnMessageReceived?.Invoke(e.Data);
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($"Closed - Code: {e.Code}, Reason: {e.Reason}");
            OnConnectionStatusChanged?.Invoke(false);

            if (e.Code != 1000)
            {
                Reconnect();
            }
        }

        private void Reconnect()
        {
            if (reconnectAttempts >= MAX_RECONNECT_ATTEMPTS)
            {
                Console.WriteLine("Không thể kết nối lại Hệ thông!");
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
                    Reconnect();
                }
            }, null, RECONNECT_INTERVAL_MS, Timeout.Infinite);
        }

        private void StopReconnectTimer()
        {
            reconnectTimer?.Dispose();
            reconnectTimer = null;
        }

        public void Send(string msg)
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

        public void Close()
        {
            StopReconnectTimer();
            reconnectAttempts = MAX_RECONNECT_ATTEMPTS;
            ws?.Close(1000, "Normal closure");
        }
    }
}

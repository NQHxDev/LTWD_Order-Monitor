import { WebSocketServer } from 'ws';

let wss;
let clients = new Set();

export const initWebSocket = () => {
   // Port Websocket 8081
   wss = new WebSocketServer({ port: 8081 });

   wss.on('connection', (ws, req) => {
      clients.add(ws);
      console.log('{/} NQH Dev: Client Connected');

      // Khi nhận được message từ client
      ws.on('message', (message) => {
         try {
            const data = JSON.parse(message.toString());
            console.log('[LOG] Parsed JSON:', data);

            if (data.type === 'updateOrderStatus') {
               const { orderId, status, reason } = data.payload;
               console.log(
                  `[LOG] Order #${orderId} updated -> ${status}${
                     reason ? ` (${reason})` : ''
                  }`
               );

               // Gửi cho các client khác
               const broadcastData = JSON.stringify({
                  type: 'orderStatusUpdate',
                  payload: { orderId, status, reason },
               });

               for (let client of clients) {
                  if (client.readyState === 1 && client !== ws) {
                     client.send(broadcastData);
                  }
               }
            } else {
               ws.send('Echo: ' + message.toString());
            }
         } catch (err) {
            console.error('[ERROR] Failed to parse message:', err.message);
         }
      });

      // Khi client ngắt kết nối
      ws.on('close', () => {
         clients.delete(ws);
         console.log('{/} NQH Dev: Client Disconnected');
      });
   });
};

export const broadcastOrder = (orderData) => {
   const data = JSON.stringify({ type: 'orderFood', payload: orderData });
   for (let client of clients) {
      if (client.readyState === 1) {
         client.send(data);
      }
   }
};

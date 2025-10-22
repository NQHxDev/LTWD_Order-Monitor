import { WebSocketServer } from 'ws';
import dotenv from 'dotenv';

let wss;
dotenv.config();
let clients = new Set();

export const initWebSocket = () => {
   wss = new WebSocketServer({ port: process.env.PORT_WS });

   wss.on('connection', (ws, req) => {
      clients.add(ws);
      console.log('{/} NQH Dev: Client Connected');

      ws.on('message', (message) => {
         try {
            const data = JSON.parse(message.toString());

            // Khi application gửi cập nhật trạng thái đơn
            if (data.type === 'updateOrderStatus') {
               const { orderId, status, reason } = data.payload;

               // Gửi lại cho các client khác
               const broadcastData = JSON.stringify({
                  type: 'orderStatusUpdate',
                  payload: {
                     orderId,
                     status,
                     reason,
                     time: new Date().toISOString(),
                  },
               });

               for (let client of clients) {
                  if (client.readyState === 1 && client !== ws) {
                     client.send(broadcastData);
                  }
               }

               console.log(`>> Broadcast order #${orderId} → ${status}`);
            } else if (data.type === 'orderFood') {
               const broadcastData = JSON.stringify({
                  type: 'orderFood',
                  payload: data.payload,
               });
               for (let client of clients) {
                  if (client.readyState === 1 && client !== ws) {
                     client.send(broadcastData);
                  }
               }
            }
         } catch (err) {
            console.error('❌ Failed to parse message:', err.message);
         }
      });

      ws.on('close', () => {
         clients.delete(ws);
         console.log('{/} NQH Dev: Client Disconnected');
      });
   });

   console.log(`🌐 WebSocket listening on port ${process.env.PORT_WS}`);
};

export const broadcastOrder = (orderData) => {
   const data = JSON.stringify({ type: 'orderFood', payload: orderData });
   for (let client of clients) {
      if (client.readyState === 1) {
         client.send(data);
      }
   }
};

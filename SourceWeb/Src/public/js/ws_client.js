if (!window.globalWS) {
   const ws = new WebSocket('ws://localhost:8081');
   window.globalWS = ws;

   ws.onopen = () => console.log('[WS] Connected');
   ws.onclose = () => console.log('[WS] Disconnected');

   ws.onmessage = (event) => {
      try {
         const data = JSON.parse(event.data);
         if (data.type === 'orderStatusUpdate') {
            const { orderId, status, reason } = data.payload;
            const orders = JSON.parse(localStorage.getItem('orders')) || [];
            const order = orders.find(
               (o) => o && (o.orderId === orderId || o.id === orderId)
            );

            if (order) {
               order.status = status;
               if (reason) order.reason = reason;
               order.updated_at = new Date().toISOString();
               localStorage.setItem('orders', JSON.stringify(orders));

               console.log(
                  `[WS] Order #${orderId} updated → ${status}${
                     reason ? ' (' + reason + ')' : ''
                  }`
               );

               // 🔔 Gọi toast nếu có
               if (typeof window.showToast === 'function') {
                  window.showToast(
                     `Đơn hàng #${orderId} cập nhật: ${convertStatus(status)}`,
                     'success'
                  );
               }

               if (typeof window.renderOrders === 'function') {
                  window.renderOrders();
               }
            }
         }
      } catch (err) {
         console.error('[WS] Parse error:', err);
      }
   };
}

function convertStatus(status) {
   switch (status) {
      case 0:
         return 'Chờ xác nhận';
      case 'accepted':
         return 'Đang chế biến';
      case 'completed':
         return 'Hoàn thành';
      case 'cancelled':
         return 'Đã hủy';
      default:
         return 'Không xác định';
   }
}

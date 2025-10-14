const orderListEl = document.getElementById('orderList');
let orders = JSON.parse(localStorage.getItem('orders')) || [];

// Render danh sách Order
function renderOrders() {
   orderListEl.innerHTML = '';

   if (!orders || orders.length === 0) {
      orderListEl.innerHTML = `
         <p style="color:#aaa;text-align:center;margin-top:40px;">
            Bạn chưa có đơn hàng nào. <br>
            <a href="/" style="color:#007bff;text-decoration:none;">Quay lại menu</a>
         </p>`;
      return;
   }

   // Duyệt từng đơn hàng
   orders.forEach((order, index) => {
      if (!order) return;

      const orderId = order.orderId || order.id || index + 1;
      const totalFormatted = (order.total_price || 0).toLocaleString('vi-VN');
      const statusText = convertStatus(order.status);
      const statusClass = `status-${order.status}`;

      console.log('Cart data:', order.cart);

      // Danh sách món ăn
      const cartHTML = Array.isArray(order.cart)
         ? order.cart
              .map((item) => {
                 const foodItem = listFood.find((food) => food.id === item.id);
                 const itemName = foodItem
                    ? foodItem.name
                    : `Món ID: ${item.id}`;
                 return `
                     <div class="cart-item">
                        <span>${itemName}</span>
                        <span>x${item.quantity}</span>
                        <span>${(item.price * item.quantity).toLocaleString(
                           'vi-VN'
                        )}.000 VNĐ</span>
                     </div>`;
              })
              .join('')
         : '<em>Không có món ăn</em>';

      // Tạo phần tử đơn hàng
      const div = document.createElement('div');
      div.classList.add('order-item');
      div.innerHTML = `
         <div class="order-header">
            <h3>Đơn hàng #${orderId}</h3>
            <span class="order-status ${statusClass}">${statusText}</span>
         </div>
         <div class="order-info">
            <p><strong>Khách hàng:</strong> ${order.customer_name || 'N/A'}</p>
            <p><strong>SĐT:</strong> ${order.customer_phone || 'N/A'}</p>
            <p><strong>Ghi chú:</strong> ${order.note || 'Không có'}</p>
         </div>
         <div class="order-cart">
            <h4>Danh sách món</h4>
            ${cartHTML}
         </div>
         <div class="order-footer">
            <strong>Tổng tiền:</strong> ${totalFormatted}đ
            <span class="order-date">${formatDate(order.created_at)}</span>
         </div>
      `;

      orderListEl.appendChild(div);
   });
}

// Hàm chuyển trạng thái Order
function convertStatus(status) {
   switch (status) {
      case 0:
         return 'Chờ xác nhận';
      case 'accepted':
         return 'Đang chế biến';
      case 2:
         return 'Hoàn thành';
      case 'cancelled':
         return 'Đã hủy';
      default:
         return 'Không xác định';
   }
}

// Format thời gian
function formatDate(dateStr) {
   if (!dateStr) return '';
   const date = new Date(dateStr);
   return date.toLocaleString('vi-VN', {
      hour: '2-digit',
      minute: '2-digit',
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
   });
}

// Gọi render khi load trang
renderOrders();

// Kết nối WebSocket
const ws = new WebSocket('ws://localhost:8081');

ws.onopen = () => console.log('[WS] Connected to server');
ws.onclose = () => console.log('[WS] Disconnected');

ws.onmessage = (event) => {
   try {
      const data = JSON.parse(event.data);
      if (data.type === 'orderStatusUpdate') {
         const { orderId, status, reason } = data.payload;
         const order = orders.find(
            (o) => o && (o.orderId === orderId || o.id === orderId)
         );
         if (order) {
            order.status = status;
            if (reason) order.reason = reason;
            localStorage.setItem('orders', JSON.stringify(orders));
            renderOrders();
         }
      }
   } catch (err) {
      console.error('[WS] Parse error:', err);
   }
};

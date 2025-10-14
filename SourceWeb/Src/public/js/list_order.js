const orderListEl = document.getElementById('orderList');
let orders = JSON.parse(localStorage.getItem('orders')) || [];

// Hàm render danh sách đơn hàng
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

   // Sắp xếp đơn hàng
   orders.sort((a, b) => {
      const isACompletedOrCancelled =
         a.status === 'completed' || a.status === 'cancelled';
      const isBCompletedOrCancelled =
         b.status === 'completed' || b.status === 'cancelled';

      // Ưu tiên đơn chưa hoàn thành / hủy lên trước
      if (!isACompletedOrCancelled && isBCompletedOrCancelled) return -1;
      if (isACompletedOrCancelled && !isBCompletedOrCancelled) return 1;

      // Nếu cả 2 đều chưa hoàn thành/hủy → sắp theo created_at tăng dần
      if (!isACompletedOrCancelled && !isBCompletedOrCancelled) {
         return new Date(a.created_at) - new Date(b.created_at);
      }

      // Nếu cả 2 đều hoàn thành/hủy → sắp theo updated_at giảm dần
      return (
         new Date(b.updated_at || b.created_at) -
         new Date(a.updated_at || a.created_at)
      );
   });

   // Duyệt từng đơn hàng
   orders.forEach((order, index) => {
      if (!order) return;

      const orderId = order.orderId || order.id || index + 1;
      const totalFormatted = (order.total_price || 0).toLocaleString('vi-VN');
      const statusText = convertStatus(order.status);
      const statusClass = `status-${order.status}`;

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

      // Nếu bị hủy thì thêm lý do
      const cancelReasonHTML =
         order.status === 'cancelled' && order.reason
            ? `<p class="cancel-reason"><strong>Lý do hủy:</strong> ${order.reason}</p>`
            : '';

      // Tạo phần tử đơn hàng
      const div = document.createElement('div');
      div.classList.add('order-item');
      div.innerHTML = `
         <div class="order-header">
            <h3>Đơn hàng #${orderId}</h3>
            <span class="order-status ${statusClass}">${statusText}</span>
         </div>

         <div class="order-info">
            <div class="order-customer">
               <p><strong>Khách hàng:</strong> ${
                  order.customer_name || 'N/A'
               }</p>
               <p><strong>SĐT:</strong> ${order.customer_phone || 'N/A'}</p>
            </div>

            <div class="order-notes">
               <p><strong>Ghi chú:</strong> ${order.note || 'Không có'}</p>
               ${
                  order.status === 'cancelled' && order.reason
                     ? `<p class="cancel-reason"><strong>Lý do hủy:</strong> ${order.reason}</p>`
                     : ''
               }
            </div>
         </div>

         <div class="order-cart">
            <h4>Danh sách món</h4>
            ${cartHTML}
         </div>

         <div class="order-footer">
            <strong>Tổng tiền:</strong> ${totalFormatted}đ
            <span class="order-date">${formatDate(
               order.updated_at || order.created_at
            )}</span>
         </div>
      `;

      orderListEl.appendChild(div);
   });
}

// Chuyển đổi trạng thái sang text
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

// Định dạng ngày giờ
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

// WebSocket nhận trạng thái mới
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
            order.updated_at = new Date().toISOString();
            localStorage.setItem('orders', JSON.stringify(orders));
            renderOrders();
         }
      }
   } catch (err) {
      console.error('[WS] Parse error:', err);
   }
};

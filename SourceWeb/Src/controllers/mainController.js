import connect from '../configs/connectDB.js';
import { broadcastOrder } from '../middlewares/wsServer.js';
import { setFoodCache, getFoodCache } from '../controllers/handleData.js';

//! GET: Trang chủ
export const indexPage = (req, res) => {
   res.render('index');
};

//! GET: Trang danh sách các Order
export const listOrderPage = (req, res) => {
   res.render('list_order', {
      listFood: getFoodCache(),
   });
};

//! GET: Lấy danh sách món ăn
export const getListFood = async (req, res) => {
   try {
      let foods = getFoodCache();

      if (!foods) {
         foods = await connect.executeQuery(`
            SELECT 
               food_id AS id,
               name,
               status,
               price,
               created_at
            FROM food
         `);
         setFoodCache(foods);
      }

      res.json({
         success: true,
         data: foods,
      });
   } catch (error) {
      console.error('Lỗi tải danh sách món ăn:', error);
      res.status(500).json({
         success: false,
         data: [],
         message: 'Lỗi tải danh sách món ăn',
      });
   }
};

//! POST: Tạo đơn hàng mới
export const postListFood = async (req, res) => {
   let pool;
   let transaction;

   try {
      const { customer_name, customer_phone, note, total_price, cart } =
         req.body;

      if (!customer_name) {
         return res
            .status(400)
            .json({ message: 'Vui lòng nhập tên khách hàng' });
      }

      if (!cart || !Array.isArray(cart) || cart.length === 0) {
         return res.status(400).json({ message: 'Giỏ hàng trống' });
      }

      // Kết nối DB và bắt đầu transaction
      pool = await connect.createPool();
      transaction = new connect.sql.Transaction(pool);
      await transaction.begin();

      // Tạo đơn hàng mới
      const queryOrder = `
         INSERT INTO list_order (customer_name, note, total_price, status, customer_phone)
         OUTPUT INSERTED.oder_id
         VALUES (@customer_name, @note, @total_price, @status, @customer_phone)
      `;

      const requestOrder = new connect.sql.Request(transaction);
      requestOrder.input('customer_name', connect.sql.NVarChar, customer_name);
      requestOrder.input('note', connect.sql.NVarChar, note || '');
      requestOrder.input(
         'total_price',
         connect.sql.Decimal(12, 2),
         total_price || 0
      );
      requestOrder.input('status', connect.sql.SmallInt, 0);
      requestOrder.input(
         'customer_phone',
         connect.sql.NVarChar,
         customer_phone || ''
      );

      const resultOrder = await requestOrder.query(queryOrder);
      const orderId = resultOrder.recordset[0].oder_id;

      // Thêm chi tiết món ăn
      for (const item of cart) {
         const queryDetail = `
            INSERT INTO order_detail (order_id, food_id, quantity, price)
            VALUES (@order_id, @food_id, @quantity, @price)
         `;

         const requestDetail = new connect.sql.Request(transaction);
         requestDetail.input('order_id', connect.sql.Int, orderId);
         requestDetail.input('food_id', connect.sql.Int, item.id);
         requestDetail.input('quantity', connect.sql.Int, item.quantity);
         requestDetail.input(
            'price',
            connect.sql.Decimal(10, 2),
            item.price || 0
         );

         await requestDetail.query(queryDetail);
      }

      await transaction.commit();

      // Gửi qua WebSocket cho app realtime
      broadcastOrder({
         orderId,
         customer_name,
         customer_phone,
         note,
         total_price,
         cart,
         status: 0,
         created_at: new Date().toISOString(),
      });

      // Trả phản hồi về cho frontend
      res.status(201).json({
         success: true,
         orderId,
         orderData: {
            id: orderId,
            customer_name,
            customer_phone,
            note,
            total_price,
            status: 0,
            created_at: new Date().toISOString(),
            cart,
         },
      });
   } catch (error) {
      if (transaction) {
         try {
            await transaction.rollback();
         } catch (rollbackError) {
            console.error('Lỗi khi rollback:', rollbackError);
         }
      }

      console.error('Lỗi khi tạo đơn:', error);
      res.status(500).json({ message: 'Có lỗi xảy ra khi lưu đơn hàng' });
   } finally {
      if (pool) {
         pool.close();
      }
   }
};

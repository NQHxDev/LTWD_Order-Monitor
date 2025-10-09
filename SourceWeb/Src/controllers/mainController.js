const connect = require("../configs/connectDB");
const { broadcastOrder } = require("../middlewares/wsServer");

//! GET: Trang chủ
const indexPage = (req, res) => {
  res.render("index");
};

//! GET: Lấy danh sách món ăn
const getListFood = async (req, res) => {
  try {
    const result = await connect.executeQuery(`
      SELECT 
        food_id AS id, 
        name, 
        status, 
        price, 
        created_at
      FROM food
      ORDER BY 
        CASE status
          WHEN 3 THEN 1
          WHEN 4 THEN 2
          WHEN 1 THEN 3
          WHEN 2 THEN 4
          WHEN 0 THEN 5
        END,
        created_at DESC;
    `);

    res.json({
      success: true,
      data: result,
    });
  } catch (error) {
    console.error("❌ Lỗi tải danh sách món ăn:", error);
    res.status(500).json({
      success: false,
      data: [],
      message: "Lỗi tải danh sách món ăn",
    });
  }
};

//! POST: Tạo đơn hàng mới
const postListFood = async (req, res) => {
  const pool = await connect.createPool();
  const transaction = new connect.sql.Transaction(pool);

  try {
    const { customer_name, customer_phone, note, total_price, cart } = req.body;

    if (!customer_name) {
      return res.status(400).json({ message: "Vui lòng nhập tên khách hàng" });
    }

    if (!cart || !Array.isArray(cart) || cart.length === 0) {
      return res.status(400).json({ message: "Giỏ hàng trống" });
    }

    await transaction.begin();

    // Tạo đơn hàng mới
    const queryOrder = `
      INSERT INTO list_order (customer_name, note, total_price, status)
      OUTPUT INSERTED.oder_id
      VALUES (@customer_name, @note, @total_price, @status)
    `;

    const requestOrder = new connect.sql.Request(transaction);
    requestOrder.input("customer_name", connect.sql.NVarChar, customer_name);
    requestOrder.input("note", connect.sql.NVarChar, note || "");
    requestOrder.input(
      "total_price",
      connect.sql.Decimal(12, 2),
      total_price || 0
    );
    requestOrder.input("status", connect.sql.SmallInt, 0);

    const resultOrder = await requestOrder.query(queryOrder);
    const orderId = resultOrder.recordset[0].oder_id;

    // Thêm chi tiết món ăn
    for (const item of cart) {
      const queryDetail = `
        INSERT INTO order_detail (order_id, food_id, quantity, price)
        VALUES (@order_id, @food_id, @quantity, @price)
      `;

      const requestDetail = new connect.sql.Request(transaction);
      requestDetail.input("order_id", connect.sql.Int, orderId);
      requestDetail.input("food_id", connect.sql.Int, item.id);
      requestDetail.input("quantity", connect.sql.Int, item.quantity);
      requestDetail.input("price", connect.sql.Decimal(10, 2), item.price || 0);

      await requestDetail.query(queryDetail);
    }

    await transaction.commit();

    // Send Data to WebSocket Application
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

    res.status(201).json({
      message: "Tạo đơn hàng thành công",
      orderId,
    });
  } catch (error) {
    try {
      await transaction.rollback();
    } catch (rollbackError) {
      console.error("Lỗi khi rollback:", rollbackError);
    }

    console.error("Lỗi khi tạo đơn:", error);
    res.status(500).json({ message: "Có lỗi xảy ra khi lưu đơn hàng" });
  }
};

module.exports = {
  indexPage,
  getListFood,
  postListFood,
};

require("dotenv").config();
const sql = require("mssql");

const dbConfig = {
  user: process.env.DB_USER || "dragondev",
  password: process.env.DB_PASS || "582005",
  server: process.env.HOST_DB || "NQHXMN",
  database: process.env.NAME_DB || "ltwd_order_monitor",
  options: {
    encrypt: false,
    trustServerCertificate: true,
  },
  pool: {
    max: 10,
    min: 0,
    idleTimeoutMillis: 30000,
  },
};

let connectionPool = null;

async function createPool() {
  try {
    if (connectionPool) {
      await connectionPool.close().catch(() => {});
    }
    connectionPool = await new sql.ConnectionPool(dbConfig).connect();
    return connectionPool;
  } catch (err) {
    console.error("Lỗi kết nối SQL Server:", err.message || err);
    throw err;
  }
}

async function executeQuery(query, params = {}) {
  try {
    if (!connectionPool || !connectionPool.connected) {
      await createPool();
    }

    const request = connectionPool.request();

    for (const [key, value] of Object.entries(params)) {
      if (typeof value === "number") request.input(key, sql.Int, value);
      else request.input(key, sql.NVarChar, value);
    }

    const result = await request.query(query);
    return result.recordset;
  } catch (error) {
    console.error("Lỗi khi thực thi query:", error.message || error);
    if (
      ["ESOCKET", "ECONNRESET"].includes(error.code) ||
      error.message?.includes("Connection lost")
    ) {
      console.warn("Mất kết nối, đang thử kết nối lại...");
      await createPool();
      return await executeQuery(query, params);
    }

    throw error;
  }
}

process.on("SIGINT", async () => {
  if (connectionPool) {
    await connectionPool.close();
  }
  process.exit(0);
});

module.exports = { executeQuery, createPool, sql };

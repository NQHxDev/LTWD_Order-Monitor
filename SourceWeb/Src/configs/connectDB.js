import writeLogServer from '../utils/serverLog.js';

import dotenv from 'dotenv';
import SQL from 'mssql';

dotenv.config();

const dbConfig = {
   user: process.env.DB_USER || 'dragondev',
   password: process.env.DB_PASS || '582005',
   server: process.env.HOST_DB || 'NQHXMN',
   database: process.env.NAME_DB || 'ltwd_order_monitor',
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

const createPool = async () => {
   try {
      if (connectionPool) {
         await connectionPool.close().catch(() => {});
      }
      connectionPool = await new SQL.ConnectionPool(dbConfig).connect();
      return connectionPool;
   } catch (err) {
      console.error('Lỗi kết nối SQL Server:', err.message || err);
      writeLogServer(`Lỗi kết nối SQL Server: ${err.message || err}`, 'ERROR');
      throw err;
   }
};

const executeQuery = async (query, params = {}) => {
   try {
      if (!connectionPool || !connectionPool.connected) {
         await createPool();
      }

      const request = connectionPool.request();

      for (const [key, value] of Object.entries(params)) {
         if (typeof value === 'number') request.input(key, SQL.Int, value);
         else request.input(key, SQL.NVarChar, value);
      }

      const result = await request.query(query);
      return result.recordset;
   } catch (error) {
      console.error('Lỗi khi thực thi query:', error.message || error);
      writeLogServer(
         `Lỗi khi thực thi query: ${error.message || error}`,
         'ERROR'
      );
      if (
         ['ESOCKET', 'ECONNRESET'].includes(error.code) ||
         error.message?.includes('Connection lost')
      ) {
         console.warn('Mất kết nối, đang thử kết nối lại...');
         writeLogServer(`Mất kết nối, đang thử kết nối lại...`, 'WARN');
         await createPool();
         return await executeQuery(query, params);
      }

      throw error;
   }
};

const getTransaction = async (pool) => {
   const transaction = new SQL.Transaction(pool);
   return transaction;
};

export { executeQuery, createPool, getTransaction, SQL };

export default {
   executeQuery,
   createPool,
   getTransaction,
   sql: SQL,
};

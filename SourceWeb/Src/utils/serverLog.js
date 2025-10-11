import path from 'path';
import { fileURLToPath } from 'url';
import fs from 'fs';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

async function writeLogServer(message, level = 'INFO') {
   const timestamp = new Date().toISOString();
   const spaceLine = '--------------------------------------\n';
   const logLine = `[------------ ${timestamp}] ------------\n[${level}] ${message}\n${spaceLine}`;
   const logFilePath = path.join(__dirname, '../../logServer.log');

   try {
      await fs.promises.appendFile(logFilePath, logLine, 'utf8');
   } catch (error) {
      console.error('Failed to write to log file:', error);
   }
}

export default writeLogServer;

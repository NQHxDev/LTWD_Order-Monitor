import dotenv from 'dotenv';
import express from 'express';
import http from 'http';

const app = express();
dotenv.config();

import webRouter from './router/mainRouter.js';
import configEngine from './configs/viewEngine.js';
import { initWebSocket } from './middlewares/wsServer.js';

const server = http.createServer(app);
const port = process.env.PORT_SV;
const hostname = process.env.HOST_SV;

initWebSocket(server);
configEngine(app);
app.use('/', webRouter);

server.listen(port, hostname, () => {
   console.log(`Server run on: http://${hostname}:${port}`);
});

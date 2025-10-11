import { fileURLToPath } from 'url';
import express from 'express';
import path from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const configEngine = (app) => {
   // Config Engine
   app.set('views', path.join(__dirname, '../views'));
   app.set('view engine', 'ejs');
   // Static File
   app.use(express.static(path.join(__dirname, '../public')));
   // Config Request Body
   app.use(express.json());
   app.use(express.urlencoded({ extended: true }));
};

export default configEngine;

import express from 'express';

import {
   indexPage,
   getListFood,
   postListFood,
} from '../controllers/mainController.js';

const router = express.Router();

router.get('/', indexPage);

router.get('/api/get-list-food', getListFood);

router.post('/api/post-orders-food', postListFood);

export default router;

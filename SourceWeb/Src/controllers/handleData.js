import connect from '../configs/connectDB.js';

let foodCache = [];

export const setFoodCache = (foods) => {
   foodCache = foods;
};

export const getFoodCache = () => {
   return foodCache;
};

export const loadFoodCache = async () => {
   try {
      const foods = await connect.executeQuery(`
         SELECT 
            food_id AS id,
            name,
            status,
            price,
            created_at
         FROM food
      `);
      setFoodCache(foods);
      console.log(`{/} Successfully food loaded: ${foods.length}`);
   } catch (error) {
      console.error('{--} Lỗi tải danh sách món ăn:', error);
   }
};

// src/services/categoryService.js
import axios from 'axios';

// Access the API URL from the environment variable
const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Categories`;

export const fetchCategories = async () => {
  try {
    const response = await axios.get(`${API_URL}/Summary`);
    return response.data;
  } catch (error) {
    console.error('Error fetching categories:', error);
    throw error;
  }
};

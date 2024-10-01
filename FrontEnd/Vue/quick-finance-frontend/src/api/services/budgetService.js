import axios from "axios";

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Budgets`

export const fetchBudgets = async() => {
    try {
        const response = await axios.get(`${API_URL}/Summary`)
        return response.data;
    } catch (error) {
        console.error('Error fetching categories:', error);
        throw error;
    }
};
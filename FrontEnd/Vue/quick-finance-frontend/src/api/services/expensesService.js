import axios from 'axios';

// Access the API URL from the environment variable
const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Categories`;

export async function fetchExpenses(budgetId) {
    const response = await axios.get(`${API_URL}/api/Expenses?budgetId=${budgetId}`);
    return response.data;
}

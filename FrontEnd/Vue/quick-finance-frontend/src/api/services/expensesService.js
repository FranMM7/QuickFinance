import axios from 'axios';

// Access the API URL from the environment variable
const API_URL = import.meta.env.VITE_API_BASE_URL;

export async function fetchExpenses(budgetId) {
    if (!budgetId) {
        throw new Error('Budget ID is required');
    }
    const response = await axios.get(`${API_URL}/Expenses/Summary/${budgetId}`);
    return response.data;
}

import axios from "axios";
const API_URL = `${import.meta.env.VITE_API_BASE_URL}`
const apiClient = axios.create({
    baseURL: API_URL, // Adjust this to match your backend URL
    headers: {
        "Content-Type": "application/json",
    },
});

export default apiClient;

import axios from "axios";
import { Console, error } from "console";
import { promises } from "dns";


const API_URL = `${import.meta.env.VITE_API_BASE_URL}/PaymentMethods`

export interface PaymentMethod {
    id: number;
    name: string;
    createdOn?: Date,
    updatedOn?: Date
}

export const fetchPaymentMethods = async (): Promise<PaymentMethod[]> => {
    try {
        const URL = `${API_URL}`;
        const response = await axios.get(URL);

        return response.data; 
    } catch (error) {
        console.log('Failed to fetch payment methods:', error);
        throw error;
    }
};

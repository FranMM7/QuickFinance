import axios from "axios";
import { Console, error } from "console";
import { promises } from "dns";


const API_URL = `${import.meta.env.VITE_API_BASE_URL}/PaymentMethods`

export interface PaymentMethod {
    id: number;
    paymentMethodName: string;
    createdOn?: Date;
    updatedOn?: Date;
    state:number;
}

export const fetchPaymentMethods = async (): Promise<PaymentMethod[]> => {
    try {
        const URL = `${API_URL}`;
        const response = await axios.get(URL);

        // Extract the $values array from the response
        return response.data.$values as PaymentMethod[]; // Ensure the response is of type PaymentMethod[]
    } catch (error) {
        console.error('Failed to fetch payment methods:', error);
        throw error;
    }
};


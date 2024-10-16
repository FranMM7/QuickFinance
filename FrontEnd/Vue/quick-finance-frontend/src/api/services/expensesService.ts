import axios from 'axios';
import { error } from 'console';
import exp from 'constants';
import { promises } from 'dns';

// Access the API URL from the environment variable
const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Expenses`

export interface Expenses {
    id: number;
    description: string;
    amount: number;
    expenseDueDate: Date;
    categoryId: number;
    budgetId: number;
    paymentMethodId: number;
    isExecuted: boolean;
    createdOn: Date;
    updatedOn?: Date | null;
}

export interface ExpensesDTO {
    id: number;
    description: string;
    amount: number;
    expenseDueDate?: Date | null;
    categoryId: number;
    budgetId: number;
    paymentMethodId: number;
    isExecuted: boolean;
    createdOn: Date;
    updatedOn?: Date | null;
}


//get the expenses list
export const fecthExpenses = async (budgetId: number, PageNumber: number = 1, RowsPage: number = 10): Promise<Expenses[]> => {
    try {
        if (!budgetId) {
            throw new Error('Budget ID is required');
        }

        if (!PageNumber)
            PageNumber = 1;
        const response = await axios.get(`${API_URL}/list/${budgetId}?PageNumber=${PageNumber}&RowsPage=${RowsPage}`);
        return response.data;
    } catch (error) {
        console.error('Failed to fetch expenses')
        throw error;
    }
};

//return the expenses record
export const getExpense = async (expenseId: number): Promise<Expenses> => {
    try {
        if (!expenseId)
            throw new Error('Expense ID is required');

        const response = await axios.get(`${API_URL}/${expenseId}`);
        return response.data;
    } catch (error) {
        console.log('Failed to retrieve expenses', error);
        throw error;
    }
};


//add new record
export const addExpense = async (expense: Expenses): Promise<Expenses> => {
    try {
        const response = await axios.post(API_URL, expense);
        return response.data;
    } catch (error) {
        console.log('Fail to add expense');
        throw error;
    }
};


//update record
export const editExpense = async (expenseId: number, expense: Expenses): Promise<Expenses> => {
    try {
        if (!expenseId)
            throw new Error('Expense ID is required');

        const response = await axios.put(`${API_URL}/${expense}`, expense)
        return response.data;
    } catch (error) {
        console.log('Fail to edit expense');
        throw error;
    }
};

//delete record
export const deleteExpense = async (expenseId: number): Promise<number> => {
    try {
        if (!expenseId)
            throw new Error('Expense ID is missing');

        const response = await axios.delete(`${API_URL}/${expenseId}`);
        return response.status;
    } catch (error) {
        console.log('Fail to delete expense');
        throw error;
    }
};
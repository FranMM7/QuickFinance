import axios from "axios";
import { error } from "console";
import { promises } from "dns";

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Budgets`

export interface Budget {
    id: number;
    createadOn?: Date;
    updatedOn?: Date;
    title: string;
    totalBudget: number;
}

export interface BudgetSumary {
    BudgetId:number,
    Title: string;
    TotalBudget: number;
    Expenses: number;
    Saving: number;
  }
export interface BudgetInfo {
    BudgetTop5: BudgetSumary[];
    MonthWithHighestExpenses: BudgetSumary[];
}

//return the list of budgets summary
export const fetchBudgets = async (PageNumber: number): Promise<Budget[]> => {
    try {
        if (!PageNumber)
            PageNumber = 1;

        const url = `${API_URL}/Summary?PageNumber=${PageNumber}`
        const response = await axios.get(url)
        return response.data;
    } catch (error) {
        console.error('Error fetching Budgets:', error);
        throw error;
    }
}

//return the budget information as json 
export async function getBudgetInfo() {
    try {
        const response = await axios.get(`${API_URL}/BudgetsInfo`)
        console.log("getBudgetInfo", response)
        return response.data;
    } catch (error) {
        console.error('Error fetching Budget Info:', error);
        throw error;
    }
}

//return the lastest budget information
export const getBudget = async (budgeId: number): Promise<Budget> => {
    try {
        if (!budgeId) {
            throw new Error('Budget Id is required');
        }
        const response = await axios.get(`${API_URL}/${budgeId}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching Budgets:', error);
        throw error;
    }
}

//creates a budgets
export const addBudget = async (budget: Budget): Promise<Budget> => {
    try {
        if (!budget)
            throw new Error('Not object or class was receive');

        const response = await axios.post(API_URL, budget)
        return response.data;

    } catch (error) {
        console.error('Error adding Budgets:', error);
        throw error;
    }
}

//edits a record
export const editBudget = async (budgetId: number, budget: Budget): Promise<Budget> => {
    try {
        if (!budgetId)
            throw new Error('Budget Id is required');
        const response = await axios.put(`${API_URL}/${budgetId}`, budget);
        return response.data;
    } catch (error) {
        console.error('Error editing Budgets:', error);
        throw error;
    }
}

//deletes a record
export const deleteBudget = async (budgetId: number): Promise<number> => {
    try {
        if (!budgetId)
            throw new Error('Budget Id is required');

        const response = await axios.delete(`${API_URL}/${budgetId}`);
        return response.status;
    } catch (error) {
        console.error("Fail to delete budget", error);
        throw error;
    }
}
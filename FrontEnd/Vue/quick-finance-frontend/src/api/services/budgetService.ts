import axios from "axios";
import { error } from "console";
import { promises } from "dns";
import { Expenses, ExpensesDTO } from "./expensesService";

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Budgets`

export interface Budget {
    id: number;
    createadOn?: Date;
    updatedOn?: Date | null;
    title: string;
    totalAllocatedBudget: number;
    state:number;
}

export interface budgetDTO {
    id: number;
    createdOn?: Date; // Fixed typo
    updatedOn?: Date | null;
    title: string;
    totalAllocatedBudget: number;
    state: number;
    expensesDTO: ExpensesDTO[]; // Change this to an array to hold multiple expenses
}


export interface BudgetList{
    id:number;
    totalAllocatedBudget:number;
    executedBudget:number;
    title:string;
    modifiedOn:Date;
}

export interface BudgetSumary {
    BudgetId:number,
    Title: string;
    TotalAllocatedBudget: number;
    Expenses: number;
    Saving: number;
  }
export interface BudgetInfo {
    BudgetTop5: BudgetSumary[];
    MonthWithHighestExpenses: BudgetSumary[];
}

// Return the list of budgets
export const fetchBudgets = async (PageNumber: number, RowsPage: number): Promise<BudgetList[]> => {
    try {
        if (!PageNumber) PageNumber = 1;
        
        const url = `${API_URL}/List?PageNumber=${PageNumber}&RowsPage=${RowsPage}`;
        const response = await axios.get(url);
        
        console.log('fetchBudgets: ', response);

        // Check if the response has the $values array, and map it correctly
        const budgetList = response.data.$values?.map((budget: any) => ({
            id: budget.id,
            title: budget.title,
            totalAllocatedBudget: budget.totalAllocatedBudget,
            executedBudget: budget.executedBudget,
            modifiedOn: budget.modifiedOn // make sure to correctly assign modifiedOn from response
        })) || []; // Use an empty array as fallback

        return budgetList;
    } catch (error) {
        console.error('Error fetching Budgets:', error);
        throw error;
    }
};



//return the budget information as json 
export async function getBudgetInfo() {
    try {
        const response = await axios.get(`${API_URL}/BudgetsInfo`)
        // console.log("getBudgetInfo", response)
        return response.data;
    } catch (error) {
        console.error('Error fetching Budget Info:', error);
        throw error;
    }
}

//return the lastest budget information
export const getBudget = async (budgeId: number): Promise<budgetDTO> => {
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
import axios from 'axios';
import { error } from 'console';
import { promises } from 'dns';

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Categories`

export interface Category {
    id: number;
    createdOn?:Date;
    updatedOn?:Date;
    name: string;
    budgetlimit:number;
}

export const fetchCategories = async (PageNumber:number): Promise<Category[]> => {
    try {
        if (!PageNumber)
            PageNumber=1;
        
        const response = await axios.get(`${API_URL}/Summary?PageNumber=${PageNumber}`);
        return response.data; // Ensure the response type matches the expected structure
    } catch (error) {
        console.error('Failed to fetch categories:', error);
        throw error; // Rethrow the error for handling in the component
    }
};

export const getCategory = async (categoryId:number): Promise<Category> =>{
  try {
    if (!categoryId){
        throw new Error('Category ID is required');
    }
    const response = await axios.get(`${API_URL}/${categoryId}`);
    return response.data;
  } catch (error) {
    console.error('Failed to fetch categories:', error);
        throw error; // Rethrow the error for handling in the component
  } 
}

//function to create a new category
export const addCategory = async (category: Category): Promise<Category> => {
    try {
        const response = await axios.post(API_URL, category);
        return response.data; // Adjust this if the response structure is different
    } catch (error) {
        console.error('Failed to add category:', error);
        throw error; // Rethrow the error for handling in the component
    }
};

//function to edit an existing category
export const editCategory = async (categoryId: number, category: Category): Promise<Category> => {
    try {
        const response = await axios.put(`${API_URL}/${categoryId}`, category);
        return response.data; // Adjust this if the response structure is different
    } catch (error) {
        console.error('Failed to edit category:', error);
        throw error; // Rethrow the error for handling in the component
    }
};

//function to delete a category
export const deleteCategory = async (categoryId: number): Promise<number> => {
    try {
        const response = await axios.delete(`${API_URL}/${categoryId}`);        
        return response.status;
    } catch (error) {
        console.error('Failed to delete category:', error);
        throw error; // Rethrow the error for handling in the component
    }
};

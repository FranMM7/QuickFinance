import axios from 'axios'
import { PaginatedResponse } from './paginationServices'
import { Category } from './categoryService'
import { location } from './locationServices'
import { useAuthStore } from '@/stores/auth'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Shopping`

export interface Shopping {
  id: number
  description: string
  state: number
  modifiedOn?: string // Using string for ISO date format
  grandTotal: number
}

export interface ShoppingList {
  id: number
  itemName: string
  brand: string
  quantity: number
  amount: number
  subTotal: number
  categoryId: number
  category?: string | null
  locationId: number
  location?: string | null
}

export interface ShoppingListGroup {
  category: string
  items: {
    id: number
    itemName: string
    brand: string
    quantity: number
    amount: number
    subTotal: number
    categoryId: number
    locationId: number
    location?: string | null
  }
}

export interface ShoppingData {
  id: number
  description: string
  modifiedOn: Date
  state: number
}

export interface PaginatedShoppingResponse {
  shoppingData: ShoppingData
  data: {
    $values: ShoppingList[]
  }
  pagination: {
    currentPage: number
    rowsPerPage: number
    totalRecords: number
    totalPages: number
    firstPage: string | null
    lastPage: string | null
    nextPage: string | null
    previousPage: string | null
  }
}

export interface shoppingDataSave {
  id: number
  updatedOn: Date
  description: string
  state: number
  userId:string
  ShoppingLists: ShoppingList[]
}

export const fetchShoppingInfo = async (
  userId:string,
  pageNumber: number,
  rowsPerPage: number
): Promise<PaginatedResponse<Shopping>> => {
  try {
    if (!pageNumber) pageNumber = 1

    if (!userId) throw new Error('UserId is required')

    const url = `${API_URL}?userId=${userId}&pageNumber=${pageNumber}&rowsPerPage=${rowsPerPage}`
    const response = await axios.get(url)

    // Extracting the required data from the response
    const responseData = response.data
    const shoppingData = responseData.data.$values || [] // Ensure it's an array

    return {
      data: shoppingData, // This should now be a flat array of Shopping
      totalPages: responseData.totalPages,
      totalRecords: responseData.totalRecords,
      firstPage: responseData.firstPage,
      lastPage: responseData.lastPage,
      nextPage: responseData.nextPage,
      previousPage: responseData.previousPage
    }
  } catch (error) {
    console.error('Error fetching shopping list:', error)
    throw error
  }
}

export const goToPage = async (pageUrl: string): Promise<PaginatedResponse<Shopping>> => {
  try {
    const response = await axios.get(pageUrl)
    // Extracting the required data from the response
    const responseData = response.data
    const shoppingData = responseData.data.$values || [] // Ensure it's an array

    return {
      data: shoppingData, // This should now be a flat array of Shopping
      totalPages: responseData.totalPages,
      totalRecords: responseData.totalRecords,
      firstPage: responseData.firstPage,
      lastPage: responseData.lastPage,
      nextPage: responseData.nextPage,
      previousPage: responseData.previousPage
    }
  } catch (error) {
    console.error('Error fetching shopping list:', error)
    throw error
  }
}

//when default values are set to 0, will return all the data
export const getShoppingById = async (Id: number): Promise<PaginatedShoppingResponse> => {
  try {
    const url = `${API_URL}/List?Id=${Id}&pageNumber=0&rowsPerPage=0`
    const response = await axios.get(url)
    return response.data
  } catch (error) {
    console.error('Error fetching shopping info:', error)
    throw error
  }
}

export const getCloneShopping = async (id: number): Promise<{ id: number }> => {
  try {
    const url = `${API_URL}/Clone?id=${id}`;
    const response = await axios.get(url);
    return { id: response.data.id };  // Extract and return the id
  } catch (error) {
    console.error('Error cloning record', error);
    throw error;
  }
};


// Creates a record
export const addShopping = async (record: shoppingDataSave): Promise<shoppingDataSave> => {
  try {
    const response = await axios.post(API_URL, record)
    return response.data
  } catch (error) {
    console.error('Error adding shopping:', error)
    throw error
  }
}

// Edits a record
export const editShopping = async (
  Id: number,
  record: shoppingDataSave
): Promise<shoppingDataSave> => {
  try {
    const response = await axios.put(`${API_URL}/${Id}`, record)
    return response.data
  } catch (error) {
    console.error('Error editing shopping:', error)
    throw error
  }
}

// Deletes a record
export const deleteShopping = async (Id: number): Promise<number> => {
  try {
    const response = await axios.delete(`${API_URL}/${Id}`)
    return response.status
  } catch (error) {
    console.error('Failed to delete shopping:', error)
    throw error
  }
}

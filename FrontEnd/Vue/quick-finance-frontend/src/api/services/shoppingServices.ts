import axios from 'axios'
import { error } from 'console'
import { promises } from 'dns'
import { Category } from './categoryService'
import { location } from './locationServices'
import { PaginatedResponse } from './paginationServices'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Shopping`

export interface Shopping {
  id: number
  createdOn?: Date
  updatedOn?: Date
  description: string
  state: number
  modifiedOn?:Date
}

export interface ShoppingList {
  id: number
  createdOn?: Date
  updatedOn?: Date
  description: string
  item: string
  qty: number
  amount: number
  subTotal: number
  category: string
  location: string
}

export interface ShoppingDTO {
  id: number
  createdOn?: Date
  updatedOn?: Date
  description: string
  state: number
  shoppingList: ShoppingList
}

export interface ShoppingData {
  id: number;
  createdOn: string; // ISO date string
  updatedOn: string | null; // nullable
  description: string;
  state: number;
  shoppingLists: ShoppingLists;
}

export interface ShoppingLists {
  $values: ShoppingListItem[];
}

export interface ShoppingListItem {
  $id: string;
  id: number;
  shoppingId: number;
  categoryId: number;
  locationId: number;
  description: string;
  quantity: number;
  amount: number;
  subtotal: number;
  shopping: ShoppingReference;
  category: Category | null;
  locations: location | null;
}

export interface ShoppingReference {
  $ref: string;
}



export const fetchShoppingInfo = async (
  PageNumber: number,
  RowsPage: number
): Promise<PaginatedResponse<Shopping>> => {
  try {
    if (!PageNumber) PageNumber = 1

    const url = `${API_URL}/?PageNumber=${PageNumber}&RowsPage=${RowsPage}`
    const response = await axios.get(url)
    const list = response.data.$values?.map((records: any) => ({
      id: records.id,
      createdOn: records.createdOn,
      updatedOn: records.updatedOn,
      description: records.description,
      state: records.state,
      modifiedOn:!records.updatedOn? records.createdOn: records.updatedOn,
    }))

    return list || []
  } catch (error) {
    console.error('Error fetching shopping list:', error)
    throw error
  }
}

export const getShoppingById = async (Id: number): Promise<ShoppingData> => {
  try {
    if (!Id) throw new Error('Shopping Id is required')

    const url = `${API_URL}/${Id}`
    const response = await axios.get(url)

    return response.data || null
  } catch (error) {
    console.error('Error fetching data Shopping info', error)
    throw error
  }
}

//creates a record
export const addShopping = async (record: ShoppingDTO): Promise<ShoppingDTO> => {
  try {
    if (!record) throw new Error('Not object or class was receive')

    const response = await axios.post(API_URL, record)
    return response.data
  } catch (error) {
    console.error('Error adding Shoppings:', error)
    throw error
  }
}

//edits a record
export const editShopping = async (Id: number, record: ShoppingDTO): Promise<ShoppingDTO> => {
  try {
    if (!record) throw new Error('Shopping Id is required')
    const response = await axios.put(`${API_URL}/${Id}`, record)
    return response.data
  } catch (error) {
    console.error('Error editing Shoppings:', error)
    throw error
  }
}

//deletes a record
export const deleteShopping = async (Id: number): Promise<number> => {
  try {
    if (!Id) throw new Error('Shopping Id is required')

    const response = await axios.delete(`${API_URL}/${Id}`)
    return response.status
  } catch (error) {
    console.error('Fail to delete Shopping', error)
    throw error
  }
}

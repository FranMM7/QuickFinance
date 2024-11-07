import axios from 'axios'
import { PaginatedResponse } from './paginationServices'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Budgets`

export interface Finance {
  id: number
  createdOn?: Date
  updatedOn?: Date
  title: string
  state: number
}

export interface FinanceDetails {
  id: number
  financeId: number
  description: string
  expenseCategory: number
  amount: number
  categoryId: number
  financeEvaluation: string
}

export interface FinancePageResponse {
  id: number
  description: string
  financeDetails: FinanceDetails[]
}

export const fetchFinanceList = async (
  pageNumber: number,
  rowsPerPage: number
): Promise<PaginatedResponse<FinancePageResponse>> => {
  try {
    const url = `${API_URL}/List?pageNumber=${pageNumber}&rowsPerPage=${rowsPerPage}`
    const response = await axios.get(url)
    return {
      data: response.data.data,
      totalPages: response.data.totalPages,
      totalRecords: response.data.totalRecords,
      firstPage: response.data.firstPage,
      lastPage: response.data.lastPage,
      nextPage: response.data.nextPage,
      previousPage: response.data.previousPage
    }
  } catch (error) {
    console.error('Error fetching finance list:', error)
    throw error
  }
}

export const goToPage = async (
  pageUrl: string
): Promise<PaginatedResponse<FinancePageResponse>> => {
  try {
    const response = await axios.get(pageUrl)
    return {
      data: response.data.data,
      totalPages: response.data.totalPages,
      totalRecords: response.data.totalRecords,
      firstPage: response.data.firstPage,
      lastPage: response.data.lastPage,
      nextPage: response.data.nextPage,
      previousPage: response.data.previousPage
    }
  } catch (error) {
    console.error('Error fetching paginated finance data:', error)
    throw error
  }
}

export const fetchFinanceData = async (): Promise<Finance | undefined> => {
  try {
    const response = await axios.get(API_URL)
    return {
      id: response.data.id,
      title: response.data.title,
      createdOn: response.data.createdOn,
      updatedOn: response.data.updatedOn,
      state: response.data.state
    }
  } catch (error) {
    console.error('Error fetching finance data:', error)
  }
}

export const fetchFinanceById = async (id: number): Promise<Finance | undefined> => {
  try {
    if (!id) throw new Error('ID is required')

    const url = `${API_URL}/${id}`
    const response = await axios.get(url)
    return {
      id: response.data.id,
      title: response.data.title,
      createdOn: response.data.createdOn,
      updatedOn: response.data.updatedOn,
      state: response.data.state
    }
  } catch (error) {
    console.error(`Error fetching finance data by ID (${id}):`, error)
  }
}

export const addFinance = async (finance: FinancePageResponse): Promise<FinancePageResponse> => {
  try {
    if (!finance) throw new Error('Not object or class was receive')

    const response = await axios.post(API_URL, finance)
    return response.data
  } catch (error) {
    console.error('Error adding finance', error)
    throw error
  }
}

export const editFinance = async (id: number, finance: FinancePageResponse): Promise<Finance> => {
  try {
    if (!id) throw new Error('Id is required')

    const response = await axios.put(`${API_URL}/${id}`, finance)
    return response.data
  } catch (error) {
    console.error('Error adding finance', error)
    throw error
  }
}

export const deleteBudget = async (id: number): Promise<number> => {
  try {
    if (!id) throw new Error('Id is required')

    const response = await axios.delete(`${API_URL}/${id}`)
    return response.status
  } catch (error) {
    console.error('Fail to delete finance', error)
    throw error
  }
}

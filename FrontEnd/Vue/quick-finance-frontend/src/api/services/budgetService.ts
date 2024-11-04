import axios from 'axios'
import { error } from 'console'
import { promises } from 'dns'
import { Expenses, ExpensesDTO } from './expensesService'
import { PaginatedResponse } from './paginationServices'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Budgets`

export interface Budget {
  id: number
  createdOn?: Date
  updatedOn?: Date | null
  title: string
  totalAllocatedBudget: number
  state: number
}

export interface budgetDTO {
  id: number
  createdOn?: Date // Fixed typo
  updatedOn?: Date | null
  title: string
  totalAllocatedBudget: number
  state: number
  expensesDTO: ExpensesDTO[] // Change this to an array to hold multiple expenses
}

export interface BudgetList {
  id: number
  totalAllocatedBudget: number
  executedBudget: number
  title: string
  modifiedOn: Date
}

export interface BudgetSumary {
  BudgetId: number
  Title: string
  TotalAllocatedBudget: number
  Expenses: number
  Saving: number
}
export interface BudgetInfo {
  BudgetTop5: BudgetSumary[]
  MonthWithHighestExpenses: BudgetSumary[]
}

// Return the list of budgets
export const fetchBudgets = async (
  PageNumber: number,
  RowsPage: number
): Promise<PaginatedResponse<BudgetList>> => {
  try {
    if (!PageNumber) PageNumber = 1

    const url = `${API_URL}/List?pageNumber=${PageNumber}&rowsPerPage=${RowsPage}`
    const response = await axios.get(url)

    const budgetList =
      response.data.data.$values?.map((budget: any) => ({
        id: budget.id,
        title: budget.title,
        totalAllocatedBudget: budget.totalAllocatedBudget,
        executedBudget: budget.executedBudget,
        modifiedOn: new Date(budget.modifiedOn) // Ensure date is correctly parsed
      })) || []

    return {
      data: budgetList,
      totalPages: response.data.totalPages,
      totalRecords: response.data.totalRecords,
      firstPage: response.data.firstPage,
      lastPage: response.data.lastPage,
      nextPage: response.data.nextPage,
      previousPage: response.data.previousPage
    }
  } catch (error) {
    console.error('Error fetching Budgets:', error)
    throw error
  }
}

export const goToPage = async (pageUrl: string): Promise<PaginatedResponse<BudgetList>> => {
  try {
    if (!pageUrl) {
      throw new Error('invalid URL')
    }
    const response = await axios.get(pageUrl)
    const budgetList =
      response.data.data.$values?.map((budget: any) => ({
        id: budget.id,
        title: budget.title,
        totalAllocatedBudget: budget.totalAllocatedBudget,
        executedBudget: budget.executedBudget,
        modifiedOn: new Date(budget.modifiedOn)
      })) || []

    return {
      data: budgetList,
      totalPages: response.data.totalPages,
      totalRecords: response.data.totalRecords,
      firstPage: response.data.firstPage,
      lastPage: response.data.lastPage,
      nextPage: response.data.nextPage,
      previousPage: response.data.previousPage
    }
  } catch (error) {
    console.error('Error fetching goToPage Budgets:', error)
    throw error
  }
}

//return the budget information as json
export async function getBudgetInfo() {
  try {
    const response = await axios.get(`${API_URL}/BudgetsInfo`)
    // console.log("getBudgetInfo", response)
    return response.data
  } catch (error) {
    console.error('Error fetching Budget Info:', error)
    throw error
  }
}

// Return the latest budget information
export const getBudget = async (budgetId: number): Promise<budgetDTO> => {
  try {
    if (!budgetId) {
      throw new Error('Budget Id is required')
    }

    const response = await axios.get(`${API_URL}/${budgetId}`)

    // Extract fields individually from response.data
    const record: budgetDTO = {
      id: response.data.id,
      title: response.data.title,
      totalAllocatedBudget: response.data.totalAllocatedBudget,
      state: response.data.state,
      expensesDTO: response.data.expenses?.$values || [] // Extract expenses from $values
      // Add any other fields that you need to map here individually
    }

    return record
  } catch (error) {
    console.error('Error fetching Budget:', error)
    throw error
  }
}

//creates a budgets
export const addBudget = async (budget: Budget): Promise<Budget> => {
  try {
    if (!budget) throw new Error('Not object or class was receive')

    const response = await axios.post(API_URL, budget)
    return response.data
  } catch (error) {
    console.error('Error adding Budgets:', error)
    throw error
  }
}

//edits a record
export const editBudget = async (budgetId: number, budget: budgetDTO): Promise<Budget> => {
  try {
    if (!budgetId) throw new Error('Budget Id is required')
    const response = await axios.put(`${API_URL}/${budgetId}`, budget)
    return response.data
  } catch (error) {
    console.error('Error editing Budgets:', error)
    throw error
  }
}

//deletes a record
export const deleteBudget = async (budgetId: number): Promise<number> => {
  try {
    if (!budgetId) throw new Error('Budget Id is required')

    const response = await axios.delete(`${API_URL}/${budgetId}`)
    return response.status
  } catch (error) {
    console.error('Fail to delete budget', error)
    throw error
  }
}

import axios from 'axios'

// Access the API URL from the environment variable
const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Expenses`

export interface Expenses {
  id: number
  description: string
  amount: number
  expenseDueDate: Date
  categoryId: number
  category: string
  budgetId: number
  paymentMethodId: number
  paymentMethod: string
  executed: boolean
  modifiedOn: Date
}

export interface ExpensesDTO {
  id: number
  description: string
  amount: number
  expenseDueDate?: Date | null
  categoryId: number
  budgetId: number
  paymentMethodId: number
  isExecuted: boolean
  createdOn: Date
  updatedOn?: Date | null
}

// Get the expenses list
export const fetchExpenses = async (
  budgetId: number,
  pageNumber: number = 1,
  rowsPage: number = 10
): Promise<Expenses[]> => {
  try {
    if (!budgetId) {
      throw new Error('Budget ID is required')
    }

    const response = await axios.get(
      `${API_URL}/list/${budgetId}?PageNumber=${pageNumber}&RowsPage=${rowsPage}`
    )

    return response.data.$values || [] // Ensure this is returning the expected array format
  } catch (error) {
    console.error('Failed to fetch expenses', error)
    throw error
  }
}

// Return the expenses record
export const getExpense = async (expenseId: number): Promise<Expenses> => {
  try {
    if (!expenseId) throw new Error('Expense ID is required')

    const response = await axios.get(`${API_URL}/${expenseId}`)
    return response.data
  } catch (error) {
    console.log('Failed to retrieve expense', error)
    throw error
  }
}

// Add new record
export const addExpense = async (expense: Expenses): Promise<Expenses> => {
  try {
    const response = await axios.post(API_URL, expense)
    return response.data
  } catch (error) {
    console.log('Fail to add expense', error)
    throw error
  }
}

// Update record
export const editExpense = async (expenseId: number, expense: Expenses): Promise<Expenses> => {
  try {
    if (!expenseId) throw new Error('Expense ID is required')

    const response = await axios.put(`${API_URL}/${expenseId}`, expense) // Fix here
    return response.data
  } catch (error) {
    console.log('Fail to edit expense', error)
    throw error
  }
}

// Delete record
export const deleteExpense = async (expenseId: number): Promise<number> => {
  try {
    if (!expenseId) throw new Error('Expense ID is missing')

    const response = await axios.delete(`${API_URL}/${expenseId}`)
    return response.status
  } catch (error) {
    console.log('Fail to delete expense', error)
    throw error
  }
}

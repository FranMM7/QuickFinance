import { promises } from 'dns'
import axios from 'axios'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/General`

export const paginationInfo = async (
  RowsPage: number,
  Table: string,
  Id: number = 0
): Promise<number> => {
  try {
    const idUrl = Id == 0 ? '' : `&Id=${Id}`
    const url = `${API_URL}/TotalPages?RowsPage=${RowsPage}&tableName=${Table}${idUrl}`
    console.log('pagination url:', url)
    const response = await axios.get(url)
    return response.data
  } catch (error) {
    console.log('Unable to access Pagination info: ', error)
    throw error
  }
}

export function formatDate(dateString: string) {
  const date = new Date(dateString)
  return date.toLocaleDateString()
}

export interface GroupedData<T> {
  [key: string]: T[]
}

export function groupDataByColumns<T>(data: any, columns: (keyof T)[]): GroupedData<T> {
  // Convert object to array if needed
  const dataArray: T[] = Array.isArray(data) ? data : Object.values(data)

  // Initialize the grouping object
  const grouped: GroupedData<T> = {}

  // Iterate through each item and group by specified columns
  dataArray.forEach((item: T) => {
    // Create a group key by joining values of specified columns
    const groupKey = columns.map((column) => item[column] || 'N/A').join('-')

    if (!grouped[groupKey]) {
      grouped[groupKey] = []
    }
    grouped[groupKey].push(item)
  })

  return grouped
}

/*Example usage
type ExampleData = {
  category: string
  subCategory: string
  itemName: string
  amount: number
}

const jsonData = {
  1: { category: 'Food', subCategory: 'Dairy', itemName: 'Milk', amount: 100 },
  2: { category: 'Food', subCategory: 'Fruits', itemName: 'Apple', amount: 50 }
}

const groupedByCategory = groupDataByColumns<ExampleData>(jsonData, ['category'])
console.log(groupedByCategory)
*/

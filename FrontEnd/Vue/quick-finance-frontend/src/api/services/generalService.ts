import axios from "axios";

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/General`;

export interface Settings {
  id?: number; // Optional for POST requests where the ID might be auto-generated.
  settingsName: string;
  jsonValue: string;
  userId: string;
}

// POST /api/General/saveSettings
export const saveSettings = async (settings: Settings): Promise<number> => {
  try {
    const url = `${API_URL}/saveSettings`;
    const response = await axios.post(url, settings); // Pass the settings object in the request body.
    return response.data; // Assuming the API returns a numeric ID or similar.
  } catch (error) {
    console.error("Error while saving settings:", error);
    throw error; // Re-throw the error so the caller can handle it.
  }
};


export function formatDate(dateString: string) {
  const date = new Date(dateString)
  return date.toLocaleDateString()
}

export interface GroupedData<T> {
  [key: string]: T[]
}

export function groupDataByColumns<T>(data: T[], columns: (keyof T)[]): GroupedData<T> {
  // Initialize the grouping object
  const grouped: GroupedData<T> = {}

  // Iterate through each item and group by specified columns
  data.forEach((item: T) => {
    // Create a group key by joining values of specified columns
    const groupKey = columns.map((column) => item[column] || 'N/D').join('-')

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

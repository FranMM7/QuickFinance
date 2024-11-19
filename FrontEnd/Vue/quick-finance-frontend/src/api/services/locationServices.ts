import { useAuthStore } from '@/stores/auth'
import axios from 'axios'

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/Location`

export interface location {
  id: number
  createdOn?: Date
  updatedOn?: Date
  name: String
  state: number
}

export interface locationDTO {
  id: number
  name: String
  state: number
  modifiedOn: Date
}

export const fetchlocation = async (userId:string): Promise<location[]> => {
  try {

    if (!userId) throw new Error('UserId is required')

    const url = `${API_URL}/List?userId=${userId}`
    const response = await axios.get(url)
    const list = response.data.$values?.map((records: any) => ({
      id: records.id,
      createdOn: records.createdOn,
      updatedOn: records.updatedOn,
      name: records.name,
      state: records.state
    }))

    return list || []
  } catch (error) {
    console.error('Error fetching location list:', error)
    throw error
  }
}

export const getlocationbyId = async (Id: number): Promise<location> => {
  try {
    if (!Id) throw new Error('location Id is required')

    const url = `${API_URL}/${Id}`
    const response = await axios.get(url)

    return response.data || null
  } catch (error) {
    console.error('Error fetching data location info', error)
    throw error
  }
}

//creates a record
export const addlocation = async (record: location): Promise<location> => {
  try {
    if (!record) throw new Error('Not object or class was receive')

    const response = await axios.post(API_URL, record)
    return response.data
  } catch (error) {
    console.error('Error adding locations:', error)
    throw error
  }
}

//edits a record
export const editlocation = async (Id: number, record: location): Promise<location> => {
  try {
    if (!record) throw new Error('location Id is required')
    const response = await axios.put(`${API_URL}/${Id}`, record)
    return response.data
  } catch (error) {
    console.error('Error editing locations:', error)
    throw error
  }
}

//deletes a record
export const deletelocation = async (Id: number): Promise<number> => {
  try {
    if (!Id) throw new Error('location Id is required')

    const response = await axios.delete(`${API_URL}/${Id}`)
    return response.status
  } catch (error) {
    console.error('Fail to delete location', error)
    throw error
  }
}

// {
//     "id": 0,
//     "createdOn": "2024-10-28T19:24:42.425Z",
//     "updatedOn": "2024-10-28T19:24:42.425Z",
//     "name": "string",
//     "state": 0
//   }

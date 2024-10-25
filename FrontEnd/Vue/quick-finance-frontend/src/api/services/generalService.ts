import { promises } from "dns";
import axios from "axios";

const API_URL = `${import.meta.env.VITE_API_BASE_URL}/General`

export const paginationInfo = async (RowsPage: number, Table: string, Id:number=0): Promise<number> => {
    try {
        const idUrl = Id==0? '':`&Id=${Id}`;
        const url = `${API_URL}/TotalPages?RowsPage=${RowsPage}&tableName=${Table}${idUrl}`;
        console.log('pagination url:', url)
        const response = await axios.get(url);
        return response.data;
    } catch (error) {
        console.log("Unable to access Pagination info: ", error);
        throw error;
    }
}


export function formatDate(dateString: string) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}
export interface PaginatedResponse<T> {
  data: T[]
  totalPages: number
  totalRecords: number,
  firstPage:string, 
  lastPage:string,
  nextPage:string,
  previousPage:string
}

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        Data = data;
        Succeeded = true;
    }
}
DECLARE @PageNumber as int = 2, 
		@RowsOfPage as int = 3


SELECT *
FROM Categories
ORDER BY Id
OFFSET (@PageNumber-1)*@RowsOfPage Rows 
FETCH NEXT @RowsOfPage ROWS ONLY
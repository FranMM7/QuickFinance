DECLARE @spid INT;

DECLARE connection_cursor CURSOR FOR
SELECT spid 
FROM sys.sysprocesses
WHERE db_name(dbid) in ('QuickFinanceDB','QuickFinanceDB-Copy' );

OPEN connection_cursor;

FETCH NEXT FROM connection_cursor INTO @spid;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC('KILL ' + @spid); -- Kill each session individually
    FETCH NEXT FROM connection_cursor INTO @spid;
END

CLOSE connection_cursor;
DEALLOCATE connection_cursor;

-- Full Backup and Restore Script

-- Step 1: Backup the original QuickFinanceDB database
BACKUP DATABASE [QuickFinanceDB]
TO DISK = 'C:\Code\QuickFinanceDB.bak'
WITH FORMAT, 
     NAME = 'QuickFinance Full Backup';

-- Step 2: Restore the QuickFinanceDB database as QuickFinanceDB-Copy
RESTORE DATABASE [QuickFinanceDB-Copy]
FROM DISK = 'C:\Code\QuickFinanceDB.bak'
WITH MOVE 'QuickFinanceDB' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS01\MSSQL\DATA\QuickFinanceDB-Copy.mdf',
     MOVE 'QuickFinanceDB_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS01\MSSQL\DATA\QuickFinanceDB-Copy_log.ldf',
     REPLACE;

use QuickFinanceDB

declare @userid nvarchar(50)

select @userid = Id
from AspNetUsers
where UserName like 'fmejia%'

select *
from Locations
where userid=@userid

select *
from Categories 
where UserId=@userid
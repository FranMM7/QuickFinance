select *
from AspNetUsers
--WHERE UserName <> 'admin';

select *
from AspNetUserRoles

select *
from Settings

BEGIN TRANSACTION;

--delete settings
DELETE FROM Settings
WHERE UserId <>  '0104021d-1b1f-41f3-8be5-68d0f08f23d1'

-- First, delete roles of users that are not 'admin'
DELETE FROM AspNetUserRoles
WHERE UserId IN (
    SELECT Id
    FROM AspNetUsers
    WHERE UserName <> 'admin'
);

-- Then, delete users that are not 'admin'
DELETE FROM AspNetUsers
WHERE UserName <> 'admin';


--COMMIT TRANSACTION;

CREATE procedure [dbo].[spGetLoginusers]
@CompId varchar(3) 
as
select distinct  emp.Name  AS Id, emp.PhotographPath AS [Value]
from UserLogTime as ms
inner join Employee as emp on ms.UserId=emp.EmployeeId
--inner join [User] as u on emp.EmployeeId=u.EmployeeId
where ms.[Offline]=1  
and CAST(ms.LoginTime as DATE)= CAST(GETDATE() AS DATE)

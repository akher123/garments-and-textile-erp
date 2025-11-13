CREATE procedure spHousekeepingRegister
@employeeCardId varchar(10)
as

SELECT (SELECT  Name FROM    HouseKeepingItem WHERE (HouseKeepingItemId = HR.HouseKeepingItemId)) AS ItemName, 
HR.Quantity,
HR.Rate,
(HR.Quantity*HR.Rate )as Amt,
HR.IusseDate,
HR.Remarks, 
HR.ReturnQty, 
HR.ReturnDate,
E.Name as EmpName,
E.Designation,
E.Department,
E.EmployeeCardId as CardId
FROM  HouseKeepingRegister AS HR
INNER JOIN  VEmployee AS E ON HR.EmployeeId = E.EmployeeId
where E.EmployeeCardId=@employeeCardId


--exec spHousekeepingRegister '5372'






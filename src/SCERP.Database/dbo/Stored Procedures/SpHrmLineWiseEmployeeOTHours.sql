CREATE procedure [dbo].[SpHrmLineWiseEmployeeOTHours]
@TransactionDate date,
@DepartmentLineId int
as
select 

Employee.EmployeeCardId as EmpID,
Employee.Name AS [Emp Name],
EmployeeInOut.EmployeeDesignation as Designation,
(select GrossSalary  from VEmployee where EmployeeId=EmployeeInOut.EmployeeId) as Salary,
 Format(EmployeeInOut.TransactionDate,'dd-MM-yyyy') as [OT DATE],
 EmployeeInOut.LineName as LINE,(EmployeeInOut.OTHours+EmployeeInOut.ExtraOTHours+EmployeeInOut.HolidayOTHours+EmployeeInOut.WeekendOTHours)as OTHs,
 cast((EmployeeInOut.OTHours+EmployeeInOut.ExtraOTHours+EmployeeInOut.HolidayOTHours+EmployeeInOut.WeekendOTHours)*(select BasicSalary / 104 from VEmployee where EmployeeId=EmployeeInOut.EmployeeId)as decimal(18,2)) as OTAmounts
 from EmployeeInOut
inner join Employee on  EmployeeInOut.EmployeeId=Employee.EmployeeId
left join LineOvertimeHour  on EmployeeInOut.DepartmentLineId=LineOvertimeHour.DepartmentLineId and EmployeeInOut.TransactionDate=Convert(date,LineOvertimeHour.TransactionDate)
where EmployeeInOut.TransactionDate=Convert(date,@TransactionDate) and EmployeeInOut.DepartmentLineId=@DepartmentLineId and (EmployeeInOut.OTHours+EmployeeInOut.ExtraOTHours+EmployeeInOut.HolidayOTHours+EmployeeInOut.WeekendOTHours)>0

order by EmployeeInOut.DepartmentLineId



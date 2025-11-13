create procedure [dbo].[spGetDailyAttStatusReport]

@BranchId int ,
@BranchUnitId int ,
@BranchUnitDepartmentId int,
@FilterDate date,
@EmpSatatus varchar(50)
as


select BranchName,UnitName,DepartmentName,EmployeeName,EmployeeCardId,EmployeeType,EmployeeDesignation,EmployeeGrade,[Status] 
,InTime 
from [dbo].[EmployeeInOut]

where TransactionDate=cast(@FilterDate as Date)  

and [Status]=@EmpSatatus and  (BranchId=@BranchId or @BranchId=-1)  and (BranchUnitId=@BranchUnitId  or  @BranchUnitId=-1) and (BranchUnitDepartmentId=@BranchUnitDepartmentId or @BranchUnitDepartmentId=-1)


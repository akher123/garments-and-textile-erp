
--- Select [dbo].[fnGetEmployeeBranchUnitWorkShiftId]('6FDDD59E-5112-4D11-A805-09561B08AE7C','2015-09-20')

CREATE FUNCTION [dbo].[fnGetEmployeeBranchUnitWorkShiftId] (  
	@EmployeeId uniqueidentifier,
	@Date Date
)

RETURNS INT

AS BEGIN

    DECLARE @BranchUnitWorkShiftId INT
				
	SELECT TOP (1) @BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId 
	FROM WorkShift AS workShift							
	INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
	INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId			
	INNER JOIN Employee AS employee ON Employee.EmployeeId = employeeWorkShift.EmployeeId
	WHERE CAST(employeeWorkShift.ShiftDate AS DATE) = @Date
	AND employeeWorkShift.EmployeeId = @EmployeeId
	AND branchUnitWorkShift.IsActive = 1
	AND branchUnitWorkShift.[Status] = 1
	AND employeeWorkShift.IsActive = 1
	AND branchUnitWorkShift.[Status] = 1
	AND employee.IsActive = 1

    RETURN @BranchUnitWorkShiftId
END







--- Select [dbo].[fnGetEmployeeWorkShift]('7A87AE36-E056-4CFF-A3A7-5425AB06C7D1','2015-10-05')

CREATE FUNCTION [dbo].[fnGetEmployeeWorkShift] (  
	@EmployeeId uniqueidentifier,
	@Date Date
)

RETURNS VARCHAR(100)
AS BEGIN

    DECLARE @Result VARCHAR(250),
			@EmployeeWorkShift NVARCHAR(100)
					
		 SELECT TOP 1 @EmployeeWorkShift = workShift.Name 
		 FROM WorkShift AS workShift							
		 INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
		 INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId			
		 INNER JOIN Employee AS employee ON Employee.EmployeeId = employeeWorkShift.EmployeeId
		 WHERE ((CAST(employeeWorkShift.ShiftDate AS DATE) =  CAST(@Date AS DATE)) 
			   AND (employeeWorkShift.EmployeeId = @EmployeeId))
			   AND workShift.IsActive = 1	
			   AND branchUnitWorkShift.IsActive = 1
			   AND employeeWorkShift.IsActive = 1
			   AND employee.IsActive = 1 
		ORDER BY employeeWorkShift.EmployeeWorkShiftId ASC

    RETURN @EmployeeWorkShift
END






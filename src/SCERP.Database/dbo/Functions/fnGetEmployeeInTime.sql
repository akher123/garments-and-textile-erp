
--- SELECT [dbo].[fnGetEmployeeInTime]('D68B04EC-C478-413B-95A4-D3DCBB92F455', '2016-02-16', 185554, '2015-01-01', NULL)

CREATE FUNCTION [dbo].[fnGetEmployeeInTime] 
(  
	@EmployeeId uniqueidentifier,
	@Date Datetime,
	@EmployeeWorkShiftId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME = NULL
)

RETURNS Time

AS BEGIN

		IF(@EmployeeWorkShiftId IS NULL)
			RETURN NULL;
		
		IF(CAST(@JoiningDate AS DATE) > @Date)
		BEGIN
			RETURN  NULL;
		END
	
		IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
		BEGIN
			RETURN  NULL;
		END

		IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
		BEGIN
			RETURN  NULL;
		END		
		
		DECLARE @InTime Time,
			    @StartTime Time,				
				@ShiftStartTime Time,
				@EmployeeCurrentShiftName NVARCHAR(100) = NULL, 
				@employeePresviousWorkShiftId INT = NULL,
				@employeePreviousWorkShiftAttendanceStatus NVARCHAR(100) = NULL,
				@TransactionDate DATETIME 

		SELECT @StartTime = DATEADD(MINUTE, CAST((-WorkShift.MaxBeforeTime) AS DECIMAL),DATEADD(HOUR, - 0, WorkShift.StartTime)),
		@ShiftStartTime = WorkShift.StartTime,
		@EmployeeCurrentShiftName = WorkShift.Name 		
		FROM WorkShift AS workShift							
		INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
		INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId	
		INNER JOIN Employee AS employee ON Employee.EmployeeId = employeeWorkShift.EmployeeId
		WHERE ((CAST(employeeWorkShift.ShiftDate AS DATE) =  CAST(@Date AS DATE)) 
			AND (employeeWorkShift.EmployeeId = @EmployeeId)
			AND employeeWorkShift.EmployeeWorkShiftId = @EmployeeWorkShiftId
			AND workShift.IsActive = 1	
			AND branchUnitWorkShift.IsActive = 1
			AND branchUnitWorkShift.[Status] = 1
			AND employeeWorkShift.IsActive = 1
			AND employeeWorkShift.[Status] = 1
			AND employee.IsActive = 1) 
			ORDER BY WorkShift.StartTime

		IF(@EmployeeCurrentShiftName = 'MORNING')
		BEGIN

			SET @TransactionDate  = DATEADD(DAY, -1, @Date);

			SELECT 
			@employeePresviousWorkShiftId = employeeWorkShift.EmployeeWorkShiftId
			FROM WorkShift AS workShift							
			INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
			INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId
			INNER JOIN Employee AS employee ON Employee.EmployeeId = employeeWorkShift.EmployeeId
			WHERE ((CAST(employeeWorkShift.ShiftDate AS DATE) =  CAST(@TransactionDate AS DATE)) 
			AND (employeeWorkShift.EmployeeId = @EmployeeId)
			AND workShift.IsActive = 1	
			AND branchUnitWorkShift.IsActive = 1
			AND branchUnitWorkShift.[Status] = 1
			AND employeeWorkShift.IsActive = 1
			AND employeeWorkShift.[Status] = 1
			AND employee.IsActive = 1			
			AND WorkShift.Name = 'EVENING')

			IF(@employeePresviousWorkShiftId IS NOT NULL)
			BEGIN
				SET @StartTime = @ShiftStartTime;
			END 
		END
		
		IF(@EmployeeCurrentShiftName = 'EVENING')
		BEGIN
			SET @TransactionDate  = CAST(@Date AS DATE);

			SELECT 
			@employeePresviousWorkShiftId = employeeWorkShift.EmployeeWorkShiftId
			FROM WorkShift AS workShift							
			INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
			INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId
			INNER JOIN Employee AS employee ON Employee.EmployeeId = employeeWorkShift.EmployeeId
			WHERE ((CAST(employeeWorkShift.ShiftDate AS DATE) =  CAST(@TransactionDate AS DATE)) 
			AND (employeeWorkShift.EmployeeId = @EmployeeId)
			AND workShift.IsActive = 1	
			AND branchUnitWorkShift.IsActive = 1
			AND branchUnitWorkShift.[Status] = 1
			AND employeeWorkShift.IsActive = 1
			AND employeeWorkShift.[Status] = 1
			AND employee.IsActive = 1				
			AND workShift.Name = 'MORNING')
		
			IF(@employeePresviousWorkShiftId IS NOT NULL)
			BEGIN
				SET @StartTime = @ShiftStartTime;
			END 
		END

			SELECT TOP(1) @InTime=  CAST(EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance	
			WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
			AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = CAST(@Date AS DATE)
			AND	CAST(EmployeeDailyAttendance.TransactionDateTime AS time) >= @StartTime
			ORDER BY EmployeeDailyAttendance.TransactionDateTime
	
		RETURN @InTime

END


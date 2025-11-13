
--- SELECT [dbo].[fnGetEmployeeOutTimeModel]('f6dfcd1f-0d73-48be-b153-df861fbae72a','2016-02-28',206051,'2015-01-04',NULL)

CREATE FUNCTION [dbo].[fnGetEmployeeOutTimeModel] (  
	@EmployeeId uniqueidentifier,
	@Date Datetime,
	@EmployeeWorkShiftId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME =  NULL
)

RETURNS Time

AS 
BEGIN
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
		

	 	DECLARE @OutTime Time,
				@InTime Time = NULL,
				@StartTime TIME = NULL,
				@EndTime Time,				
				@ShiftStartTime Time,
				@ShiftEndTime Time,
				@NextShiftStartTime Time,
				@EmployeeCurrentShiftName NVARCHAR(100) = NULL, 
				@EmployeeNextWorkShiftId INT = NULL,
				@TransactionDate DATETIME,
				@MaxAfterTime INT,
				@ExceededMaxAfterTime INT  
			
		SET @TransactionDate = CAST(@Date AS DATE);
		SELECT  @StartTime = DATEADD(MINUTE, CAST((-WorkShift.MaxBeforeTime) AS DECIMAL),DATEADD(HOUR, - 0, WorkShift.StartTime)),	
				@EndTime =  DATEADD(MINUTE, CAST((WorkShift.MaxAfterTime) AS DECIMAL), DATEADD(HOUR, 0, WorkShift.EndTime)),																											
				@ShiftStartTime = WorkShift.StartTime,
				@ShiftEndTime = WorkShift.EndTime,
				@EmployeeCurrentShiftName = WorkShift.Name,
				@MaxAfterTime = WorkShift.MaxAfterTime, 
				@ExceededMaxAfterTime = WorkShift.ExceededMaxAfterTime	
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
				SET @TransactionDate  = CAST(@Date AS DATE);

				SELECT 
				@EmployeeNextWorkShiftId = employeeWorkShift.EmployeeWorkShiftId, 
				@NextShiftStartTime =  WorkShift.StartTime
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

				IF(@employeeNextWorkShiftId IS NOT NULL)
				BEGIN
					SET @EndTime = @NextShiftStartTime;
					
					SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
					WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
					AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate
					AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) >= @ShiftStartTime 
					AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) < @EndTime
					ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC

				END 
				ELSE
				BEGIN	
					
					SET @TransactionDate = CAST(DATEADD(DAY, 1, @Date) AS DATE);
					SET @EndTime = DATEADD(MINUTE,@ExceededMaxAfterTime,@ShiftEndTime);
												
					SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
					WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
					AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate
					AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) <= @EndTime
					ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC

					IF(@OutTime IS NULL)
					BEGIN
						SET @TransactionDate  = CAST(@Date AS DATE);
						SET @EndTime = DATEADD(MINUTE,@MaxAfterTime,@ShiftEndTime)
													
						SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
						WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
						AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate
						AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) >= @ShiftStartTime 
						AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) <= @EndTime
						ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC
					END

				END
			END

			IF(@EmployeeCurrentShiftName = 'EVENING')
			BEGIN
				SET @TransactionDate = CAST(DATEADD(DAY, 1, @Date) AS DATE);

				SELECT 
				@EmployeeNextWorkShiftId = employeeWorkShift.EmployeeWorkShiftId, 
				@NextShiftStartTime =  WorkShift.StartTime
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
		
				IF(@employeeNextWorkShiftId IS NOT NULL)
				BEGIN
					SET @EndTime = @NextShiftStartTime;

					SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
					WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
					AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate					
					AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) < @EndTime
					ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC
				END 
				ELSE
				BEGIN
					SET @TransactionDate = CAST(DATEADD(DAY, 1, @Date) AS DATE);
					SET @EndTime = DATEADD(MINUTE,@ExceededMaxAfterTime,@ShiftEndTime);
										
					SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
					WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
					AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate
					AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) <= @EndTime
					ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC

					IF(@OutTime IS NULL)
					BEGIN
						SET @TransactionDate  = CAST(@Date AS DATE);
							
						SELECT TOP(1) @OutTime = CAST( EmployeeDailyAttendance.TransactionDateTime AS time) FROM EmployeeDailyAttendance
						WHERE EmployeeDailyAttendance.EmployeeId = @EmployeeId AND EmployeeDailyAttendance.IsActive = 1
						AND CAST(EmployeeDailyAttendance.TransactionDateTime AS DATE) = @TransactionDate
						AND	CAST( EmployeeDailyAttendance.TransactionDateTime AS time) >= @ShiftStartTime
						ORDER BY EmployeeDailyAttendance.TransactionDateTime DESC
					END
				END
			END

			SET @TransactionDate = CAST(@Date AS DATE);

			SELECT @InTime = InTime FROM EmployeeInOut
			WHERE EmployeeId = @EmployeeId
			AND TransactionDate = @TransactionDate
			AND EmployeeWorkShiftId = @EmployeeWorkShiftId
			AND IsActive = 1

			IF @OutTime = @InTime
			BEGIN
				SET @OutTime  = NULL	
			END

			IF(@OutTime IS NOT NULL)
			BEGIN
				DECLARE  @OTHours NUMERIC(18,2) = 0.0;
				SELECT @OTHours = OTHours FROM EmployeeInOut
				WHERE EmployeeId = @EmployeeId
				AND TransactionDate = @TransactionDate
				AND EmployeeWorkShiftId = @EmployeeWorkShiftId
				AND IsActive = 1;  	
		
				IF(@OTHours <= 0.00 AND @OutTime >=  DATEADD(MINUTE, 24, @ShiftEndTime))
				BEGIN
					SET @OutTime = CAST(DATEADD(MINUTE, CAST((SELECT RandomValue FROM ViewGetRandomValue) * 10 AS INT), @ShiftEndTime) AS TIME)
					SET @OutTime = CAST(DATEADD(SECOND, CAST((SELECT RandomValue FROM ViewGetRandomValue) * (-58) AS INT), @OutTime) AS TIME)
				END

				DECLARE  @ExtraOTHours NUMERIC(18,2) = 0.0;
				SELECT @ExtraOTHours =  ExtraOTHours FROM EmployeeInOut
				WHERE EmployeeId = @EmployeeId
				AND TransactionDate = @TransactionDate
				AND EmployeeWorkShiftId = @EmployeeWorkShiftId
				AND IsActive = 1;   	
		
				IF(@ExtraOTHours > 0.0)
				BEGIN
					SET @OutTime = CAST(DATEADD(MINUTE, CAST((SELECT RandomValue FROM ViewGetRandomValue) * 10 AS INT), @ShiftEndTime) AS TIME)
					SET @OutTime = CAST(DATEADD(SECOND, CAST((SELECT RandomValue FROM ViewGetRandomValue) * (-58) AS INT), @OutTime) AS TIME)
					SET @OutTime = CAST(DATEADD(MINUTE, 120, @OutTime) AS TIME)
				END
			END

			IF(@InTime IS NOT NULL AND @OutTime IS NULL)
			BEGIN
				SET @OutTime = CAST(DATEADD(MINUTE, CAST((SELECT RandomValue FROM ViewGetRandomValue) * 10 AS INT), @ShiftEndTime) AS TIME)
				SET @OutTime = CAST(DATEADD(SECOND, CAST((SELECT RandomValue FROM ViewGetRandomValue) * (-58) AS INT), @OutTime) AS TIME)
			END
		
			RETURN @OutTime
  END




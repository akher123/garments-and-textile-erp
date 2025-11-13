
-- SELECT [dbo].[fnGetExtraOTHours]('9e8405dd-d704-478a-81bf-5aa92f2c548e','2016-02-25',206019,2,'2016-02-09',NULL)

CREATE FUNCTION [dbo].[fnGetExtraOTHours] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATETIME,
	@EmployeeWorkShiftId INT,
	@BranchUnitId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME = NULL
)

RETURNS NUMERIC(18,2)

AS BEGIN
		
	DECLARE @ExtraOTHours NUMERIC(18,2) = 0.0,			
			@MaximumOutTime TIME,
			@InTime TIME,
			@OutTime TIME,
			@ShiftInTime TIME,
			@ShiftOutTime TIME,
			@ExtraOverTimeHours INT,
			@ExtraOverTimeMinutes INT = 0,
			@TotalOverTimeMinutes INT,
			@OvertimeSettingHour NUMERIC(18,2),
			@OvertimeSettingHourInMinute NUMERIC(18,2),
			@Result NUMERIC(18,2);

	BEGIN
				
				IF(CAST(@JoiningDate AS DATE) > @Date)
				BEGIN
					RETURN  @ExtraOTHours;
				END
	
				IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
				BEGIN
					RETURN  @ExtraOTHours;
				END

				IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
				BEGIN
					RETURN  @ExtraOTHours;
				END		
			
				DECLARE @ExistingEmployeeId UNIQUEIDENTIFIER = NULL

			    SELECT TOP(1) @ExistingEmployeeId = employee.EmployeeId
				        FROM Employee employee																
						LEFT JOIN (SELECT EmployeeId, FromDate, IsEligibleForOvertime, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @Date) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						LEFT JOIN OvertimeEligibleEmployee overtimeEligibleEmployee
						ON employee.EmployeeId = overtimeEligibleEmployee.EmployeeId 
						LEFT JOIN EmployeeDailyAttendance employeeDailyAttendance
						ON employee.EmployeeId = employeeDailyAttendance.EmployeeId						
						WHERE employee.EmployeeId = @EmployeeId
						AND employeeCompanyInfo.rowNum = 1
						AND employeeCompanyInfo.IsEligibleForOvertime = 1
						AND overtimeEligibleEmployee.IsActive = 1
						AND employee.IsActive = 1
						AND employeeDailyAttendance.IsActive = 1
						AND CAST(overtimeEligibleEmployee.OvertimeDate AS DATE)= @Date

			
			IF(@ExistingEmployeeId IS NOT NULL)
			
			BEGIN
		
				DECLARE @OTHours NUMERIC(18,2);

				SELECT @OTHours = OTHours FROM EmployeeInOut
				WHERE EmployeeId = @EmployeeId
				AND TransactionDate = @Date
				AND EmployeeWorkShiftId = @EmployeeWorkShiftId
				AND IsActive = 1

				IF (@OTHours > 0.0)
				BEGIN
					SELECT  @ShiftOutTime =  ws.EndTime																											
					FROM WorkShift AS ws							
					INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
					INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId
					INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
					WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(@Date AS DATE)) 
						AND (ews.EmployeeId = @EmployeeId)
						AND ews.EmployeeWorkShiftId = @EmployeeWorkShiftId
						AND ws.IsActive = 1	
						AND buws.IsActive = 1
						AND buws.[Status] = 1
						AND ews.IsActive = 1
						AND ews.[Status] = 1
						AND emp.IsActive = 1) 
						ORDER BY ws.StartTime

					SELECT @OutTime = OutTime FROM EmployeeInOut
					WHERE EmployeeId = @EmployeeId
					AND TransactionDate = @Date
					AND EmployeeWorkShiftId = @EmployeeWorkShiftId
					AND IsActive = 1

					DECLARE @EmployeeCurrentShiftName NVARCHAR(100),
							@MaxAfterEndTime TIME,
							@ExceededMaxAfterTime TIME

					SELECT @EmployeeCurrentShiftName = WorkShift.Name ,
						   @MaxAfterEndTime = DATEADD(MINUTE,workShift.MaxAfterTime,@ShiftOutTime),
						   @ExceededMaxAfterTime = DATEADD(MINUTE,workShift.ExceededMaxAfterTime,@ShiftOutTime)
					FROM WorkShift AS workShift							
					INNER JOIN BranchUnitWorkShift AS branchUnitWorkShift ON branchUnitWorkShift.WorkShiftId = workShift.WorkShiftId
					INNER JOIN EmployeeWorkShift AS employeeWorkShift ON employeeWorkShift.BranchUnitWorkShiftId = branchUnitWorkShift.BranchUnitWorkShiftId		
					WHERE employeeWorkShift.EmployeeWorkShiftId = @EmployeeWorkShiftId
					AND employeeWorkShift.IsActive = 1


					IF(@OutTime >= @ShiftOutTime AND @OutTime <= @MaxAfterEndTime)
					BEGIN
						SET @TotalOverTimeMinutes = DATEDIFF(MINUTE, @ShiftOutTime, @OutTime);
					END

					IF(@OutTime >= DATEADD(SECOND,1,@MaxAfterEndTime) AND @OutTime <= @ExceededMaxAfterTime)
					BEGIN
						SET @TotalOverTimeMinutes = DATEDIFF(MINUTE, @ShiftOutTime, @MaxAfterEndTime)  + DATEDIFF(MINUTE, DATEADD(SECOND,1,@MaxAfterEndTime), @OutTime);
					END


					--SELECT TOP (1) @OvertimeSettingHour =	os.OvertimeHours FROM OvertimeSettings os
					--WHERE os.IsActive = 1
					--AND os.FromDate <= @Date
					--ORDER BY os.FromDate DESC

					SELECT TOP(1) @OvertimeSettingHour = OvertimeHour FROM OvertimeEligibleEmployee
														WHERE OvertimeEligibleEmployee.EmployeeId = @EmployeeId
														AND OvertimeDate = @Date
														AND IsActive = 1

					DECLARE @OvertimeSettingMinute INT;
					SET @OvertimeSettingMinute = @OvertimeSettingHour * 60; 

					IF(@TotalOverTimeMinutes > = @OvertimeSettingMinute)
					BEGIN
						SET @ExtraOvertimeMinutes =   @TotalOverTimeMinutes - @OvertimeSettingMinute;
					END
				

					IF(@ExtraOvertimeMinutes >= 60)
					BEGIN
						SET @ExtraOverTimeHours = @ExtraOvertimeMinutes/60.0;
						SET @ExtraOverTimeMinutes = @ExtraOvertimeMinutes % 60.0;
						SET @ExtraOTHours = @ExtraOverTimeHours;
					END
			
					IF(@ExtraOvertimeMinutes < 25) SET @ExtraOTHours = ISNULL(@ExtraOverTimeHours,0.0) + 0.0;
					IF(@ExtraOvertimeMinutes >= 25 AND @ExtraOvertimeMinutes < 50) SET @ExtraOTHours  = ISNULL(@ExtraOverTimeHours,0.0) + 0.5;
					IF(@ExtraOvertimeMinutes >= 50) SET @ExtraOTHours  = ISNULL(@ExtraOverTimeHours,0.0) + 1.0;

					IF(@ExtraOverTimeHours <= 0.0) SET  @ExtraOTHours = 0.0;
			
					SET @Result = @ExtraOTHours;
				END
			END
		ELSE
			BEGIN
				SET @Result = 0.0;
			END							
					 
	END	
	
	IF(@Result IS NULL)
	  SET @Result = 0.0;

	RETURN @Result

END


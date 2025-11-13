
----- SELECT [dbo].[fnGetOTHours]('73AE4B2C-E2AA-4810-BF47-73A73221C9AB','2016-03-08',187021,2,'2015-10-10',NULL)

CREATE FUNCTION [dbo].[fnGetOTHours] 
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
		
	DECLARE @OTHours NUMERIC(18,2) = 0.0,			
			@MaximumOutTime TIME,
			@ShiftOutTime TIME,
			@OverTimeHours INT,
			@OverTimeMinutes INT,
			@OvertimeSettingHour NUMERIC(18,2),
			@OvertimeSettingHourInMinute NUMERIC(18,2),
			@Result NUMERIC(18,2);

	BEGIN

			IF(CAST(@JoiningDate AS DATE) > @Date)
			BEGIN
				RETURN  @OTHours;
			END
	
			IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
			BEGIN
				RETURN  @OTHours;
			END

			IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
			BEGIN
				RETURN  @OTHours;
			END		


			DECLARE @DailyAttendanceStatus NVARCHAR(100);
			SELECT @DailyAttendanceStatus = [Status] FROM EmployeeInOut
			WHERE EmployeeId = @EmployeeId
			AND TransactionDate = @Date
			AND EmployeeWorkShiftId = @EmployeeWorkShiftId
			AND IsActive = 1

			IF((@DailyAttendanceStatus <> 'PRESENT'))
			BEGIN
				IF(@DailyAttendanceStatus <> 'LATE')
				BEGIN
					RETURN  @OTHours;
				END
			END

			DECLARE @ExistingEmployeeId UNIQUEIDENTIFIER;

			SELECT TOP (1) @ExistingEmployeeId = employee.EmployeeId
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

				DECLARE @CheckWeekEnd BIT = 0; 
				IF DATENAME(weekday, @Date) IN (Select Weekends.[DayName] FROM Weekends WHERE Weekends.IsActive = 1)
				BEGIN
					 IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE 
									BranchUnitId = @BranchUnitId
									AND ExceptionDate = @Date
									AND IsExceptionForWeekend = 1
									AND IsDeclaredAsGeneralDay = 1
									AND IsActive = 1)
					 BEGIN
						  SET @CheckWeekEnd = 1;
					 END
				END

				DECLARE @CheckHoliday BIT = 0; 
				IF EXISTS(SELECT 1 FROM HolidaysSetup 
						  WHERE CAST(HolidaysSetup.StartDate AS date) <= CAST(@Date AS date)  
						  AND  CAST(HolidaysSetup.EndDate AS date) >= CAST(@Date AS date))
				BEGIN
					 IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE  BranchUnitId = @BranchUnitId
									AND ExceptionDate = @Date
									AND IsExceptionForHoliday = 1
									AND IsDeclaredAsGeneralDay = 1
									AND IsActive = 1)
					 BEGIN
						  SET @CheckHoliday = 1;
					 END
				END
					
				
				IF(@CheckWeekEnd != 1 OR @CheckHoliday != 1)
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

					
					DECLARE @OutTime TIME;

					SELECT @OutTime = [OutTime] FROM EmployeeInOut
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
					INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = workShift.WorkShiftId
					INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId					
					WHERE ews.EmployeeWorkShiftId = @EmployeeWorkShiftId
					AND ews.IsActive = 1


					IF(@OutTime >= @ShiftOutTime AND @OutTime <= @MaxAfterEndTime)
					BEGIN
						SET @OverTimeMinutes = DATEDIFF(MINUTE, @ShiftOutTime, @OutTime);
					END

					IF(@OutTime >= DATEADD(SECOND,1,@MaxAfterEndTime) AND @OutTime <= @ExceededMaxAfterTime)
					BEGIN
						SET @OverTimeMinutes = DATEDIFF(MINUTE, @ShiftOutTime, @MaxAfterEndTime)  + DATEDIFF(MINUTE, DATEADD(SECOND,1,@MaxAfterEndTime), @OutTime);
					END

					--SELECT TOP (1) @OvertimeSettingHour =	os.OvertimeHours FROM OvertimeSettings os
					--										WHERE os.IsActive = 1
					--										AND os.FromDate <= @Date
					--										ORDER BY os.FromDate DESC

					SELECT TOP(1) @OvertimeSettingHour = OvertimeHour FROM OvertimeEligibleEmployee
														WHERE OvertimeEligibleEmployee.EmployeeId = @EmployeeId
														AND OvertimeDate = @Date
														AND IsActive = 1

					DECLARE @OvertimeSettingMinute INT;
					SET @OvertimeSettingMinute = @OvertimeSettingHour * 60; 

					IF(@OverTimeMinutes >= @OvertimeSettingMinute)
					BEGIN
						SET @OverTimeMinutes =   @OvertimeSettingMinute;
					END

					IF(@OverTimeMinutes >= 55 AND @OverTimeMinutes < 60)
					BEGIN
						SET @OverTimeMinutes = 60;
					END

					IF(@OverTimeMinutes >= 60)
					BEGIN
						SET @OverTimeHours = @OverTimeMinutes/60.0;
						SET @OverTimeMinutes = @OverTimeMinutes % 60.0;
						SET @OTHours = @OverTimeHours;

						IF(@OverTimeMinutes < 25) SET @OTHours = ISNULL(@OverTimeHours,0.0) + 0.0;
						IF(@OverTimeMinutes >= 25 AND @OverTimeMinutes < 50) SET @OTHours  = ISNULL(@OverTimeHours,0.0) + 0.5;
						IF(@OverTimeMinutes >= 50) SET @OTHours  = ISNULL(@OverTimeHours,0.0) + 1.0;
					END
			
					IF(@OverTimeHours <= 0.0) SET  @OTHours = 0.0;
			
					SET @Result = @OTHours;
				END	
				ELSE
				
				BEGIN
					SET @Result = 0.00;
				END		
		 END
		 ELSE
			BEGIN
				SET @Result = 0.00;
			END							
					 
	END	
	
	IF(@Result IS NULL)
		SET @Result = 0.00 ;

	RETURN @Result

END


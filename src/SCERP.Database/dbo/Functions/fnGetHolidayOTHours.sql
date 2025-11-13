
--- SELECT [dbo].[fnGetHolidayOTHours]('E8711F26-1B8C-437E-A7B4-DFCD6E414187','2015-11-06',86039)

CREATE FUNCTION [dbo].[fnGetHolidayOTHours]
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATETIME,
	@EmployeeWorkShiftId INT,
	@BranchUnitId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME
)

RETURNS NUMERIC(18,2)

AS BEGIN
		
	DECLARE @HolidayOTHours NUMERIC(18,2) = 0.0,			
			@MaximumOutTime TIME,
			@InTime TIME,
			@OutTime TIME,
			@ShiftInTime TIME,
			@ShiftOutTime TIME,
			@HolidayOverTimeHours INT,
			@HolidayOverTimeMinutes INT = 0,
			@TotalHolidayOverTimeMinutes INT,
			@Result NUMERIC(18,2);
	BEGIN
			IF(CAST(@JoiningDate AS DATE) > @Date)
			BEGIN
				RETURN  @HolidayOTHours;
			END
	
			IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
			BEGIN
				RETURN  @HolidayOTHours;
			END

			IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
			BEGIN
				RETURN  @HolidayOTHours;
			END	

			DECLARE @ExistingEmployeeId UNIQUEIDENTIFIER = NULL;

			SELECT TOP(1) @ExistingEmployeeId = employee.EmployeeId 
				        FROM Employee employee																
						LEFT JOIN (SELECT EmployeeId, FromDate, IsEligibleForOvertime, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @Date) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						LEFT JOIN OvertimeEligibleEmployee overtimeEligibleEmployee
						ON (employee.EmployeeId = overtimeEligibleEmployee.EmployeeId)  
						LEFT JOIN EmployeeDailyAttendance employeeDailyAttendance
						ON (employee.EmployeeId = employeeDailyAttendance.EmployeeId)
						WHERE employee.EmployeeId = @EmployeeId
						AND employeeCompanyInfo.rowNum = 1
						AND employeeCompanyInfo.IsEligibleForOvertime = 1
						AND overtimeEligibleEmployee.IsActive = 1
						AND employee.IsActive = 1
						AND employeeDailyAttendance.IsActive = 1
						AND CAST(overtimeEligibleEmployee.OvertimeDate AS DATE)= @Date

			
			IF(@ExistingEmployeeId IS NOT NULL)			
			BEGIN			
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
				ELSE
				BEGIN
					IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE  BranchUnitId = @BranchUnitId
															  AND ExceptionDate = @Date
															  AND IsExceptionForGeneralDay = 1
															  AND IsDeclaredAsHoliday = 1
															  AND IsActive = 1)
					BEGIN
						SET @CheckHoliday = 1;
					END
				END

				IF(@CheckHoliday = 1)
				BEGIN				
						SELECT @InTime = InTime FROM EmployeeInOut
						WHERE EmployeeId = @EmployeeId
						AND TransactionDate = @Date
						AND EmployeeWorkShiftId = @EmployeeWorkShiftId
						AND IsActive = 1


						SELECT  @ShiftInTime =  ws.StartTime																											
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


						IF(@InTime < @ShiftInTime)
						   SET @InTime = @ShiftInTime
					
						SELECT @OutTime = OutTime FROM EmployeeInOut
						WHERE EmployeeId = @EmployeeId
						AND TransactionDate = @Date
						AND EmployeeWorkShiftId = @EmployeeWorkShiftId
						AND IsActive = 1

						IF(@OutTime >=CAST('14:00:00' AS TIME(7)))
							SELECT @HolidayOvertimeMinutes = (DATEDIFF(MINUTE, @InTime, @OutTime)-60); ---60 minutes lunch time 	
						ELSE
							SELECT @HolidayOvertimeMinutes = (DATEDIFF(MINUTE, @InTime, @OutTime)); ---60 minutes lunch time 	

						IF(@HolidayOvertimeMinutes >= 60)
						BEGIN
							SET @HolidayOverTimeHours = @HolidayOvertimeMinutes/60.0;
							SET @HolidayOverTimeMinutes = @HolidayOvertimeMinutes % 60.0;
							SET @HolidayOTHours = @HolidayOverTimeHours;
						END
			
						IF(@HolidayOvertimeMinutes < 25) SET @HolidayOTHours = ISNULL(@HolidayOverTimeHours,0.0) + 0.0;
						IF(@HolidayOvertimeMinutes >= 25 AND @HolidayOvertimeMinutes < 50) SET @HolidayOTHours  = ISNULL(@HolidayOverTimeHours,0.0) + 0.5;
						IF(@HolidayOvertimeMinutes >= 50) SET @HolidayOTHours  = ISNULL(@HolidayOverTimeHours,0.0) + 1.0;

						IF(@HolidayOverTimeHours <= 0.0) SET  @HolidayOTHours = 0.0;
					
						SET @Result = @HolidayOTHours;
					END
				END
		  ELSE
			BEGIN
				SET @Result = 0.0;
			END												 
	END	
	
	IF(@Result IS NULL) 
	   SET @Result = 0.0

	RETURN @Result

END


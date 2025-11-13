
--- SELECT [dbo].[fnGetWeekendOTHours]('9F7E7372-F5FD-4782-8201-53CC4BD2B6D5','2019-08-02',4625779,3,'2017-03-02',NULL)

CREATE FUNCTION [dbo].[fnGetWeekendOTHours]
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
		
	DECLARE @WeekendOTHours NUMERIC(18,2) = 0.0,			
			@MaximumOutTime TIME,
			@InTime TIME,
			@OutTime TIME,
			@ShiftInTime TIME,
			@ShiftOutTime TIME,
			@WeekendOverTimeHours INT,
			@WeekendOverTimeMinutes INT = 0,
			@TotalWeekendOverTimeMinutes INT,
			@BranchUnitWorkShiftId INT,
			@Result NUMERIC(18,2);
	BEGIN

			IF(CAST(@JoiningDate AS DATE) > @Date)
			BEGIN
				RETURN  @WeekendOTHours;
			END
	
			IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
			BEGIN
				RETURN  @WeekendOTHours;
			END

			IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
			BEGIN
				RETURN  @WeekendOTHours;
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
				DECLARE @CheckWeekEnd BIT = 0; 
				IF DATENAME(WEEKDAY, @Date) IN (SELECT Weekends.[DayName] FROM Weekends WHERE Weekends.IsActive = 1)
				BEGIN
					IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE  BranchUnitId = @BranchUnitId
															  AND ExceptionDate = @Date
															  AND IsExceptionForWeekend = 1
															  AND IsDeclaredAsGeneralDay = 1
															  AND IsActive = 1)
					BEGIN
						SET @CheckWeekEnd = 1;
					END
				END
				ELSE
				BEGIN
					IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE  BranchUnitId = @BranchUnitId
															  AND ExceptionDate = @Date
															  AND IsExceptionForGeneralDay = 1
															  AND IsDeclaredAsWeekend = 1
															  AND IsActive = 1)
					BEGIN
						SET @CheckWeekEnd = 1;
					END
				END

				IF(@CheckWeekEnd = 1)
				BEGIN	
						DECLARE @EmployeeCurrentShiftName NVARCHAR(100),
								@MaxAfterEndTime TIME,
								@ExceededMaxAfterTime TIME
									
						SELECT @InTime = InTime 
							  ,@BranchUnitWorkShiftId = BranchUnitWorkShiftId
						FROM EmployeeInOut
						WHERE EmployeeId = @EmployeeId
						AND TransactionDate = @Date
						AND EmployeeWorkShiftId = @EmployeeWorkShiftId
						AND IsActive = 1

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
					
						SELECT  
						@EmployeeCurrentShiftName = ws.Name,
						@MaxAfterEndTime = DATEADD(MINUTE,ws.MaxAfterTime,@ShiftOutTime),
						@ExceededMaxAfterTime = DATEADD(MINUTE,ws.ExceededMaxAfterTime,@ShiftOutTime),
						@ShiftInTime =  ws.StartTime																											
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

						IF(@EmployeeCurrentShiftName ='MORNING')
						BEGIN
						     IF((@OutTime >= CAST('14:00:00' AS TIME(7))) AND (@OutTime <= @MaxAfterEndTime))
							 BEGIN
								SET @WeekendOvertimeMinutes = (DATEDIFF(MINUTE, @InTime, @OutTime)-60); ---60 minutes lunch time 
								IF(CAST(@Date AS DATE) BETWEEN '2019-05-07' AND '2019-06-04')	-- FOR RAMADAN ONLY
									SET @WeekendOvertimeMinutes = @WeekendOvertimeMinutes + 30  -- FOR RAMADAN ONLY
							 END												
							 ELSE IF((@OutTime >= DATEADD(SECOND,1,@MaxAfterEndTime)) AND (@OutTime <= @ExceededMaxAfterTime))
							 BEGIN
							    SET @WeekendOvertimeMinutes = (((DATEDIFF(MINUTE, @InTime, @MaxAfterEndTime))  + (DATEDIFF(MINUTE, DATEADD(SECOND,1,@MaxAfterEndTime), @OutTime)))- 60);							
								IF(CAST(@Date AS DATE) BETWEEN '2019-05-07' AND '2019-06-04')	-- FOR RAMADAN ONLY
									SET @WeekendOvertimeMinutes = @WeekendOvertimeMinutes + 30  -- FOR RAMADAN ONLY
							 END


							 ELSE IF(@BranchUnitWorkShiftId = 50)
							 BEGIN								
								IF(@InTime > @OutTime)
								BEGIN
									SET @WeekendOvertimeMinutes = DATEDIFF(MINUTE, DATEADD(HOUR, 12, @OutTime), DATEADD(HOUR, -12, @InTime))
								END
								ELSE
									SET @WeekendOvertimeMinutes = DATEDIFF(MINUTE, @InTime, @OutTime)
							 END
									
							 ELSE
								SET @WeekendOvertimeMinutes = (DATEDIFF(MINUTE, @InTime, @OutTime)) ---60 minutes lunch time 	
									

							IF(@WeekendOvertimeMinutes < 0)
								SET @WeekendOvertimeMinutes = @WeekendOvertimeMinutes *-1

							IF(@WeekendOvertimeMinutes >= 60)
							BEGIN
								SET @WeekendOverTimeHours = @WeekendOvertimeMinutes/60.0;
								SET @WeekendOverTimeMinutes = @WeekendOvertimeMinutes % 60.0;
								SET @WeekendOTHours = @WeekendOverTimeHours;
							END
			
							IF(@WeekendOvertimeMinutes < 25) SET @WeekendOTHours = ISNULL(@WeekendOverTimeHours,0.0) + 0.0;
							IF(@WeekendOvertimeMinutes >= 25 AND @WeekendOvertimeMinutes < 50) SET @WeekendOTHours  = ISNULL(@WeekendOverTimeHours,0.0) + 0.5;
							IF(@WeekendOvertimeMinutes >= 50) SET @WeekendOTHours  = ISNULL(@WeekendOverTimeHours,0.0) + 1.0;

							IF(@WeekendOverTimeHours <= 0.0) SET  @WeekendOTHours = 0.0;

							SET @Result = @WeekendOTHours;
						END				

						 -- Newly Added
						IF(@EmployeeCurrentShiftName ='Evening')
						BEGIN
							SET @WeekendOvertimeMinutes = (DATEDIFF(MINUTE, @OutTime, @InTime))
							
							IF(@InTime > @OutTime)
							BEGIN
								SET @WeekendOvertimeMinutes = (DATEDIFF(MINUTE, @date + CAST(@InTime AS DATETIME), DATEADD(DAY, 1, @date) + CAST(@OutTime AS DATETIME) ))
							END 

							IF(@WeekendOvertimeMinutes < 0)
								SET @WeekendOvertimeMinutes = @WeekendOvertimeMinutes *-1
																																									
							IF(@WeekendOvertimeMinutes >= 60)
							BEGIN
								SET @WeekendOverTimeHours = @WeekendOvertimeMinutes/60.0;
								SET @WeekendOverTimeMinutes = @WeekendOvertimeMinutes % 60.0;
								SET @WeekendOTHours = @WeekendOverTimeHours;
							END
			
							IF(@WeekendOvertimeMinutes < 25) SET @WeekendOTHours = ISNULL(@WeekendOverTimeHours,0.0) + 0.0;
							IF(@WeekendOvertimeMinutes >= 25 AND @WeekendOvertimeMinutes < 50) SET @WeekendOTHours  = ISNULL(@WeekendOverTimeHours,0.0) + 0.5;
							IF(@WeekendOvertimeMinutes >= 50) SET @WeekendOTHours  = ISNULL(@WeekendOverTimeHours,0.0) + 1.0;

							IF(@WeekendOverTimeHours <= 0.0) SET  @WeekendOTHours = 0.0;

							SET @Result = @WeekendOTHours;
						END
						
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

--- Select [dbo].[fnGetEmployeeAttendanceStatus] ('D68B04EC-C478-413B-95A4-D3DCBB92F455','2016-02-16','185554',1,'2016-01-01', NULL)

CREATE FUNCTION [dbo].[fnGetEmployeeAttendanceStatus] 
(  
	@EmployeeId uniqueidentifier,
	@Date Date,
	@EmployeeWorkShiftId INT,
	@BranchUnitId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME = NULL
)

RETURNS VARCHAR(100)
AS BEGIN
	
	IF(@EmployeeWorkShiftId IS NULL)
			RETURN NULL;

    DECLARE @Result VARCHAR(250),
			@InTime TIME,
			@ShiftInTime TIME

	IF(CAST(@JoiningDate AS DATE) > @Date)
	BEGIN
		SET @Result = ''
		RETURN  @Result;
	END
	
	
	IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
	BEGIN
		SET @Result = ''
		RETURN  @Result;
	END

	IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
	BEGIN
		SET @Result = ''
		RETURN  @Result;
	END		
	ELSE
		SET @Result ='Absent'


	IF EXISTS(SELECT 1 FROM EmployeeLeaveDetail 
			  WHERE EmployeeLeaveDetail.EmployeeId = @EmployeeId 
			  AND EmployeeLeaveDetail.IsActive = 1 
			  AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) = CAST(@Date AS DATE))
	RETURN 'Leave'


	-- Newly added for Individual Employee Holiday
	IF EXISTS (SELECT * FROM EmployeeHoliday WHERE EmployeeId = @EmployeeId
														AND CAST(@Date AS DATE) BETWEEN CAST(FromDate AS DATE)AND CAST(ToDate AS DATE)
														AND IsActive = 1)																
	BEGIN
			RETURN 'Holiday'
	END

	-- Newly added for Individual Employee Weekend
	IF EXISTS (SELECT * FROM EmployeeWeekend WHERE EmployeeId = @EmployeeId
														AND CAST(@Date AS DATE) BETWEEN CAST(FromDate AS DATE)AND CAST(ToDate AS DATE)
														AND IsActive = 1)																
	BEGIN
			RETURN 'Weekend'
	END


	IF EXISTS (SELECT 1 FROM EmployeeDailyAttendance 
			  WHERE CAST(EmployeeDailyAttendance.TransactionDateTime AS date) = @Date
			  AND EmployeeDailyAttendance.EmployeeId = @EmployeeId 
			  AND EmployeeDailyAttendance.IsActive=1)
	BEGIN		
		
		DECLARE @DayStatus NVARCHAR(100) = ''

		IF EXISTS(SELECT 1 FROM HolidaysSetup 
			  WHERE CAST(HolidaysSetup.StartDate AS date) <= CAST(@Date AS date)  
			  AND  CAST(HolidaysSetup.EndDate AS date) >= CAST(@Date AS date))
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForHoliday = 1
														AND IsDeclaredAsGeneralDay = 1
												        AND IsActive = 1)
			BEGIN
				SET @DayStatus = 'Holiday'
			END
		END
				
		IF DATENAME(weekday, @Date) IN (Select Weekends.[DayName] FROM Weekends WHERE Weekends.IsActive = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForWeekend = 1
														AND IsDeclaredAsGeneralDay = 1
												        AND IsActive = 1)
			BEGIN
				SET @DayStatus = 'Weekend'
			END
		END
			

		IF EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForGeneralDay= 1
														AND IsDeclaredAsHoliday = 1
												        AND IsActive = 1)
		BEGIN
			SET @DayStatus = 'Holiday'
		END
		

		IF EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForGeneralDay = 1
														AND IsDeclaredAsWeekend = 1
												        AND IsActive = 1)
		BEGIN
			SET @DayStatus = 'Weekend'
		END
		

		IF((@DayStatus = 'Holiday') OR (@DayStatus = 'Weekend'))
		BEGIN
			RETURN @DayStatus
		END

		SELECT @InTime = InTime FROM EmployeeInOut
		WHERE EmployeeId = @EmployeeId
		AND TransactionDate = @Date
		AND EmployeeWorkShiftId = @EmployeeWorkShiftId
		AND IsActive = 1

		IF (@InTime IS NULL)
			RETURN @Result;

		DECLARE @DelayTime INT;
		SELECT @DelayTime = LateInMinutes FROM EmployeeInOut
		WHERE EmployeeId = @EmployeeId
		AND TransactionDate = @Date
		AND EmployeeWorkShiftId = @EmployeeWorkShiftId
		AND IsActive = 1

		IF @DelayTime > 0
			RETURN 'Late'
		ELSE
			RETURN 'Present'
		
	END

	IF EXISTS(SELECT 1 FROM OutStationDuty WHERE OutStationDuty.EmployeeId = @EmployeeId 
			  AND CAST(OutStationDuty.DutyDate AS date) = CAST(@Date AS date) 
			  AND OutStationDuty.IsActive =1)
	RETURN 'OSD'

	IF EXISTS(SELECT 1 FROM HolidaysSetup 
			  WHERE CAST(HolidaysSetup.StartDate AS date) <= CAST(@Date AS date)  
			  AND  CAST(HolidaysSetup.EndDate AS date) >= CAST(@Date AS date))
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForHoliday = 1
														AND IsDeclaredAsGeneralDay = 1
												        AND IsActive = 1)
		RETURN 'Holiday'
			
	END

	IF EXISTS(SELECT 1 FROM Weekends WHERE Weekends.IsActive = 1 AND Weekends.[DayName] = DATENAME(dw, @Date))
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
														AND ExceptionDate = @Date
														AND IsExceptionForWeekend = 1
														AND IsDeclaredAsGeneralDay = 1
												        AND IsActive = 1)
			
		RETURN 'Weekend'
			
	END

	
	IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
													AND ExceptionDate = @Date
													AND IsExceptionForGeneralDay = 1
													AND IsDeclaredAsHoliday = 1
												    AND IsActive = 1)
	BEGIN
		RETURN 'Holiday'
	END


	IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
													AND ExceptionDate = @Date
													AND IsExceptionForGeneralDay = 1
													AND IsDeclaredAsWeekend = 1
												    AND IsActive = 1)
	BEGIN
		RETURN 'Weekend'
	END


    RETURN @Result
END




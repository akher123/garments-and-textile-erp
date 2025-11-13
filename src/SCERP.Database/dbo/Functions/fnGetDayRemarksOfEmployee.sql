
--- SELECT [dbo].[fnGetDayRemarksOfEmployee]('E64F3691-9B72-4F12-888E-8BEEBE5A780E','2016-02-21','194014',1)

CREATE FUNCTION [dbo].[fnGetDayRemarksOfEmployee] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATETIME,
	@EmployeeWorkShiftId INT,
	@BranchUnitId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME
)

RETURNS VARCHAR(100)

AS BEGIN
   
	 DECLARE @Result VARCHAR(250),
			 @InTime TIME,
			 @ShiftInTime TIME,
			 @DelayTime int;


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
	
	DECLARE @AttendanceStatus NVARCHAR(100);

	SELECT @AttendanceStatus = [Status] FROM EmployeeInOut
	WHERE EmployeeId = @EmployeeId
	AND TransactionDate = @Date
	AND EmployeeWorkShiftId = @EmployeeWorkShiftId
	AND IsActive = 1

	IF (@AttendanceStatus ='WEEKEND')
	BEGIN
		 RETURN ''
	END

	/*Present*/
	IF (@AttendanceStatus = 'PRESENT')
	BEGIN
		  RETURN ''
	END

	/*Late*/
	IF (@AttendanceStatus = 'LATE')
	BEGIN
		 RETURN @AttendanceStatus
	END

	/*Late*/
	IF (@AttendanceStatus = 'OSD')
	BEGIN
		 RETURN @AttendanceStatus
	END

	/*Leave*/
	IF (@AttendanceStatus = 'LEAVE')
	BEGIN
		SELECT @Result = EmployeeLeaveDetail.LeaveTypeTitle
		FROM EmployeeLeaveDetail 
		WHERE EmployeeLeaveDetail.EmployeeId = @EmployeeId 
		AND EmployeeLeaveDetail.IsActive = 1 
		AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) = CAST(@Date AS DATE)
		RETURN @Result
	END

	/* Leave */
	IF (@AttendanceStatus = 'HOLIDAY')
	BEGIN

		/* Individual Employee Leave Adjusted Holiday */
		IF EXISTS (SELECT 1 FROM EmployeeHoliday WHERE EmployeeId = @EmployeeId
												   AND CAST(@Date AS DATE) BETWEEN CAST(FromDate AS DATE)AND CAST(ToDate AS DATE)
												   AND IsActive = 1)
		BEGIN
			RETURN 'Ind. Adjust. Holiday'
		END

		SELECT @Result = HolidaysSetup.Title FROM HolidaysSetup WHERE @Date >= CAST(HolidaysSetup.StartDate AS date) AND @Date <= CAST(HolidaysSetup.EndDate AS date)
		RETURN @Result;
	END

	
	/*Adjusted Weekend*/
	IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
													AND ExceptionDate = @Date
													AND IsExceptionForGeneralDay = 1
													AND IsDeclaredAsWeekend = 1
												    AND IsActive = 1)
	BEGIN
		RETURN 'Adjusted Weekend'
	END


	/*Adjusted Holiday*/
	IF  EXISTS (SELECT 1 FROM ExceptionDay WHERE BranchUnitId = @BranchUnitId
													AND ExceptionDate = @Date
													AND IsExceptionForGeneralDay = 1
													AND IsDeclaredAsHoliday = 1
												    AND IsActive = 1)
	BEGIN
		RETURN 'Adjusted Holiday'
	END

	

    RETURN @Result
END



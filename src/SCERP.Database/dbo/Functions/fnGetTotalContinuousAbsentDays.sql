
--- SELECT [dbo].[fnGetTotalContinuousAbsentDays]('C30F6EA6-53FA-4000-AF5C-1896CC8EA7B6','2016-03-16', 0, 233521)

CREATE FUNCTION [dbo].[fnGetTotalContinuousAbsentDays] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATE,
	@Count INT,
	@EmployeeWorkShiftId INT
)

RETURNS  INT

AS BEGIN

		 DECLARE @DailyAttendanceStatus NVARCHAR(100) = '',
				 @PreviousWorkShiftId INT;

		 SET @PreviousWorkShiftId = @EmployeeWorkShiftId;

		 SELECT @DailyAttendanceStatus = [Status] FROM EmployeeInOut
		 WHERE EmployeeWorkShiftId = @EmployeeWorkShiftId
		 AND IsActive = 1
	
		 WHILE(@Count < 10)
		 BEGIN
			
			 SET @Date = DATEADD(DD, -1, @Date);

			 SELECT TOP(1) @EmployeeWorkShiftId = EmployeeWorkShiftId FROM EmployeeInOut
			 WHERE EmployeeId = @EmployeeId
			 AND TransactionDate = @Date
			 AND IsActive = 1
		 
			

			 IF(@DailyAttendanceStatus = 'ABSENT')
			 BEGIN
				SET @Count = @Count + 1 
			    
				IF(@EmployeeWorkShiftId = @PreviousWorkShiftId)
				BREAK;

			    SET @PreviousWorkShiftId = @EmployeeWorkShiftId	
				
				SELECT @DailyAttendanceStatus = [Status] FROM EmployeeInOut
				WHERE EmployeeWorkShiftId = @EmployeeWorkShiftId
				CONTINUE;
			 END
			 ELSE IF(@DailyAttendanceStatus = 'WEEKEND')
			 BEGIN
				IF(@EmployeeWorkShiftId = @PreviousWorkShiftId)
				BREAK;

			    SET @PreviousWorkShiftId = @EmployeeWorkShiftId	

				SELECT @DailyAttendanceStatus = [Status] FROM EmployeeInOut
				WHERE EmployeeWorkShiftId = @EmployeeWorkShiftId
				CONTINUE;
			 END
			 ELSE IF(@DailyAttendanceStatus = 'HOLIDAY')
			 BEGIN
				IF(@EmployeeWorkShiftId = @PreviousWorkShiftId)
				BREAK;

			    SET @PreviousWorkShiftId = @EmployeeWorkShiftId	

				SELECT @DailyAttendanceStatus = [Status] FROM EmployeeInOut
				WHERE EmployeeWorkShiftId = @EmployeeWorkShiftId
				CONTINUE;
			 END
			
			 ELSE IF (@DailyAttendanceStatus = 'PRESENT')
			 BEGIN
				BREAK;
			 END				
			 ELSE IF (@DailyAttendanceStatus = 'LATE') 
			 BEGIN
				BREAK;
			 END
			 ELSE IF (@DailyAttendanceStatus = 'OSD')
			 BEGIN
				BREAK;
			 END
			 ELSE IF(@DailyAttendanceStatus = 'LEAVE')
			 BEGIN
				BREAK;
			 END
			 ELSE IF (@DailyAttendanceStatus = '' OR @DailyAttendanceStatus IS NULL)
			 BEGIN
				BREAK;
			 END			
		 END

		 RETURN @Count

END



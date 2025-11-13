
--- SELECT [dbo].[fnGetLastDayWeekendOTHours]('E8711F26-1B8C-437E-A7B4-DFCD6E414187','2015-11-21')

CREATE FUNCTION [dbo].[fnGetLastDayWeekendOTHours] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATE
)

RETURNS NUMERIC(18,2)

AS BEGIN

		  DECLARE @LastDayWeekendOTHours NUMERIC(18,2) = NULL,
			      @TransactionDate DATETIME,
				  @EmployeeLastDayWorkShiftId INT;
 
		 SET @TransactionDate = DATEADD(DAY, -1, @Date);

		 SELECT TOP(1) @EmployeeLastDayWorkShiftId = ews.EmployeeWorkShiftId 
		 FROM EmployeeWorkShift ews
		 WHERE ews.EmployeeId = @EmployeeId
		 AND ews.[Status] = 1
		 AND ews.IsActive = 1
		 AND ews.ShiftDate = @TransactionDate
		 ORDER BY ews.EmployeeWorkShiftId DESC

		 SELECT TOP (1) @LastDayWeekendOTHours =  eio.WeekendOTHours
		 FROM  EmployeeInOut eio
		 WHERE eio.EmployeeWorkShiftId = @EmployeeLastDayWorkShiftId

		 RETURN @LastDayWeekendOTHours

END



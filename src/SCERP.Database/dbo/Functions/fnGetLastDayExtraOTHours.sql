
--- SELECT [dbo].[fnGetLastDayExtraOTHours]('AE1E9C92-418E-424B-BE18-509A58C00F3D','2015-10-11')

CREATE FUNCTION [dbo].[fnGetLastDayExtraOTHours] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATE
)

RETURNS NUMERIC(18,2)

AS BEGIN

		DECLARE  @LastDayExtraOTHours NUMERIC(18,2) = NULL,
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

		 SELECT TOP (1) @LastDayExtraOTHours =  eio.ExtraOTHours
		 FROM  EmployeeInOut eio
		 WHERE eio.EmployeeWorkShiftId = @EmployeeLastDayWorkShiftId

		 RETURN @LastDayExtraOTHours

END



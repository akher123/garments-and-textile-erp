
--- SELECT [dbo].[fnGetEmployeeLastDayOutTimeModel]('13A58B03-E1F2-4386-BEEF-81FC1EE2BC61','2015-11-28')

CREATE FUNCTION [dbo].[fnGetEmployeeLastDayOutTimeModel] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@Date DATE
)

RETURNS TIME

AS BEGIN

	DECLARE @LastDayOutTime Time = NULL,
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

	SELECT TOP (1) @LastDayOutTime =  eio.OutTime
	FROM  EmployeeInOutModel eio
	WHERE eio.EmployeeWorkShiftId = @EmployeeLastDayWorkShiftId

	RETURN @LastDayOutTime

END



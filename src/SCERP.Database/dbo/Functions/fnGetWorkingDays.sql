
---Select [dbo].[fnGetWorkingDays] ('FC136FFD-5E86-43F1-B7CB-3B5D53037DC0','2015-10-01','2015-10-31')

CREATE FUNCTION [dbo].[fnGetWorkingDays] (  

	@EmployeeId UNIQUEIDENTIFIER,
	@FromDate DATETIME,
	@ToDate DATETIME
)

RETURNS INT

AS BEGIN
	
	DECLARE @JoiningDate DATETIME;
	SELECT @JoiningDate = JoiningDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1
	
	IF(CAST(@JoiningDate AS DATE) > CAST(@FromDate AS DATE))
				SELECT @FromDate = CAST(@JoiningDate AS DATE) 
	
	DECLARE @QuitDate DATETIME;
	SELECT @QuitDate = QuitDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND [Status] = 2
	
	IF(@QuitDate IS NOT NULL)
	BEGIN
		IF((CAST(@QuitDate AS DATE) >= CAST(@FromDate AS DATE)) AND (CAST(@QuitDate AS DATE) < CAST(@ToDate AS DATE)))
				SELECT @ToDate = CAST(@QuitDate AS DATE) 
	END


	IF(@ToDate > CAST(CURRENT_TIMESTAMP AS DATE))
		SELECT @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
	
	DECLARE @TotalWorkingDays INT = 0;

	SET @TotalWorkingDays = (DATEDIFF(DAY, @FromDate, @ToDate) + 1) - (dbo.fnGetHolidays(@EmployeeId,@FromDate,@ToDate)) - (dbo.fnGetWeekend(@EmployeeId,@FromDate,@ToDate))
	
RETURN @TotalWorkingDays 
END







---Select [dbo].[fnGetPayDaysModel] ('FC136FFD-5E86-43F1-B7CB-3B5D53037DC0','2015-10-01','2015-10-31')

CREATE FUNCTION [dbo].[fnGetPayDaysModel] (  

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

	DECLARE @TotalPayDays INT = 0;

	SET @TotalPayDays = (DATEDIFF(DAY, @FromDate, @ToDate) + 1) -  (dbo.fnGetAbsentDaysModel(@EmployeeId,@FromDate,@ToDate)) - (dbo.fnGetIndividualLeaveDaysModel(@EmployeeId,@FromDate,@ToDate,6))
	
RETURN @TotalPayDays 
END






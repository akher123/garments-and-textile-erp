
CREATE FUNCTION [dbo].[fnGetTotalWeekendOTHours] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS NUMERIC(18,2)

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
	
	  DECLARE	@TotalWeekendOTHours DECIMAL(18,2) = 0.00

	  SELECT @TotalWeekendOTHours = SUM(eio.WeekendOTHours)
					   FROM EmployeeInOut eio
					   WHERE  eio.EmployeeId = @EmployeeId
					   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					   AND eio.IsActive = 1
	 
	 IF @TotalWeekendOTHours IS NULL 
	 RETURN 0.00

	 RETURN @TotalWeekendOTHours

END







--- SELECT [dbo].[fnGetTotalOTHours]('C9B68DCA-8FA4-4CCC-84F8-7661389F5795','2016-05-26','2016-06-25')

CREATE FUNCTION [dbo].[fnGetTotalOTHours] (  

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
	
	  DECLARE	@TotalOTHours DECIMAL(18,2) = 0.00

	  SELECT @TotalOTHours = SUM(eio.OTHours)
					   FROM EmployeeInOut eio
					   WHERE  eio.EmployeeId = @EmployeeId
					   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					   AND eio.IsActive = 1
	 
	 IF @TotalOTHours IS NULL 
	 RETURN 0.00

	 RETURN @TotalOTHours

END





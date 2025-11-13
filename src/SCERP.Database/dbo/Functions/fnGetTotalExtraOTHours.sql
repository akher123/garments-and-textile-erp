
CREATE FUNCTION [dbo].[fnGetTotalExtraOTHours] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS NUMERIC(18,2)

AS BEGIN

	 --DECLARE	@Days INT,				
	 --		@Result NUMERIC(18,2),
	 --		@TotalExtraOTHours DECIMAL(18,2) = 0.00


	 --SET @Days = DATEDIFF(DAY, @FromDate, @ToDate)
	 --WHILE @Days >= 0
	 --BEGIN
				
	 --		 SET @TotalExtraOTHours = @TotalExtraOTHours + CAST(dbo.fnGetExtraOTHours(@EmployeeId,@FromDate) AS DECIMAL(18,2))
	 --		 SET @FromDate =  DATEADD (day , 1 , @FromDate)
	 --		 SET @Days = @Days - 1

	 --END	

	 --IF @TotalExtraOTHours IS NULL 
	 --RETURN 0.00

	 --SET @Result = CONVERT(NUMERIC(18,2), @TotalExtraOTHours)

	 --RETURN @Result


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
	
	  DECLARE	@TotalExtraOTHours DECIMAL(18,2) = 0.00

	  SELECT @TotalExtraOTHours = SUM(eio.ExtraOTHours)
					   FROM EmployeeInOut eio
					   WHERE  eio.EmployeeId = @EmployeeId
					   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					   AND eio.IsActive = 1
	 
	 IF @TotalExtraOTHours IS NULL 
	 RETURN 0.00

	 RETURN @TotalExtraOTHours

END






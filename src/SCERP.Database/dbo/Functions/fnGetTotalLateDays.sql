
--- select [dbo].[fnGetTotalLateDays] ('0ACBE4E5-518E-4243-B591-9010C747DB59','2015-10-01','2015-10-31')  

CREATE FUNCTION [dbo].[fnGetTotalLateDays] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS int

AS BEGIN

	DECLARE	@Days INT = 0,
			@Count INT = 0,
			@DelayTime INT = 0;	
	
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

	DECLARE @Employee uniqueidentifier;

	SELECT @Employee = eio.EmployeeId FROM EmployeeInOut eio
					   WHERE  eio.EmployeeId = @EmployeeId
					   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					   AND eio.IsActive = 1
					   AND REPLACE(eio.[Status], ' ', '') = 'LATE'

	RETURN @@ROWCOUNT 


END



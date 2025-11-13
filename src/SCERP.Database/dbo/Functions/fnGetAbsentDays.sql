
---- Select [dbo].[fnGetAbsentDays] ( '64F6B58B-4E46-4D10-8FFC-99CC39BDC6ED','2017-06-11','2017-06-25')
 
CREATE FUNCTION [dbo].[fnGetAbsentDays] (  
	@EmployeeId uniqueidentifier,
	@FromDate DateTime,
	@ToDate DateTime
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

	DECLARE @Employee uniqueidentifier;


	SELECT @Employee = eio.EmployeeId FROM EmployeeInOut eio
					   WHERE  eio.EmployeeId = @EmployeeId
					   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
					   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
					   AND eio.IsActive = 1
					   AND REPLACE(eio.[Status], ' ', '') = 'ABSENT'


	RETURN @@ROWCOUNT 


END





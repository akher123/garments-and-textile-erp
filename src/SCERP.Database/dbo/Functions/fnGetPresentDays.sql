
CREATE FUNCTION [dbo].[fnGetPresentDays] (  

	@EmployeeId uniqueidentifier,
	@FromDate Datetime,
	@ToDate Datetime
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

	DECLARE @TotalPresentDays INT = 0, @PresentDays INT = 0, @LateDays INT = 0, @OSDDays INT = 0;

	

	SELECT @PresentDays =  COUNT(eio.EmployeeId) FROM EmployeeInOut eio
						   WHERE  eio.EmployeeId = @EmployeeId
						   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
						   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
						   AND eio.IsActive = 1
						   AND REPLACE(eio.[Status], ' ', '') = 'PRESENT'

	SELECT @LateDays =  COUNT(eio.EmployeeId) FROM EmployeeInOut eio
						   WHERE  eio.EmployeeId = @EmployeeId
						   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
						   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
						   AND eio.IsActive = 1
						   AND REPLACE(eio.[Status], ' ', '') = 'LATE'

	SELECT @OSDDays =  COUNT(eio.EmployeeId) FROM EmployeeInOut eio
						   WHERE  eio.EmployeeId = @EmployeeId
						   AND (CAST(eio.TransactionDate AS DATE)) >= (CAST(@FromDate AS DATE))
						   AND (CAST(eio.TransactionDate AS DATE)) <= (CAST(@ToDate AS DATE))
						   AND eio.IsActive = 1
						   AND REPLACE(eio.[Status], ' ', '') = 'OSD'


	SET @TotalPresentDays = ISNULL(@PresentDays,0) + ISNULL(@LateDays,0) + ISNULL(@OSDDays,0);

	RETURN @TotalPresentDays;
	 
END





-- SELECT [dbo].[fnGetEmployeeTotalPenaltyFinancialAmount] ('54aab6b7-d401-4186-9206-46436b17a143','2015-01-01',NULL,'2016-02-26','2016-03-25',5)

CREATE FUNCTION [dbo].[fnGetEmployeeTotalPenaltyFinancialAmount] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@JoiningDate DATETIME,
	@QuitDate DATETIME,
	@FromDate DATETIME,
	@ToDate DATETIME
)

RETURNS NUMERIC(18,2)

AS BEGIN

	IF(CAST(@JoiningDate AS DATE) > CAST(@FromDate AS DATE))
				SET @FromDate = CAST(@JoiningDate AS DATE) 

	IF(@QuitDate IS NOT NULL)
	BEGIN
		IF((CAST(@QuitDate AS DATE) >= CAST(@FromDate AS DATE)) AND (CAST(@QuitDate AS DATE) < CAST(@ToDate AS DATE)))
				SET @ToDate = CAST(@QuitDate AS DATE) 
	END

	IF(@ToDate > CAST(CURRENT_TIMESTAMP AS DATE))
		SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)

	DECLARE @TotalPenaltyFinancialAmount NUMERIC(18,2) = 0.0;

	SELECT @TotalPenaltyFinancialAmount =   SUM(Penalty) FROM HrmPenalty 
										    WHERE PenaltyTypeId = 4 --- 4 For Financial Amount
										    AND EmployeeId = @EmployeeId
										    AND (CAST(PenaltyDate AS DATE)) >= (CAST(@FromDate AS DATE))
										    AND (CAST(PenaltyDate AS DATE)) <= (CAST(@ToDate AS DATE))
										    AND IsActive = 1

	RETURN ISNULL(@TotalPenaltyFinancialAmount,0.0);

END



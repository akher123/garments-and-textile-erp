
-- SELECT [dbo].[fnGetEmployeeTotalPenaltyLeaveDays] ('54aab6b7-d401-4186-9206-46436b17a143','2015-01-01',NULL,'2016-02-26','2016-03-25',4)

CREATE FUNCTION [dbo].[fnGetEmployeeTotalPenaltyLeaveDays] 
(  
	@EmployeeId UNIQUEIDENTIFIER,
	@JoiningDate DATETIME,
	@QuitDate DATETIME,
	@FromDate DATETIME,
	@ToDate DATETIME
)

RETURNS INT

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

	DECLARE @TotalPenaltyAttendanceDays INT = 0;

	SELECT @TotalPenaltyAttendanceDays =   SUM(Penalty) FROM HrmPenalty 
										   WHERE PenaltyTypeId = 3 --- 3 For Leave Days
										   AND EmployeeId = @EmployeeId
										   AND (CAST(PenaltyDate AS DATE)) >= (CAST(@FromDate AS DATE))
										   AND (CAST(PenaltyDate AS DATE)) <= (CAST(@ToDate AS DATE))
										   AND IsActive = 1

	RETURN ISNULL(@TotalPenaltyAttendanceDays,0);

	
END



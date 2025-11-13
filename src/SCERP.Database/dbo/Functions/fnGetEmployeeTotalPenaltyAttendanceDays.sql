
-- SELECT [dbo].[fnGetEmployeeTotalPenaltyAttendanceDays] ('F1BE80F5-F3DB-4284-A141-FD364D9E1D63','2016-03-13',NULL,'2017-09-26','2017-10-25')

CREATE FUNCTION [dbo].[fnGetEmployeeTotalPenaltyAttendanceDays] 
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
										   WHERE PenaltyTypeId = 2 --- 2 For Attendance Days
										   AND EmployeeId = @EmployeeId
										   AND (CAST(PenaltyDate AS DATE)) >= (CAST(@FromDate AS DATE))
										   AND (CAST(PenaltyDate AS DATE)) <= (CAST(@ToDate AS DATE))
										   AND IsActive = 1

	RETURN ISNULL(@TotalPenaltyAttendanceDays,0);

	
END



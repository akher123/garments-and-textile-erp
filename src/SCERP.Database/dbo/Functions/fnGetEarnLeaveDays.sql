
-- SELECT dbo.fnGetEarnLeaveDays('96606683-9F0E-42B1-92AB-4AF7A87BC6F6','2018-11-26', '2019-02-03')

CREATE FUNCTION [dbo].[fnGetEarnLeaveDays] (  

							@EmployeeId UNIQUEIDENTIFIER,
							@FromDate Datetime,
							@ToDate Datetime
)

RETURNS DECIMAL(18,2)

AS BEGIN

			DECLARE @EarnLeave DECIMAL(18,2) = 0.0
			DECLARE @EarnLeaveConsumed DECIMAL(18,2) = 0.0

			DECLARE @JoinDate DATETIME

			SELECT @JoinDate = joiningDate FROM Employee WHERE EmployeeId = @EmployeeId

			IF(DATEDIFF(DAY, @JoinDate, @ToDate) < 365)
			BEGIN
					RETURN 0.0
			END

			SELECT @EarnLeave = COUNT(1) FROM EmployeeInOut WHERE EmployeeId = @EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate AND IsActive = 1 

			SELECT @EarnLeaveConsumed = SUM(Days) FROM [EarnLeavegivenByYear] WHERE [EarnLeavegivenByYear].EmployeeId = @EmployeeId GROUP BY EmployeeId

			RETURN (@EarnLeave/18) - ISNULL(@EarnLeaveConsumed, 0.0)

END
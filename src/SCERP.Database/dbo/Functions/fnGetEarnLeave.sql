
-- SELECT [dbo].[fnGetEarnLeave]('616ECDC1-2CCA-402F-BEB0-5659BEAEBB9E','2016-06-06','2016-01-01','earnleave')

CREATE FUNCTION [dbo].[fnGetEarnLeave] (  

					@EmployeeId UNIQUEIDENTIFIER
				   ,@FromDate DATETIME
				   ,@ToDate DATETIME
				   ,@Type NVARCHAR(10)
)

RETURNS DECIMAL(18,2)

AS BEGIN
	
				SET @FromDate = CAST(@FromDate AS DATE)
				SET @ToDate = CAST(@ToDate AS DATE)				
				DECLARE @Result DECIMAL(18,2) = 0.0

	SELECT @Result = 

	  CASE
		  WHEN @Type = 'TotalDays' THEN (SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = @EmployeeId AND (Status = 'Present') AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate AND IsActive = 1)
		  WHEN @Type = 'EarnLeave' THEN CONVERT(DECIMAL(18,2),(CONVERT( DECIMAL(18,2),(SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = @EmployeeId AND (Status = 'Present') AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate AND IsActive = 1)))/18)
		  WHEN @Type = 'Previous'  THEN CONVERT(DECIMAL(18,2),(CONVERT( DECIMAL(18,2),(SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = @EmployeeId AND (Status = 'Present') AND CAST(TransactionDate AS DATE) < @FromDate AND IsActive = 1)))/18)
		  WHEN @Type = 'Consumed'  THEN CONVERT(DECIMAL(18,2),(SELECT SUM(ApprovedTotalDays) FROM EmployeeLeave WHERE EmployeeId = @EmployeeId AND LeaveTypeId = 5 AND ApprovalStatus = 1 AND IsActive = 1 AND CAST(ApprovedFromDate AS DATE) >= @FromDate AND CAST(ApprovedToDate AS DATE) <= @ToDate))
		  WHEN @Type = 'Disbursed' THEN (SELECT SUM(Days) FROM HrmEarnLeaveDisbursement WHERE EmployeeId = @EmployeeId AND Date BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) AND IsActive = 1)
	 END
	
	RETURN ISNULL(@Result, 0)

END








---- Select [dbo].[fnGetAbsentPenalty] ('93977DFA-335E-4CDC-82C0-D1EF9CB0FECB','2016-12-26','2017-01-25')
 
CREATE FUNCTION [dbo].[fnGetAbsentPenalty] ( 
 
	@EmployeeId uniqueidentifier,
	@FromDate DateTime,
	@ToDate DateTime
)

RETURNS NUMERIC(18,5)
AS BEGIN

				DECLARE @TotalAbsentOTHours NUMERIC(18,5) = 0.0

				SELECT @TotalAbsentOTHours = SUM(OTDeduction) FROM HrmAbsentOTPenalty
											 WHERE EmployeeId = @EmployeeId								  
											 AND CAST(Date AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE)
											 AND IsActive = 1

				RETURN ISNULL(@TotalAbsentOTHours, 0.0);

END





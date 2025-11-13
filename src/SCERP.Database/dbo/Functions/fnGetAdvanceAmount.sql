


CREATE FUNCTION [dbo].[fnGetAdvanceAmount] (  

	@EmployeeId uniqueidentifier,
	@FromDate DateTime,
	@ToDate DateTime
)

RETURNS DECIMAL(18,2)

AS BEGIN
   
   DECLARE @Amount DECIMAL(18,2) = 0
		  
		   SELECT @Amount = SUM(SalaryAdvance.Amount) FROM SalaryAdvance
		   WHERE CAST(SalaryAdvance.ReceivedDate AS date) >= CAST(@FromDate AS date) 
		   AND CAST(SalaryAdvance.ReceivedDate AS date) <= CAST(@ToDate AS date)
		   AND SalaryAdvance.EmployeeId = @EmployeeId
		   AND SalaryAdvance.IsActive = 1
		   		
	IF @Amount IS NULL
		SET @Amount = 0

    RETURN @Amount
END





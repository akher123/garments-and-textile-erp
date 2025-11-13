
-- SELECT [dbo].[fnPreviousEarnLeaveNewJoin]('9A82A462-37E1-4EFA-A5A5-13B967A28348')

CREATE FUNCTION [dbo].[fnPreviousEarnLeaveNewJoin] (  

		  @EmployeeId uniqueidentifier
	
)

RETURNS DECIMAL(18,2)

AS BEGIN
   
   DECLARE @Amount DECIMAL(18,2) = 0
		  
		  SELECT @Amount = COUNT(1) FROM EmployeeInOut 
		  WHERE CAST(TransactionDate AS DATE) BETWEEN '2017-01-01' AND '2017-12-31' 
		  AND CAST(JoiningDate AS DATE) BETWEEN '2017-01-01' AND '2017-12-31'
		  AND Status IN('Present','Late')
		  AND EmployeeId = @EmployeeId
		   		
	IF @Amount IS NULL
		SET @Amount = 0

    RETURN @Amount/18
END
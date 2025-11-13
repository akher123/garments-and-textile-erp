
-- SELECT [dbo].[fnGetBonusPercentage]('7C5A3F42-A5B9-4C74-B0C9-36C1D55A227B','2015-09-15')

CREATE FUNCTION [dbo].[fnGetBonusPercentage_Old] (  
	@EmployeeId uniqueidentifier,
	@Date Datetime
)

RETURNS INT

AS BEGIN
		
	DECLARE @JoiningDate DATETIME;
	SELECT @JoiningDate = JoiningDate FROM Employee WHERE Employee.EmployeeId = @EmployeeId

	DECLARE @ServiceLength INT

	SET @ServiceLength = [dbo].[udfDateDiffinMonths](@JoiningDate,@Date);

	DECLARE @BonusPercentge  NUMERIC(18,2);

	SELECT @BonusPercentge = 
	CASE
	  WHEN @ServiceLength >= 12 THEN 50
	  WHEN @ServiceLength >= 6 THEN 40
	  WHEN @ServiceLength >= 3 THEN 25
	  WHEN @ServiceLength >= 2 THEN 20
	  WHEN @ServiceLength >= 1 THEN 15
	  WHEN @ServiceLength < 1  THEN 10
	END
	
	RETURN @BonusPercentge

END







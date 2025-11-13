-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION dbo.fnGetEmployeeCardId
(
	@EmployeeId uniqueidentifier
)
RETURNS Nvarchar(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result NVARCHAR(100)= '';

	-- Add the T-SQL statements to compute the return value here
	SELECT top(1) @Result = EmployeeCardId FROM Employee
	WHERE EmployeeId = @EmployeeId

	-- Return the result of the function
	RETURN @Result

END

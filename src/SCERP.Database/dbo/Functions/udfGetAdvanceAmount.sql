-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.09.20
-- Description:	<Description, ,>
-- =============================================

-- SELECT dbo.udfGetAdvanceAmount(37000,29)

CREATE FUNCTION [dbo].[udfGetAdvanceAmount]
(
	-- Add the parameters for the function here
	 @grossSalary FLOAT, 
	 @percentage FLOAT
)
RETURNS FLOAT
AS
BEGIN
	
	DECLARE  @payableAMount FLOAT;
	SET @payableAMount = ROUND(((@grossSalary*@percentage)/100.00),0);
	SET @payableAMount = CEILING(@payableAMount/100)*100;
	RETURN @payableAMount

END

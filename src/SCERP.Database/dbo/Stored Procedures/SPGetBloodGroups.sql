-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.8.27
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SPGetBloodGroups]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id,GroupName
	FROM BloodGroup
	WHERE IsActive=1

	UNION ALL
				SELECT        - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY Id
END

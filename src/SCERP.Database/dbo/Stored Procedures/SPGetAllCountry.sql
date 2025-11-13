-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.08.27
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SPGetAllCountry]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id,CountryName
	FROM Country
	WHERE IsActive=1
	UNION ALL
	SELECT - 1 AS Expr1, '<All>' AS Expr2
	ORDER BY Id

END

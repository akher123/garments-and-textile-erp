
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetAllCountry
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetDistrictByCountryId]
	   
			  
				@PermanentCountryId INT

AS
BEGIN
	
				SELECT District.Id, District.Name FROM District
				WHERE District.CountryId = @PermanentCountryId 
				AND District.IsActive = 1

				UNION ALL

				SELECT   - 1 AS Expr1, '<All>' AS Expr2
				ORDER BY District.Id

				SET NOCOUNT ON;
END







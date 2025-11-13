
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPGetComparisonParameter 
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPGetComparisonParameter]
	   						  

AS
BEGIN
	
				SELECT '- 1' AS Id, '<All>' AS Name
						
				UNION ALL
				SELECT  1 AS Id, 'Greater than Equal'				
				UNION ALL
				SELECT  2AS Id, 'Greater than'
				UNION ALL
				SELECT  3 AS Id, 'Less than Equal'
				UNION ALL
				SELECT  4 AS Id, 'Less than' 
				UNION ALL
				SELECT  5 AS Id, 'Equal'



				SET NOCOUNT ON;
END







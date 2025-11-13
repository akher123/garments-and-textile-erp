-- ========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <06/03/2019>
-- Description:	<> EXEC Utility_HighestOTBySection 2020, 01, '2019-12-26'
-- ========================================================================
CREATE PROCEDURE [dbo].[Utility_HighestOTBySection]
					

					   @Year			INT 
					  ,@Month			INT	
					  ,@FromDate		DATE												 
																	 						   
AS

BEGIN
	
				 SET NOCOUNT ON;
					  												 
						
				;WITH TempTable AS
				(
				   SELECT   SectionName
						   ,EmployeeCardId
						   ,([TotalOTHours] + [TotalExtraOTHours] + [TotalWeekendOTHours] + [TotalHolidayOTHours]) AS TotalOT
						   ,ROW_NUMBER() OVER (PARTITION BY SectionName ORDER BY ([TotalOTHours] + [TotalExtraOTHours] + [TotalWeekendOTHours] + [TotalHolidayOTHours]) DESC) AS Ranking
							FROM [dbo].[EmployeeJobCard_NoPenalty]
							WHERE Year = @Year AND Month = @Month AND FromDate = @FromDate
				)

				SELECT SectionName
					  ,EmployeeCardId
					  ,TotalOT
				FROM TempTable
				WHERE Ranking <= 3 AND TotalOT > 0
				ORDER BY SectionName
													 					  					  														  						  											  							
END
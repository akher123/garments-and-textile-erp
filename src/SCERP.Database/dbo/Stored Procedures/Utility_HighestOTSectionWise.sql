-- ================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/06/2019>
-- Description:	<> EXEC Utility_HighestOTSectionWise '2019-10-26', '2019-11-25', 2
-- ================================================================================

CREATE PROCEDURE [dbo].[Utility_HighestOTSectionWise]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						  ,@NoOfEmployee	INT
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  			
																	 
						;WITH TempTable AS
						(
						   SELECT   CompanyName
								   ,CompanyAddress
						           ,SectionName
								   ,EmployeeCardId
								   ,EmployeeName
								   ,([TotalOTHours] + [TotalExtraOTHours] + [TotalWeekendOTHours] + [TotalHolidayOTHours]) AS TotalOT
								   ,ROW_NUMBER() OVER (PARTITION BY SectionName ORDER BY ([TotalOTHours] + [TotalExtraOTHours]  + [TotalHolidayOTHours]) DESC) AS Ranking
									FROM [dbo].[EmployeeJobCard_10PM_NoWeekend]
									WHERE FromDate = @FromDate AND ToDate = @ToDate
									AND BranchUnitId IN(1,2)
						)

						SELECT CompanyName
							  ,CompanyAddress
							  ,SectionName
							  ,EmployeeCardId
							  ,EmployeeName
							  ,TotalOT
						FROM TempTable
						WHERE Ranking <= @NoOfEmployee
						ORDER BY SectionName
													 					  					  														  						  											  							
END
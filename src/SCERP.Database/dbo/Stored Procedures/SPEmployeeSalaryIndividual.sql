
-- =================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-10-06>
-- Description:	<> EXEC [dbo].[SPEmployeeSalaryIndividual] '43CB0E34-9226-451C-A9B1-25C78DA35477', '2018-10-12'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeSalaryIndividual]

								 
								 @EmployeeId UNIQUEIDENTIFIER
								,@UpToDate DATETIME = '1900-01-01'
				
AS

BEGIN

							  SELECT EmployeeCardId							AS EmployeeCardId
									,Year									AS Year
									,ISNULL(SUM(NetAmount), 0)				AS NetSalary
									,ISNULL(SUM(TotalOTAmount), 0)			AS OTAmount
									,ISNULL(SUM(TotalExtraOTAmount), 0)		AS ExtraOTAmount
									,ISNULL(SUM(TotalWeekendOTAmount), 0)	AS WeekendOTAmount
									,ISNULL(SUM(TotalHolidayOTAmount), 0)	AS HolidayOTAmount
									,ISNULL(SUM(TotalBonus), 0)				AS Bonus
							  FROM [dbo].[EmployeeProcessedSalary]
							  WHERE EmployeeId = @EmployeeId
							  GROUP BY EmployeeCardId, Year
							  ORDER BY Year DESC

END
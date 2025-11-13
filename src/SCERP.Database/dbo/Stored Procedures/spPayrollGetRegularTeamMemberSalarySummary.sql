-- ===================================================================
-- Author:		Golam Rabbi
-- Create date: 2016-03-30
-- Description:	To Get Regular Worker's Salary Summary
-- ===================================================================

---- EXEC [spPayrollGetRegularTeamMemberSalarySummary] 1, 1, 2016, 4, '2016-03-26', '2016-04-25'

CREATE PROCEDURE [dbo].[spPayrollGetRegularTeamMemberSalarySummary] 
	@CompanyId INT,
	@BranchId INT,
	@Year INT,
	@Month INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN

	SET NOCOUNT ON;

    SELECT  Unit, 
			Department,  
			Section, 
			Line, 
			EmployeeType,  
			SUM (NetAmount) AS TotalAmount,
			Count(ID) as TotalEmployee,
			(SUM(TotalExtraOTAmount) + SUM (TotalWeekendOTAmount ) + SUM (TotalHolidayOTAmount )) AS ExtraOTWeekendOTandHolidayOT,
			SUM(OTHours) AS TotalOTHours,
			SUM(TotalExtraOTHours) AS TotalExtraOTHours,
			SUM(TotalWeekendOTHours) AS TotalWeekendOTHours,
			SUM(TotalHolidayOTHours) AS TotalHolidayOTHours
			FROM [dbo].[EmployeeProcessedSalary]
			WHERE CompanyId = @CompanyId
			AND BranchId = @CompanyId
			AND [Year] = @Year
			AND [Month] = @Month
			AND FromDate = @FromDate
			AND ToDate = @ToDate
			AND BranchUnitId  IN (1,2)
			AND EmployeeCategoryID = 1
			AND EmployeeTypeId IN (4,5)

	GROUP BY Unit, 
			 Department, 
			 Section, 
			 Line, 
			 EmployeeType

	ORDER BY Unit ASC,
			 Department ASC,
			 Section ASC,
			 Line ASC,
			 EmployeeType ASC
END

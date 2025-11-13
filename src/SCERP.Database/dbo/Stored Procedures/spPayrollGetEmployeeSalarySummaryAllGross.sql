-- ===================================================================
-- Author:		Golam Rabbi
-- Create date: 2016-03-30
-- Description:	To Get Regular Worker's Salary Summary
-- ===================================================================

---- EXEC [spPayrollGetEmployeeSalarySummaryAll] 1, 1, 2016, 4, '2016-03-26', '2016-04-25'

CREATE PROCEDURE [dbo].[spPayrollGetEmployeeSalarySummaryAllGross] 
	@CompanyId INT,
	@BranchId INT,
	@BranchUnitIdList AS dbo.BranchUnitList READONLY,
    @EmployeeTypeIdList AS dbo.EmployeeTypeList READONLY,
	@Year INT,
	@Month INT,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@UserName NVARCHAR(100) = ''	

AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
	DECLARE @ListOfBranchIds TABLE(BranchIDs INT);

	DECLARE @CompanyName NVARCHAR(100) = ''; 
	DECLARE @CompanyAddress NVARCHAR(100) = ''; 
	DECLARE @BranchName NVARCHAR(100) = ''; 

	IF(@CompanyId = -1)
	BEGIN
		INSERT INTO @ListOfCompanyIds
		SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
		WHERE UserName = @UserName;

		SET @CompanyName = '';
		SET @CompanyAddress = '';
	END  
	ELSE
	BEGIN
		INSERT INTO @ListOfCompanyIds VALUES (@CompanyID)
			   
		SELECT @CompanyName= comp.Name, 
				@CompanyAddress = comp.FullAddress
		FROM Company comp 
		WHERE comp.Id = @companyID
	END

	IF(@BranchID = -1)
	BEGIN
		INSERT INTO @ListOfBranchIds
		SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
		WHERE UserName = @UserName;

		SET @BranchName = '';
	END  
	ELSE
	BEGIN
		INSERT INTO @ListOfBranchIds VALUES (@BranchID)
			   
		SELECT @BranchName= brnch.Name
		FROM Branch brnch 
		WHERE brnch.Id = @branchId
	END

    SELECT  
			CASE EmployeeCategoryId
			WHEN 1 THEN 'Regular Employee' 
			WHEN 2 THEN 'Quit Employee'
			WHEN 3 THEN 'New Joining Employee'
			ELSE 'New Joining and Quit Employee' END AS EmployeeCategory,
			SUM (ISNULL(NetAmount,0)) AS NetAmount,
			Count(ID) as TotalEmployee,
			(SUM(ISNULL(TotalExtraOTAmount,0)) + SUM (ISNULL(TotalWeekendOTAmount,0) ) + SUM (ISNULL(TotalHolidayOTAmount,0) )) AS ExtraOTWeekendOTandHolidayOT,
			SUM(ISNULL(OTHours,0)) AS TotalOTHours,
			SUM(ISNULL(TotalOTAmount,0)) AS TotalOTAmount,
			SUM(ISNULL(TotalExtraOTHours,0)) AS TotalExtraOTHours,
			SUM(ISNULL(TotalExtraOTAmount,0)) AS TotalExtraOTAmount,
			SUM(ISNULL(TotalWeekendOTHours,0)) AS TotalWeekendOTHours,
			SUM(ISNULL(TotalWeekendOTAmount,0)) AS TotalWeekendOTAmount,
			SUM(ISNULL(TotalHolidayOTHours,0)) AS TotalHolidayOTHours,
			SUM(ISNULL(TotalHolidayOTAmount,0)) AS TotalHolidayOTAmount,
			(SUM (ISNULL(NetAmount,0)) + SUM(ISNULL(TotalExtraOTAmount,0)) + SUM (ISNULL(TotalWeekendOTAmount,0) ) + SUM (ISNULL(TotalHolidayOTAmount,0) )) AS TotalAmount
			FROM [dbo].[EmployeeProcessedSalary]
			WHERE CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
			AND BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
			AND BranchUnitId  IN (1,2,3)
			AND EmployeeTypeId IN (SELECT EmployeeTypeId FROM @EmployeeTypeIdList)
			AND [Year] = @Year
			AND [Month] = @Month
			AND FromDate = @FromDate
			AND ToDate = @ToDate

	GROUP BY EmployeeCategoryId

	ORDER BY EmployeeCategoryId ASC
END

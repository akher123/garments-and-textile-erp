
-- =====================================================================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.04.18
-- Description:	To Get All Employee Info
-- Exec SPCheckExistingEmployeeCardNumber 'superadmin', 999999, null
-- =====================================================================================================================================================

CREATE PROCEDURE [dbo].[SPCheckExistingEmployeeCardNumber]
	 @UserName NVARCHAR(100),
	 @EmployeeCardId NVARCHAR(100),
	 @FromDate DATETIME = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SQL AS NVARCHAR(1000);
	
	DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
    DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
    DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
    DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
    DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

	INSERT INTO @ListOfCompanyIds
	SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
	WHERE UserName = @UserName;

	INSERT INTO @ListOfBranchIds
	SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
	WHERE UserName = @UserName;

	INSERT INTO @ListOfBranchUnitIds
	SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
	WHERE UserName = @UserName;

	INSERT INTO @ListOfBranchUnitDepartmentIds
	SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
	WHERE UserName = @UserName;

	INSERT INTO @ListOfEmployeeTypeIds
	SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
	WHERE UserName = @UserName;


	SELECT  COUNT(employee.EmployeeId) AS TotalRecords
			FROM Employee AS  employee
			LEFT JOIN (SELECT EmployeeId,DesignationId,BranchUnitDepartmentId,
			ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
			FROM EmployeeCompanyInfo AS employeeCompanyInfo 
			WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
			ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
			LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
			LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
			LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
			LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
			LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
			LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
			LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
			LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
			LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
			LEFT JOIN Company  AS company ON branch.CompanyId = company.Id
			WHERE (employee.IsActive = 1)			
			AND (employeeCompanyInfo.rowNum = 1) 
			AND (company.Id IN (SELECT CompanyIDs FROM @ListofCompanyIds))
			AND (branch.Id IN (SELECT BranchIDs FROM @ListOfBranchIds))
			AND (branchUnit.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds))
			AND (branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds))
			AND (employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds))
			AND ((employee.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
					
END




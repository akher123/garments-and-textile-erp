-- =============================================
-- Author:		Golam Rabbi & Akher
-- Create date: 2015.04.12
-- Description:	To select employee's active company
-- =============================================
CREATE PROCEDURE [dbo].[SPGetEmployeeActiveCompanyInfo]
	@FromDate DATETIME = NULL,
	@EmployeeID UNIQUEIDENTIFIER = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT
		employee.EmployeeCardId AS EmployeeCardId,
		employee.Name AS EmployeeName,
		designation.Title AS Designation ,
		company.Name AS CompanyName,
		branch.Name AS BranchName,
		unit.Name AS UnitName,
		department.Name AS DepartmentName,
		employee.EmployeeId AS EmployeeId ,
		branch.Id AS BranchId,
		branchUnitDepartment.BranchUnitDepartmentId AS BranchUnitDepartmentId,
		branchUnit.BranchUnitId AS BranchUnitId,
		unitDepartment.UnitDepartmentId AS UnitDepartmentId,
		unit.UnitId AS UnitId,
		department.Id AS DepartmentId,
		designation.Id AS DesignationId
		FROM Employee AS  employee
		LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		FROM EmployeeCompanyInfo AS employeeCompanyInfo WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR @FromDate IS NULL)) employeeCompanyInfo ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
		INNER JOIN EmployeeDesignation AS designation ON employeeCompanyInfo.DesignationId=designation.Id
		INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
		INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
		INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
		INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
		INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
		INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
		INNER JOIN Company  AS company ON branch.CompanyId=company.Id
		WHERE employeeCompanyInfo.rowNum = 1 
		AND (employee.IsActive=1)
		AND (employee.[Status] = 1)
		AND ((employee.EmployeeId = @EmployeeID) OR (@EmployeeID IS NULL))
		ORDER BY EmployeeCardId ASC
END




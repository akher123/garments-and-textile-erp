-- =============================================
-- Author:		Golam Rabbi 
-- Create date: 2015.04.12
-- Description:	To select specific employee's info
-- =============================================
CREATE PROCEDURE [dbo].[SPGetSpecificEmployeeActiveInfo]
    @EmployeeID UNIQUEIDENTIFIER = NULL,
	@FromDate DATETIME = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT
		employee.EmployeeId AS EmployeeId,
		employee.EmployeeCardId AS EmployeeCardId,
		employee.Name AS Name,
		employee.NameInBengali AS NameInBengali,
		employee.MothersName AS MothersName,
		employee.MothersNameInBengali AS MothersNameInBengali,
		employee.FathersName AS FathersName,
		employee.FathersNameInBengali AS FathersNameInBengali,
		employee.ReligionId AS ReligionId,
		employee.DateOfBirth AS DateOfBirth,
		employee.GenderId AS GenderId,
		employee.JoiningDate AS JoiningDate,
		employee.ConfirmationDate AS ConfirmationDate,
		employee.PhotographPath AS PhotographPath,
		employeePresentAddress.MailingAddress AS MailingAddress,
		employeePresentAddress.MailingAddressInBengali AS MailingAddressInBengali,
		employeePresentAddress.MobilePhone AS MobilePhone,
		company.Id AS CompanyId,
		branch.Id AS BranchId,
		branchUnit.BranchUnitId AS BranchUnitId,
		employeeCompanyInfo.BranchUnitDepartmentId AS BranchUnitDepartmentId,
		employeeType.Id AS EmployeeTypeId,
		employeeGrade.Id AS EmployeeGradeId,
		employeeCompanyInfo.DesignationId AS EmployeeDesignationId,
		employeeCompanyInfo.IsEligibleForOvertime AS IsEligibleForOvertime,
		employeeCompanyInfo.FromDate AS EffectiveDate
		FROM Employee AS  employee
		LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId,IsEligibleForOvertime,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		FROM EmployeeCompanyInfo AS employeeCompanyInfo 
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
		ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
		LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
		INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
		INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
		INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
		INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
		INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
		INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
		INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
		INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
		INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
		INNER JOIN Company  AS company ON branch.CompanyId=company.Id
		WHERE employeeCompanyInfo.rowNum = 1 
		AND (employee.IsActive = 1)
		AND ((employee.EmployeeId = @EmployeeID) OR (@EmployeeID IS NULL))
		AND (employeePresentAddress.[Status]=1 AND employeePresentAddress.IsActive=1) 
		ORDER BY EmployeeCardId ASC
END




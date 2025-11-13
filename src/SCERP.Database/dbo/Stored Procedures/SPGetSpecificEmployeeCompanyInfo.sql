-- =======================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.04.25
-- Description:	To select specific employee's company
-- EXEC [SCERPDB_DEV].[dbo].[SPGetSpecificEmployeeCompanyInfo] NULL, NULL
-- =======================================================================

--- EXEC [SPGetSpecificEmployeeCompanyInfo] '2015-12-26','6c1fb7e0-51ab-4c5d-87fa-ad6cd7b6bd60',5255


CREATE PROCEDURE [dbo].[SPGetSpecificEmployeeCompanyInfo]
	@FromDate DATETIME = NULL,
	@EmployeeID UNIQUEIDENTIFIER = NULL,
	@EmployeeCompanyInfoId INT = NULL
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;

	SELECT  employeeCompanyInfo.EmployeeId AS EmployeeId, 
			employeeCompanyInfo.EmployeeCompanyInfoId AS EmployeeCompanyInfoId ,
			employeeCompanyInfo.BranchUnitDepartmentId AS BranchUnitDepartmentId,
			branchUnit.BranchUnitId AS BranchUnitId,
			employeeCompanyInfo.DesignationId AS DesignationId,
			employeeCompanyInfo.DepartmentSectionId AS DepartmentSectionId,
			employeeCompanyInfo.DepartmentLineId AS DepartmentLineId,
			employeeCompanyInfo.JobTypeId As JobTypeId,
			employeeCompanyInfo.IsActive AS IsActive,
			company.Name AS CompanyName,
			branch.Name AS BranchName,
			unit.Name AS UnitName,
			department.Name AS DepartmentName,
			section.Name AS SectionName,
			line.Name AS LineName,	
			employeeType.Id AS EmployeeTypeId,
			employeeType.Title AS EmployeeType,
			employeeGrade.Id AS EmployeeGradeId,
			employeeGrade.Name AS EmployeeGrade,
			employeeDesignation.Title AS EmployeeDesignation,
			skillSet.Title AS JobType,
			employeeCompanyInfo.IsEligibleForOvertime AS IsEligibleForOvertime,
			employeeCompanyInfo.PunchCardNo AS PunchCardNo,
			employeeCompanyInfo.FromDate AS FromDate,
			employeeCompanyInfo.ToDate AS ToDate
			FROM EmployeeCompanyInfo AS employeeCompanyInfo 
			LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
			LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
			LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
			LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
			LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
			LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
			LEFT JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
			LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
			LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
			LEFT JOIN Company  AS company ON branch.CompanyId=company.Id
			LEFT JOIN DepartmentSection AS departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
			LEFT JOIN Section AS section ON departmentSection.SectionId = section.SectionId
			LEFT JOIN DepartmentLine AS departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
			LEFT JOIN Line AS line ON departmentLine.LineId = line.LineId
			LEFT JOIN SkillSet skillSet ON employeeCompanyInfo.JobTypeId = skillSet.Id
			WHERE (((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) 
			AND   ((employeeCompanyInfo.EmployeeId = @EmployeeID) OR (@EmployeeID IS NULL))
			AND   ((employeeCompanyInfo.EmployeeCompanyInfoId <> @EmployeeCompanyInfoId) OR (@EmployeeCompanyInfoId IS NULL))
			AND   (employeeCompanyInfo.IsActive = 1))		
			ORDER BY employeeCompanyInfo.FromDate DESC
END




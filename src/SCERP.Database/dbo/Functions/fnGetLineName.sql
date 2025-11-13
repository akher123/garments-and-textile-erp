-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================

-- SELECT dbo.fnGetLineName('149B1D1F-3D9F-465E-8A43-0061166F198C','2015-09-30')

CREATE FUNCTION fnGetLineName
(
	-- Add the parameters for the function here
	@EmployeeId UNIQUEIDENTIFIER,
	@FromDate DATETIME
)
RETURNS NVARCHAR(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @LineName NVARCHAR(100);

	-- Add the T-SQL statements to compute the return value here
	SELECT   @LineName = Line.Name FROM EMPLOYEE 
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId
						 LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
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
						 LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						 LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
						 LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						 LEFT JOIN Line line ON departmentLine.LineId = line.LineId	
						 WHERE 	EMPLOYEE.IsActive = 1 AND EMPLOYEE.Status=1
						 AND employee.EmployeeId = @EmployeeId

	-- Return the result of the function
	RETURN @LineName

END

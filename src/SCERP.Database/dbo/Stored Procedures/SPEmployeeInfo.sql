-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <19/03/2015>
-- Description:	<> exec SPEmployeeInfo 'B13CCFA5-A78D-4BAE-AD1D-1C95B00349E6'
-- =============================================
CREATE PROCEDURE [dbo].[SPEmployeeInfo]

	@employeeId uniqueidentifier = NULL

AS

BEGIN
	
	SET NOCOUNT ON;
			
			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,Company.Name AS CompanyName
						 ,Department.Name AS Department
						 ,Employee.FathersName
						 ,Employee.MothersName
						 ,Employee.SpousesName
						 ,Branch.Name AS Branch
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade
						 ,Line.Name AS Line
						 ,PRE.MobilePhone						 
						 ,EmployeeType.Title AS EmployeeType
						 ,PRE.MailingAddress AS PreMailingAddress
						 ,PRE.PostOffice AS PrePostOffice
						 ,PST.Name AS PrePolice
						 ,DIS.Name AS PreDist
						 ,PER.MailingAddress AS PerMailingAddress
						 ,PER.PostOffice AS PerPostOffice
						 ,PSTE.Name AS PerPolice
						 ,DIST.Name AS PerDist
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate
						 ,Unit.Name AS Unit				
						 ,Section.Name AS Section						 																																	
						 ,EmployeeSalary.BasicSalary
						 ,EmployeeSalary.HouseRent
						 ,EmployeeSalary.MedicalAllowance 
						 ,EmployeeSalary.Conveyance
						 ,EmployeeSalary.FoodAllowance
						 ,EmployeeSalary.GrossSalary
						 ,Line.LineId
						 																												
FROM					 Employee LEFT OUTER JOIN 
						 EmployeePresentAddress PRE ON Employee.EmployeeId = PRE.EmployeeId AND PRE.IsActive = 1 LEFT OUTER JOIN
						 EmployeePermanentAddress PER ON Employee.EmployeeId = PER.EmployeeId AND PER.IsActive = 1 LEFT OUTER JOIN
						 District DIS ON PRE.DistrictId = DIS.Id AND DIS.IsActive = 1 LEFT OUTER JOIN
						 PoliceStation PST ON PRE.PoliceStationId = PST.Id AND PST.IsActive = 1 LEFT OUTER JOIN 
						 District DIST ON PER.DistrictId = DIST.Id AND DIST.IsActive = 1  LEFT OUTER JOIN
						 PoliceStation PSTE ON PER.PoliceStationId = PSTE.Id AND PSTE.IsActive = 1  LEFT OUTER JOIN
						 (SELECT EmployeeId, EmployeeSalary.BasicSalary, EmployeeSalary.HouseRent,EmployeeSalary.MedicalAllowance,EmployeeSalary.Conveyance,EmployeeSalary.FoodAllowance,EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE (CAST(EmployeeSalary.FromDate AS Date) <= GETDATE()) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						 ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNum = 1  
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= GETDATE()) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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
						 LEFT JOIN Gender gender ON employee.GenderId = gender.GenderId

						 WHERE (Employee.EmployeeId = @employeeId) OR (@employeeId IS NULL)
						 AND (PRE.[Status]=1 AND PRE.IsActive=1) 
						   				
						 ORDER BY Employee.EmployeeCardId

END





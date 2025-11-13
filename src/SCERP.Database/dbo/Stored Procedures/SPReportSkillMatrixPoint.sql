
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPReportSkillMatrixPoint]  '0835'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPReportSkillMatrixPoint]
			
							 						
						  @employeeCardId			NVARCHAR(100) = NULL

AS

BEGIN
	
	SET NOCOUNT ON;
			 
			SELECT		  employee.EmployeeId					 AS EmployeeId
						 ,Employee.EmployeeCardId				 AS EmployeeCardId
						 ,Employee.Name							 AS EmployeeName
						 ,employee.FathersName					 AS FathersName
						 ,employee.DateOfBirth					 AS DateOfBirth
						 ,EmployeePresentAddress.MailingAddress  AS Address
						 ,CAST((dbo.udfDateDiffinMonths(employee.JoiningDate, GETDATE())/12) AS DECIMAL(18, 2)) AS YearsOfExperience
						 ,SkillMatrixMachineType.MachineTypeName
						 ,SkillMatrixProcessName.ProcessName
						 ,SkillMatrixDetail.ProcessSmv	
						 ,SkillMatrixDetail.ProcessGrade
						 ,SkillMatrixDetail.AverageCycle
						 ,SkillMatrixProcessName.StandardProcessSmv
						 ,SkillMatrix.Performance
						 ,SkillMatrix.Attitude

						 						 						 						 							 						 							 					 																																																																							  															  														 																											 																											
FROM					 Employee employee
						 LEFT JOIN (SELECT EmployeeSalary.EmployeeId, EmployeeSalary.BasicSalary, EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 						 																			 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE (EmployeeSalary.IsActive=1)) employeeSalary 
						 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 
						 LEFT JOIN EmployeePresentAddress ON EmployeePresentAddress.EmployeeId = employee.EmployeeId
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

						 JOIN SkillMatrix ON employee.EmployeeId = SkillMatrix.EmployeeId AND SkillMatrix.IsActive = 1
						 JOIN SkillMatrixDetail ON SkillMatrixDetail.SkillMatrixId = SkillMatrix.SkillMatrixId
						 LEFT JOIN SkillMatrixProcessName ON SkillMatrixProcessName.ProcessId = SkillMatrixDetail.ProcessId
						 LEFT JOIN SkillMatrixMachineType ON SkillMatrixMachineType.MachineTypeId = SkillMatrixDetail.MachineTypeId
						   						  						       						       
						 WHERE Employee.EmployeeCardId = @employeeCardId	
						 																				
						 ORDER BY Employee.EmployeeCardId

END
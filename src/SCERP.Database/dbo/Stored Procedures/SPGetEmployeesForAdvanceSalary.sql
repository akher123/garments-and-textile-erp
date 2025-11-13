
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <13-Sep-15 2:09:40 PM>
-- Description:	<> EXEC SPGetEmployeesForAdvanceSalary NULL, 1, 1, 1, 2, NULL,NULL, 54, NULL,'2015-09-20'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetEmployeesForAdvanceSalary]
						 @employeeCardId			NVARCHAR(100) = NULL
						,@CompanyId		            INT
						,@BranchId	      	        INT
						,@BranchUnitId		        INT
						,@BranchUnitDepartmentId    INT = NULL
						,@SectionId					INT = NULL
						,@LineId					INT = NULL
						,@Percentage				FLOAT = 0.0
						,@EmployeeTypeId			INT = NULL
						,@ReceivedDate				DATETIME

AS
BEGIN
	
	SET NOCOUNT ON;

			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade
						 ,Department.Name AS Department									
						 ,Section.Name AS Section
						 ,Line.Name AS Line					
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate						 
						 ,EmployeeSalary.BasicSalary						 																																							
						 ,EmployeeSalary.GrossSalary
						 ,dbo.udfGetAdvanceAmount(EmployeeSalary.GrossSalary,@Percentage) AS Amount
						 																												
FROM					 Employee employee
						 LEFT JOIN 													
						 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @ReceivedDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
						 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @ReceivedDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 
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

						 WHERE      
								(company.Id = @CompanyId)
								AND (branch.Id = @BranchId)
								AND (branchUnit.BranchUnitId = @BranchUnitId)
								AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
								AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
								AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
								AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId IS NULL))
								AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))
								AND (employee.IsActive = 1)
								AND (employee.[Status] = 1)
								AND employee.EmployeeId NOT IN 
															(
																SELECT EmployeeId FROM SalaryAdvance salaryAdvance
																WHERE CAST(salaryAdvance.ReceivedDate AS DATE) = @ReceivedDate
																AND salaryAdvance.IsActive = 1
															)
						 ORDER BY Employee.EmployeeCardId

END







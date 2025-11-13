
-- ===========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <20/06/2015>
-- Description:	<> exec SPSalaryAdvance 1, 0, 0, '0035', 5, '06/01/2015','07/10/2015'
-- ===========================================================================


CREATE PROCEDURE [dbo].[SPSalaryAdvance]
				
						 @DepartmentId		        INT
						,@SectionId					INT = NULL
						,@LineId					INT = NULL
						,@employeeCardId			NVARCHAR(100) = NULL 
						,@EmployeeTypeId			INT = NULL
						,@fromDate					DATETIME
						,@toDate					DATETIME

AS
BEGIN
	
	SET NOCOUNT ON;
			
			IF @SectionId = 0
			BEGIN
				SET @SectionId = NULL
			END
			
			IF @LineId = 0
			BEGIN
				SET @LineId = NULL
			END

			IF @employeeCardId = '0'
			BEGIN
				SET @employeeCardId = NULL
			END

			IF @EmployeeTypeId = 0
			BEGIN
				SET @EmployeeTypeId = NULL
			END


			SELECT		  Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,Company.Name AS CompanyName
						 ,Department.Name AS Department									
						 ,Branch.Name AS Branch
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade
						 ,Line.Name AS Line					
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate
						 ,Unit.Name AS Unit				
						 ,Section.Name AS Section						 																																							
						 ,Line.LineId
						 ,SalaryAdvance.SalaryAdvanceId
						 ,SalaryAdvance.Amount
						 ,SalaryAdvance.ReceivedDate
						 																												
FROM					 SalaryAdvance LEFT OUTER JOIN Employee ON SalaryAdvance.EmployeeId = Employee.EmployeeId AND SalaryAdvance.IsActive = 1

						 LEFT OUTER JOIN 									
				
						 (SELECT EmployeeId, EmployeeSalary.BasicSalary, EmployeeSalary.HouseRent,EmployeeSalary.MedicalAllowance,EmployeeSalary.Conveyance,EmployeeSalary.FoodAllowance,EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE (CAST(EmployeeSalary.FromDate AS Date) <= GETDATE()) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						 ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= GETDATE()) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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

						 WHERE      (BranchUnitDepartment.BranchUnitDepartmentId = @DepartmentId OR @DepartmentId IS NULL)
								AND (employeeCompanyInfo.DepartmentSectionId = @SectionId OR @SectionId IS NULL)
								AND (employeeCompanyInfo.DepartmentLineId = @LineId OR @LineId IS NULL)
								AND (Employee.EmployeeCardId = @employeeCardId OR @employeeCardId IS NULL)
								AND (EmployeeType.Id = @EmployeeTypeId OR @EmployeeTypeId IS NULL)
							   	AND SalaryAdvance.ReceivedDate >= @fromDate	AND SalaryAdvance.ReceivedDate <= @toDate		

						 ORDER BY Employee.EmployeeCardId

END






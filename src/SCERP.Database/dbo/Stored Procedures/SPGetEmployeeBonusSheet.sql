
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <15-Sep-15 2:09:40 PM>
-- Description:	<> EXEC SPGetEmployeeBonusSheet '1488', 1, 1, 1, NULL, NULL, NULL, 1 , '2017-08-31'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetEmployeeBonusSheet]
						 @employeeCardId			NVARCHAR(100) = NULL
						,@CompanyId		            INT
						,@BranchId	      	        INT
						,@BranchUnitId		        INT
						,@BranchUnitDepartmentId    INT = NULL
						,@SectionId					INT = NULL
						,@LineId					INT = NULL
						,@EmployeeTypeId			INT 
						,@effectiveDate				DATETIME
						

AS
BEGIN
	
	SET NOCOUNT ON;

			SELECT		  employee.EmployeeId 
						 ,Employee.EmployeeCardId
						 ,Employee.Name 
						 ,Company.Name AS CompanyName
						 ,Company.FullAddress AS CompanyAddress
						 ,Department.Name AS Department									
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit	
						 ,Section.Name AS Section		
						 ,Line.Name AS Line	
						 ,employeeType.Title AS EmployeeType
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade
						 ,CONVERT(VARCHAR(12),Employee.JoiningDate, 106) JoiningDate
						 ,CONVERT(VARCHAR(12),@EffectiveDate, 106) BonusDate							 
						 ,dbo.udfDateDiffinMonths(employee.JoiningDate,@EffectiveDate) AS ServiceLength
						 ,EmployeeSalary.BasicSalary						 																																							
						 ,EmployeeSalary.GrossSalary
						 ,ROUND(EmployeeBonus.Amount,0) AS BonusAmount
						 																												
FROM					 EmployeeBonus LEFT OUTER JOIN Employee ON EmployeeBonus.EmployeeId = Employee.EmployeeId AND EmployeeBonus.IsActive = 1

						 LEFT OUTER JOIN 									
				
						 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						 ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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
								AND (EmployeeType.Id = @EmployeeTypeId)
							   	AND (EmployeeBonus.EffectiveDate = @effectiveDate)	
							    AND (employee.IsActive = 1)
								--AND (employee.[Status] = 1)	

						 ORDER BY Employee.EmployeeCardId

END







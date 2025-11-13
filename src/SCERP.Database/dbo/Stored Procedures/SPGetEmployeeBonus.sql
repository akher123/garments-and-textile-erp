
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <15-Sep-15 2:09:40 PM>
-- Description:	<> EXEC SPGetEmployeeBonus NULL, 2, NULL, NULL, 2, '2015-09-01','2015-09-30'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetEmployeeBonus]
						 @employeeCardId			NVARCHAR(100) = NULL
						,@BranchUnitDepartmentId    INT
						,@SectionId					INT = NULL
						,@LineId					INT = NULL
						,@EmployeeTypeId			INT 
						,@fromDate				    DATETIME
						,@toDate				    DATETIME

						

AS
BEGIN
	
	SET NOCOUNT ON;
		
			IF (@employeeCardId = '0')
				SET @employeeCardId = NULL;

			IF (@SectionId = 0)
				SET @SectionId = NULL;

			IF (@LineId = 0)
				SET @LineId = NULL;


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
						 ,EmployeeDesignation.Title AS Designation						
						 ,EmployeeGrade.Name AS Grade
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate
						 ,CONVERT(VARCHAR(10),EffectiveDate, 103) BonusDate							 
						 ,dbo.udfDateDiffinMonths(employee.JoiningDate,@toDate) AS ServiceLength
						 ,EmployeeSalary.BasicSalary						 																																							
						 ,EmployeeSalary.GrossSalary
						 ,EmployeeBonus.Amount AS BonusAmount
						 ,EmployeeBonus.EmployeeBonusId AS EmployeeBonusId
						  																												
FROM					 EmployeeBonus LEFT OUTER JOIN Employee ON EmployeeBonus.EmployeeId = Employee.EmployeeId AND EmployeeBonus.IsActive = 1

						 LEFT OUTER JOIN 									
				
						 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						 ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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
						 

						 WHERE      
								 (BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) 
								AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
								AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
								AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId IS NULL))
								AND (EmployeeType.Id = @EmployeeTypeId)
							   	AND (EmployeeBonus.EffectiveDate >= @fromDate)
								AND (EmployeeBonus.EffectiveDate <= @toDate)	
								AND (EmployeeBonus.IsActive = 1)
							    AND (employee.IsActive = 1)
								--AND (employee.[Status] = 1)	

						 ORDER BY Employee.EmployeeCardId

END







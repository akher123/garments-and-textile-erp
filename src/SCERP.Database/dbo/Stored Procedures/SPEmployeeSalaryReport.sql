-- =======================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <13/05/2015>
-- Description:	<> exec SPEmployeeSalaryReport  -1, -1, -1, -1, -1, -1, '','', 1000, 0, NULL, 1, -1, -1  
-- =======================================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeSalaryReport]
					

						 @DepartmentId		INT = NULL
						,@SectionId			INT = NULL
					    ,@LineId			INT = NULL
						,@EmployeeTypeId	INT = NULL
						,@EmployeeGradeId	INT = NULL
						,@DesignationId		INT = NULL
						,@EmployeeCardId	NVARCHAR(100) = NULL 
						,@EmployeeName		NVARCHAR(100) = NULL
						,@GrossSalary		DECIMAL(18,2) = 0	
						,@BasicSalary		DECIMAL(18,2) = 0						
						,@EffectiveDate		DATETIME = NULL

						,@GrossComparison   NVARCHAR(100) = NULL					
						,@BasicComparison   NVARCHAR(100) = NULL					
						,@EffectiveComparison   NVARCHAR(100) = NULL
					
AS
BEGIN
	
	SET NOCOUNT ON;
	

			DECLARE 	 @GrossFrom 		DECIMAL(18,2) = 0
						,@GrossTo 			DECIMAL(18,2) = 0
						,@BasicFrom 		DECIMAL(18,2) = 0
						,@BasicTo 			DECIMAL(18,2) = 0
						,@EffectiveFrom 	DATETIME = NULL
						,@EffectiveTo 		DATETIME = NULL

				 
			     IF(@GrossComparison = '1')			SET @GrossFrom = @GrossSalary		-- greaterthanequal
			ELSE IF(@GrossComparison = '2')			SET @GrossFrom = @GrossSalary + 1	-- greaterthan
			ELSE IF(@GrossComparison = '3')			SET @GrossTo = @GrossSalary			-- lessthanequal
			ELSE IF(@GrossComparison = '4')			SET @GrossTo = @GrossSalary	- 1		-- lessthan
			ELSE IF(@GrossComparison = '5')												-- equal
			BEGIN
					SET @GrossFrom = @GrossSalary
					SET @GrossTo = @GrossSalary
			END			
			ELSE IF(@GrossComparison IS NULL)
			BEGIN
					SET @GrossFrom = 0
					SET @GrossTo = 0
			END	
				

				 IF(@BasicComparison = '1')			SET @BasicFrom = @BasicSalary			-- greaterthanequal
			ELSE IF(@BasicComparison = '2')			SET @BasicFrom = @BasicSalary + 1		-- greaterthan
			ELSE IF(@BasicComparison = '3')			SET @BasicTo = @BasicSalary				-- lessthanequal
			ELSE IF(@BasicComparison = '4')			SET @BasicTo = @BasicSalary - 1			-- lessthan
			ELSE IF(@BasicComparison = '5')													-- equal	
			BEGIN
					SET @BasicFrom = @BasicSalary
					SET @BasicTo = @BasicSalary
			END		
			ELSE IF(@BasicComparison IS NULL)		
			BEGIN
					SET @BasicFrom = 0
					SET @BasicTo = 0
			END		


			IF(@EffectiveDate IS NULL OR @EffectiveComparison IS NULL) 
			BEGIN
					SET @EffectiveFrom = NULL
					SET @EffectiveTo = NULL
			END
			ELSE IF(@EffectiveComparison = '1')		SET @EffectiveFrom =  @EffectiveDate					-- greaterthanequal
			ELSE IF(@EffectiveComparison = '2')		SET @EffectiveFrom =  DATEADD(day, 1, @EffectiveDate)   -- greaterthan
			ELSE IF(@EffectiveComparison = '3')		SET @EffectiveTo = @EffectiveDate						-- lessthanequal
			ELSE IF(@EffectiveComparison = '4')		SET @EffectiveTo = DATEADD(day, -1, @EffectiveDate)		-- lessthan
			ELSE IF(@EffectiveComparison = '5')																-- equal
			BEGIN
					SET @EffectiveFrom = @EffectiveDate
					SET @EffectiveTo = @EffectiveDate
			END	
									
																												  
			SELECT		  Company.Name AS CompanyName
						 ,Branch.Name AS Branch
						 ,Unit.Name AS Unit	
						 ,Department.Name AS Department
						 ,Section.Name AS Section
						 ,Line.Name	AS Line	
						 ,EmployeeType.Title AS EmployeeType
						 ,EmployeeGrade.Name AS Grade
						 ,EmployeeDesignation.Title AS Designation
						 ,Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,Employee.Name 					
						 ,EmployeeSalary_Processed.GrossSalary
						 ,EmployeeSalary_Processed.BasicSalary	
						 ,EmployeeSalary_Processed.MedicalAllowance														 
						 ,EmployeeSalary_Processed.HouseRent
						 ,EmployeeSalary_Processed.FoodAllowance
						 ,EmployeeSalary_Processed.Conveyance
						 ,EmployeeSalary_Processed.Tax
						 ,EmployeeSalary_Processed.ProvidentFund
						 ,EmployeeSalary_Processed.NetSalaryPaid
						 ,EmployeeSalary_Processed.Comments
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.FromDate, 103) AS FromDate		
						 ,CONVERT(VARCHAR(10),EmployeeSalary_Processed.ToDate, 103) AS ToDate						 						 
						 ,Company.FullAddress AS CompanyAddress														 																																	
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) AS JoiningDate								 												 						 															 						 												 																					
						 ,EmployeeSalary_Processed.IsActive AS IsActive
						 				
	FROM				EmployeeSalary_Processed	LEFT OUTER JOIN 																										
						Employee employee ON EmployeeSalary_Processed.EmployeeId = Employee.EmployeeId 
						AND employee.IsActive = 1  													 					
						AND employee.[Status] = 1
                        LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE (((CAST(employeeCompanyInfo.FromDate AS Date) >= @EffectiveFrom) OR (@EffectiveFrom IS NULL)) AND ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveTo) OR (@EffectiveTo IS NULL)))
						AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
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
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
					
					    WHERE EmployeeSalary_Processed.IsActive = 1 					
						AND (employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1) 
						AND employeeCompanyInfo.rowNum = 1

						AND (Department.Id = @DepartmentId OR @DepartmentId  = -1)
						AND (Section.SectionId = @SectionId OR @SectionId = -1)
						AND (Line.LineId = @LineId OR @LineId = -1)
						AND (EmployeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)
						AND (EmployeeGrade.Id = @EmployeeGradeId OR @EmployeeGradeId = -1)
						AND (EmployeeDesignation.Id = @DesignationId OR @DesignationId = -1 )		
						AND (Employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')
						AND (employee.Name LIKE '%' + @EmployeeName + '%' OR @EmployeeName ='')	

						AND ((EmployeeSalary_Processed.GrossSalary >= @GrossFrom OR @GrossFrom = 0) 
								AND (EmployeeSalary_Processed.GrossSalary <= @GrossTo OR @GrossTo = 0))

						AND ((EmployeeSalary_Processed.BasicSalary >= @BasicFrom OR @BasicFrom = 0) 
								AND (EmployeeSalary_Processed.BasicSalary <= @BasicTo OR @BasicTo = 0))

					    ORDER BY employee.EmployeeCardId
END





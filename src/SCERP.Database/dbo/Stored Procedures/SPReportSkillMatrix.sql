
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPReportSkillMatrix]  NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0835'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPReportSkillMatrix]
			
							 
							 @CompanyId		            INT = NULL
							,@BranchId	      	        INT = NULL
							,@BranchUnitId		        INT = NULL
							,@BranchUnitDepartmentId    INT = NULL
							,@SectionId					INT = NULL
							,@LineId					INT = NULL
							,@EmployeeTypeId			INT = NULL
							,@employeeCardId			NVARCHAR(100) = NULL

AS

BEGIN
	
	SET NOCOUNT ON;
			 
			SELECT		  employee.EmployeeId				AS EmployeeId
						 ,Employee.EmployeeCardId			AS EmployeeCardId
						 ,Employee.Name						AS EmployeeName
						 ,Line.Name							AS Line		
						 ,EmployeeDesignation.Title			AS Designation						
						 ,EmployeeGrade.Name				AS Grade
						 ,Department.Name					AS Department									
						 ,Section.Name						AS Section									
						 ,CONVERT(VARCHAR(10), Employee.JoiningDate, 103) AS JoiningDate
						 ,EmployeeSalary.GrossSalary
						 ,SkillMatrixProcessName.ProcessName
						 ,SkillMatrixMachineType.MachineTypeName
						 ,SkillMatrixDetail.ProcessGrade	
						 ,SkillMatrixDetail.ProcessSmv	
						 ,SkillMatrixDetail.AverageCycle	 
						 ,CAST((dbo.udfDateDiffinMonths(employee.JoiningDate, GETDATE())/12) AS DECIMAL(18, 2)) AS YearsOfExperience					 																																																		
						 ,'2%' AS [A2]
						 ,(SELECT COUNT( DISTINCT [MachineTypeId])          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId) AS NoOfMachineSkill

						 ,'20%' AS [A20]
						 ,(SELECT COUNT(DISTINCT ProcessId)          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId)  AS TotalProcessSkill

						,'15%' AS [A15]
						,(SELECT COUNT(DISTINCT ProcessId)          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId AND [SkillMatrixDetail].ProcessGrade = 'A') AS AGradeProcess

						 ,(SELECT COUNT(DISTINCT ProcessId)          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId AND [SkillMatrixDetail].ProcessGrade = 'B') AS BGradeProcess	
								  			
						 ,(SELECT COUNT(DISTINCT ProcessId)          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId AND [SkillMatrixDetail].ProcessGrade = 'C') AS CGradeProcess	
								   			
						 ,(SELECT COUNT(DISTINCT ProcessId)          
								  FROM [dbo].[SkillMatrixDetail]
								  JOIN SkillMatrix ON SkillMatrix.SkillMatrixId = [SkillMatrixDetail].SkillMatrixId
								  WHERE SkillMatrix.EmployeeId = employee.EmployeeId AND [SkillMatrixDetail].ProcessGrade = 'D') AS DGradeProcess

						 ,'15%' AS [B15]
						 ,'' AS Performance
						 ,'40%' AS [B40]
						 ,'' AS Others
						 ,'5%' AS [B5]
						 ,'' AS PresentGrade
						 ,'' AS Remarks
						 			
																								 																											
FROM					 Employee employee
						 LEFT JOIN (SELECT EmployeeSalary.EmployeeId, EmployeeSalary.BasicSalary, EmployeeSalary.GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 						 																			 
						 FROM EmployeeSalary AS EmployeeSalary 
						 WHERE (EmployeeSalary.IsActive=1)) employeeSalary 
						 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

						 LEFT JOIN (SELECT EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
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
						 JOIN SkillMatrix ON employee.EmployeeId = SkillMatrix.EmployeeId AND SkillMatrix.IsActive = 1
						 JOIN SkillMatrixDetail ON SkillMatrixDetail.SkillMatrixId = SkillMatrix.SkillMatrixId
						 LEFT JOIN SkillMatrixProcessName ON SkillMatrixProcessName.ProcessId = SkillMatrixDetail.ProcessId
						 LEFT JOIN SkillMatrixMachineType ON SkillMatrixMachineType.MachineTypeId = SkillMatrixDetail.MachineTypeId
						  						  
						 WHERE      
								(company.Id = @CompanyId OR @CompanyId IS NULL)
								AND (branch.Id = @BranchId OR @BranchId IS NULL)
								AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
								AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
								AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
								AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
								AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId IS NULL))
								AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))
								AND (employee.IsActive = 1)
								AND (employee.[Status] = 1)							
								AND employeeType.Id <> 1
								AND branchUnit.BranchUnitId IN (1,2) 
								AND (departmentSection.DepartmentSectionId <> 35 OR departmentSection.DepartmentSectionId IS NULL)
								
						 ORDER BY Employee.EmployeeCardId

END
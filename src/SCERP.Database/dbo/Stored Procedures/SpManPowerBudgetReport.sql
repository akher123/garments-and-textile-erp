-- ========================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <09/09/2018>
-- Description:	<> EXEC SpManPowerBudgetReport  1, 1, 1, 6, 6, NULL, NULL, '', '2017-10-11'
-- =========================================================================================

CREATE PROCEDURE [dbo].[SpManPowerBudgetReport]
									
									
									 @CompanyId		            INT = NULL
									,@BranchId	      	        INT = NULL
									,@BranchUnitId		        INT = NULL
									,@BranchUnitDepartmentId    INT = NULL
									,@SectionId					INT = NULL
									,@LineId					INT = NULL
									,@EmployeeTypeId			INT = NULL
									,@employeeCardId			NVARCHAR(100) = NULL
								    ,@EffectiveDate				DATETIME 
															
AS
								 
BEGIN
	
			SET NOCOUNT ON;
													
    
						DECLARE	  @CheckDate DATETIME = '2018-09-09'			 	
				
								  CREATE TABLE #TableTemp 
								  ( 
										Department		NVARCHAR(100)
									   ,DepartmentId	INT
									   ,Section			NVARCHAR(100)
									   ,SectionId		INT
									   ,Line			NVARCHAR(100)
									   ,LineId			INT
									   ,TotalEmployee	INT
									   ,BudgetEmployee	INT
								  ) 
								  INSERT INTO #TableTemp
								  (
										Department		
									   ,DepartmentId	
									   ,Section			
									   ,SectionId		
									   ,Line			
									   ,LineId			
									   ,TotalEmployee
									   ,BudgetEmployee			
								  )			 
					 						 						 
			SELECT		  Department.Name			AS Department	
						 ,Department.Id				AS DepartmentId								
						 ,Section.Name				AS Section					
						 ,departmentSection.DepartmentSectionId			AS SectionId
						 ,line.Name					AS Line
						 ,line.LineId				AS LineId	
						 ,COUNT(1)					AS TotalEmployee
						 ,0						
						 					 																											 					
			FROM		 Employee employee						 
						 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @CheckDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
						 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

						 LEFT JOIN (SELECT EmployeeId,  WorkGroupId, AssignedDate,
						 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY AssignedDate DESC) AS rowNumWG
						 FROM EmployeeWorkGroup AS employeeWorkGroup 
					     WHERE (CAST(employeeWorkGroup.AssignedDate AS Date) <= @CheckDate) AND employeeWorkGroup.IsActive=1) employeeWorkGroup 
					     ON employee.EmployeeId = employeeWorkGroup.EmployeeId
						 LEFT JOIN WorkGroup workGroup ON employeeWorkGroup.WorkGroupId = workGroup.WorkGroupId
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
						  						       								 
						 WHERE employee.IsActive = 1
						 AND employee.Status = 1
						 AND branchUnit.BranchUnitId IN(1,2)
						 AND EmployeeCardId NOT IN ('0001','0003','0004','999999','999998')
						 GROUP BY Department.Name
								 ,Department.Id
						         ,Section.Name
						         ,departmentSection.DepartmentSectionId
								 ,line.LineId	
								 ,line.Name
						
						-- Table update by Line
						UPDATE #TableTemp   
						SET BudgetEmployee = (SELECT BudgetEmployee FROM ManPowerBudgetByLine WHERE ManPowerBudgetByLine.LineId = #TableTemp.LineId)

						-- Table update by Section
						UPDATE #TableTemp
						SET BudgetEmployee = (SELECT BudgetEmployee FROM ManPowerBudgetBySection WHERE ManPowerBudgetBySection.sectionId = #TableTemp.SectionId)
						WHERE #TableTemp.LineId IS NULL

						SELECT 	 Department		
								,DepartmentId	
								,Section			
								,SectionId		
								,Line			
								,LineId			
								,TotalEmployee
								,BudgetEmployee	
								,(TotalEmployee - BudgetEmployee) AS Balance
							FROM #TableTemp
					 									 													  					  														  						  											  							
END
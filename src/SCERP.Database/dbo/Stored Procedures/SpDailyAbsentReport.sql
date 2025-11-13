-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SpDailyAbsentReport  1, 1, 1, 6, 6, NULL, NULL, '', '2017-10-11'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SpDailyAbsentReport]
									
									
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
													
    
				SELECT				  Employee.EmployeeCardId	
									 ,Employee.Name					AS EmployeeName		
									 ,EmployeeDesignation.Title		AS Designation										
									 ,Department.Name				AS Department	
									 ,Section.Name					AS Section	
									 ,Line.Name						AS Line
									 ,EmployeeType.Title			AS Type																	
									 ,@EffectiveDate				AS Date							
									 ,EmployeePresentAddress.MobilePhone
									 ,'' AS Remarks
								
						 																 																 																												
			FROM					 Employee employee
									 LEFT JOIN 													
									 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
									 FROM EmployeeSalary AS EmployeeSalary 
									 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
									 ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

									 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
									 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
									 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
									 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 			
									 LEFT JOIN EmployeePresentAddress ON EmployeePresentAddress.EmployeeId = Employee.EmployeeId 
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
									(company.Id = @CompanyId OR @CompanyId IS NULL)
									AND (branch.Id = @BranchId OR @BranchId IS NULL)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
									AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
									AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))
									AND (employee.IsActive = 1)
									AND (employee.[Status] = 1)
							
									AND employeeType.Id <> 1								--- 1 for Management Committee
									AND branchUnit.BranchUnitId IN (1,2)					--- 1 for Garments, 2 for Knitting
									AND departmentSection.DepartmentSectionId NOT IN (35)	--- Not for security
									AND CAST(EmployeeCardId AS INT) NOT IN (

											 SELECT CAST([EmployeeCardId] AS INT)										       
										     FROM [dbo].[EmployeeDailyAttendance]
										     WHERE CAST(TransactionDateTime AS DATE) = CAST(@EffectiveDate AS DATE)
										     GROUP BY [EmployeeCardId]
									)				
									AND employee.EmployeeId NOT IN 
									(
										SELECT EmployeeId FROM EmployeeLeaveDetail WHERE CAST(ConsumedDate AS DATE) = CAST(@EffectiveDate AS DATE)
									)
									ORDER BY Employee.EmployeeCardId	
					 									 													  					  														  						  											  							
END
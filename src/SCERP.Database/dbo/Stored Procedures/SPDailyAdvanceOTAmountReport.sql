-- ======================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SPDailyAdvanceOTAmountReport  0, 0, 0, 0, 0, 0, '2017-12-03'
-- ======================================================================================

CREATE PROCEDURE [dbo].[SPDailyAdvanceOTAmountReport]
	
									
									 @CompanyId		            INT = 0
									,@BranchId	      	        INT = 0
									,@BranchUnitId		        INT = 0
									,@BranchUnitDepartmentId    INT = 0
									,@SectionId					INT = 0
									,@LineId					INT = 0					
								    ,@EffectiveDate				DATETIME 				
															
AS
								 
BEGIN
	
			    SET NOCOUNT ON;
																																																											
								SELECT DepartmentName
									  ,SectionName
									  ,LineName
									  ,COUNT(1) AS NumberOfPerson
									  ,COUNT(1) * 2 AS OTHours
									  ,CAST(2 * SUM(employeeSalary.BasicSalary/208.00 * 2) AS DECIMAL(18,2)) AS OTAmount
									  ,CAST(SUM(employeeSalary.GrossSalary/30) AS DECIMAL(18,2)) AS Salary
									  ,CAST(2 * SUM(employeeSalary.BasicSalary/208.00 * 2) AS DECIMAL(18,2)) + CAST(SUM(employeeSalary.GrossSalary/30) AS DECIMAL(18,2)) AS TotalAmount	
																																																			 																 																												
						FROM	     EmployeeInOut
									 LEFT JOIN 													
									 (SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
									 FROM EmployeeSalary AS EmployeeSalary 
									 WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @EffectiveDate) AND EmployeeSalary.IsActive=1)) employeeSalary 
									 ON EmployeeInOut.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  

									 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
									 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
									 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @EffectiveDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
									 ON EmployeeInOut.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
						 			
									 LEFT JOIN EmployeePresentAddress ON EmployeePresentAddress.EmployeeId = EmployeeInOut.EmployeeId 
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
									 LEFT JOIN EmployeeWorkShift ON EmployeeWorkShift.EmployeeId = EmployeeInOut.EmployeeId AND CAST(EmployeeWorkShift.ShiftDate AS DATE) = CAST(@EffectiveDate AS DATE)
						       
							WHERE	(company.Id = @CompanyId OR @CompanyId = 0)
									AND (branch.Id = @BranchId OR @BranchId = 0)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId = 0))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId = 0))						
							
									AND CAST(TransactionDate AS DATE) = @EffectiveDate 
									AND EmployeeInOut.Status <> 'Absent'
									AND EmployeeInOut.EmployeeTypeId IN (4,5)

									GROUP BY DepartmentName, SectionName, LineName, EmployeeInOut.DepartmentLineId
									ORDER BY EmployeeInOut.DepartmentLineId				
																					 									 													  					  														  						  											  							
END
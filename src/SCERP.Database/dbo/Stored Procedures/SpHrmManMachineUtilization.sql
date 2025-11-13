-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC SpHrmManMachineUtilization '2016-11-03'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SpHrmManMachineUtilization]
	
		
								@EffectiveDate	DATETIME 
															
AS
								 

BEGIN
	
				SET NOCOUNT ON;

				DECLARE			 @employeeCardId			NVARCHAR(100) = NULL
								,@CompanyId		            INT = NULL
								,@BranchId	      	        INT = NULL
								,@BranchUnitId		        INT = NULL
								,@BranchUnitDepartmentId    INT = 6
								,@SectionId					INT = 6
								,@LineId					INT = NULL
								,@EmployeeTypeId			INT = 4
																						
    
	SELECT				 												
								  Department.Name				AS DepartmentName
								 ,EmployeeType.Title			AS EmployeeType															 
								 ,CASE WHEN EmployeeDesignation.Title = 'Assistant Operator' OR EmployeeDesignation.Title ='Line Iron Man'											  												
								  THEN 'Sewing Helper' ELSE 'Sewing Operator' END AS Designation
								 								 				
								 ,CONVERT(NVARCHAR(12), @EffectiveDate, 106) AS EffectiveDate
								 ,COUNT(1)									 AS Total
								 ,COUNT(CASE WHEN EmployeeInOut.InTime IS NOT NULL OR EmployeeInOut.OutTime IS NOT NULL THEN EmployeeInOut.Status END)          AS Present							
						 		 ,(COUNT(1) - COUNT(CASE WHEN EmployeeInOut.Status = 'Present' OR EmployeeInOut.Status = 'Late' THEN EmployeeInOut.Status END)) AS Absent						
								 ,(COUNT(CASE WHEN EmployeeInOut.Status = 'Present' OR EmployeeInOut.Status = 'Late' THEN EmployeeInOut.Status END) * 8)
								 + SUM(ISNULL(EmployeeInOut.OTHours, 0))
								 + SUM(ISNULL(ExtraOTHours, 0)) 
								 + SUM(ISNULL(WeekendOTHours, 0)) 
								 + SUM(ISNULL(HolidayOTHours, 0)) AS WorkingHours
													 												 																 																												
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
						 
								 LEFT JOIN EmployeeInOut ON EmployeeInOut.EmployeeId = Employee.EmployeeId
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
								AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId IS NULL))
								AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))
								AND (employee.IsActive = 1)
								AND (employee.[Status] = 1)
								
								AND employeeType.Id <> 1	 -- 1 for Management Committee
								AND branchUnit.BranchUnitId IN (1,2) --- 1 for Garments, 2 for Knitting
								AND departmentSection.DepartmentSectionId NOT IN (35) ---35 for security
								AND Line.Name IS NOT NULL
								AND EmployeeDesignation.Id NOT IN (114,136)
								AND CAST(EmployeeInOut.TransactionDate AS DATE) = @EffectiveDate
							
								GROUP BY CASE WHEN (EmployeeDesignation.Title = 'Assistant Operator' OR EmployeeDesignation.Title ='Line Iron Man') THEN 'Sewing Helper'											  												
											   
											  ELSE 'Sewing Operator' END
											 ,EmployeeType.Title, Department.Name	
											 
							    ORDER BY 	Total DESC				 									 								
					  					  														  						  											  							
END
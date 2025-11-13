-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/09/2016>
-- Description:	<> EXEC SpDailyAttendanceByDesignationReport  1, 1, 1, NULL, NULL, NULL, NULL, '', '2017-10-21'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SpDailyAttendanceByDesignationReport]
	
									
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
					
						TRUNCATE TABLE 	EmployeeDailyAttendance2

						INSERT INTO [dbo].[EmployeeDailyAttendance2]
							   ([EmployeeId]
							   ,[EmployeeCardId]
							   ,[TransactionDateTime]
							   ,[FunctionKey]
							   ,[IsFromMachine]
							   ,[Remarks]
							   ,[CreatedDate]
							   ,[CreatedBy]
							   ,[EditedDate]
							   ,[EditedBy]
							   ,[IsActive])
						SELECT 
							   [EmployeeId]
							  ,[EmployeeCardId]
							  ,[TransactionDateTime]
							  ,[FunctionKey]
							  ,[IsFromMachine]
							  ,[Remarks]
							  ,[CreatedDate]
							  ,[CreatedBy]
							  ,[EditedDate]
							  ,[EditedBy]
							  ,[IsActive]
						  FROM [dbo].[EmployeeDailyAttendance]
						  WHERE CAST([TransactionDateTime] AS DATE) = CAST(@EffectiveDate AS DATE) AND IsActive = 1
																	    
				SELECT				  Employee.EmployeeCardId	
									 ,Employee.Name					AS EmployeeName		
									 ,EmployeeDesignation.Title		AS Designation										
									 ,Department.Name				AS Department	
									 ,Section.Name					AS Section	
									 ,Line.Name						AS Line
									 ,EmployeeType.Title			AS Type																	
									 ,@EffectiveDate				AS Date							
									 ,EmployeePresentAddress.MobilePhone
									 ,[dbo].[fnGetAttendanceStatusShort] (Employee.EmployeeId, @EffectiveDate,EmployeeWorkShift.EmployeeWorkShiftId) AS Remarks
									 ,Gender.Title AS Gender
						 																 																 																												
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
									 LEFT JOIN EmployeeWorkShift ON EmployeeWorkShift.EmployeeId = employee.EmployeeId AND CAST(EmployeeWorkShift.ShiftDate AS DATE) = CAST(@EffectiveDate AS DATE)
						       
							WHERE	(company.Id = @CompanyId OR @CompanyId IS NULL)
									AND (branch.Id = @BranchId OR @BranchId IS NULL)
									AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
									AND ((BranchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId IS NULL))
									AND ((employeeCompanyInfo.DepartmentSectionId = @SectionId) OR (@SectionId IS NULL))
									AND ((employeeCompanyInfo.DepartmentLineId = @LineId) OR (@LineId IS NULL))
									AND ((Employee.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
									AND ((EmployeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId IS NULL))	
									AND EmployeeWorkShift.IsActive = 1								
									AND (employee.IsActive = 1)
									AND (employee.QuitDate IS NULL OR employee.QuitDate >= @EffectiveDate)
									AND employee.JoiningDate <= @EffectiveDate
							
									AND employeeType.Id NOT IN (1,2,3)						--- 1 for Management Committee and other Officers
									AND branchUnit.BranchUnitId IN (1,2)					--- 1 for Garments, 2 for Knitting
									AND departmentSection.DepartmentSectionId NOT IN (35)	--- Not for security
												
									ORDER BY Employee.EmployeeCardId	
					 									 													  					  														  						  											  							
END
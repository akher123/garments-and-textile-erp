
-- ======================================================================================================================
-- Author		:  <Yasir Arafat>
-- Create date  :  <2017-01-08>
-- Description  :  EXEC SPAbsentOtPenaltySave  '2017-01-01', ''
-- ======================================================================================================================

CREATE PROCEDURE [dbo].[SPAbsentOtPenaltySave]
					
								  @FromDate DateTime
								 ,@EmployeeList AS dbo.EmployeeList READONLY						

AS
BEGIN
	
	SET NOCOUNT ON;
																						
					IF(@fromDate IS NULL)
					BEGIN
						SET @fromDate = CAST(CURRENT_TIMESTAMP AS DATE)
					END
					ELSE
					BEGIN
						SET @fromDate = CAST(@fromDate AS DATE)
					END

					DELETE FROM [dbo].[HrmAbsentOTPenalty]
					WHERE Date = CAST(@FromDate AS DATE)	        												
						  						
																
					INSERT INTO [dbo].[HrmAbsentOTPenalty]
									   ([EmployeeId]
									   ,[EmployeeCardId]
									   ,[EmployeeName]

									   ,[Designation]
									   ,[Department]
									   ,[Section]
									   ,[Line]

									   ,[EmployeeType]
									   ,[JoinDate]
									   ,[Date]
									   ,[Amount]
									   ,[OTDeduction]
									   ,[CreatedDate]																	
									   ,[IsActive]
									   )

									SELECT		  
										  Employee.EmployeeId 
										 ,Employee.EmployeeCardId 
										 ,Employee.Name
										 ,EmployeeDesignation.Title 
										 ,Department.Name
										 ,Section.Name
										 ,Line.Name
										 ,employeeType.Title
										 ,CAST(employee.JoiningDate AS DATE)
										 ,CAST(@FromDate AS DATE)
										 ,CONVERT(DECIMAL(18,5), ((employeeSalaryInfo.GrossSalary/30) - (employeeSalaryInfo.BasicSalary/30)))
										 ,CONVERT(DECIMAL(18,5), (((employeeSalaryInfo.GrossSalary/30) - (employeeSalaryInfo.BasicSalary/30))/(employeeSalaryInfo.BasicSalary/104)))
										 ,CURRENT_TIMESTAMP AS DATE
										 ,1																						
					 																							 					
							FROM		 Employee employee
										 LEFT JOIN EmployeeInOut ON EmployeeInOut.EmployeeId = employee.EmployeeId AND CAST(EmployeeInOut.TransactionDate AS DATE) = @fromDate

										 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
										 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
										 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
										 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @fromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
										 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1

										 LEFT JOIN (SELECT EmployeeId,GrossSalary,BasicSalary,HouseRent,MedicalAllowance,FoodAllowance,Conveyance,EntertainmentAllowance, FromDate,
										 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
										 FROM EmployeeSalary AS employeeSalary
										 WHERE ((CAST(employeeSalary.FromDate AS Date) <= @fromDate) OR (@fromDate IS NULL))
										 AND employeeSalary.IsActive = 1) employeeSalaryInfo 
										 ON employee.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1

										 LEFT JOIN (SELECT EmployeeId,  WorkGroupId, AssignedDate,
										 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY AssignedDate DESC) AS rowNumWG
										 FROM EmployeeWorkGroup AS employeeWorkGroup 
										 WHERE (CAST(employeeWorkGroup.AssignedDate AS Date) <= @fromDate) AND employeeWorkGroup.IsActive=1) employeeWorkGroup 
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

										 WHERE Employee.EmployeeId IN (SELECT EmployeeId FROM @EmployeeList)  
														 		 						
										 ORDER BY Employee.EmployeeCardId

END







-- ================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPEarnLeaveSheet2 -1, -1, -1, -1, -1, -1, -1, -1, -1, '', 'superadmin', '2018-01-01', '2018-12-31', '', 1
-- ================================================================================================================================

CREATE PROCEDURE [dbo].[SPEarnLeaveSheet2]
			
								    
							     @CompanyId						INT = -1
	                            ,@BranchId						INT = -1
	                            ,@BranchUnitId					INT = -1
	                            ,@BranchUnitDepartmentId		INT = -1
	                            ,@DepartmentSectionId			INT = -1
	                            ,@DepartmentLineId				INT = -1
	                            ,@EmployeeTypeId				INT = -1
	                            ,@EmployeeGradeId				INT = -1
	                            ,@EmployeeDesignationId			INT = -1	                      
	                            ,@EmployeeName					NVARCHAR(100) = NULL	          	     
	                            ,@UserName						NVARCHAR(100) = NULL
								,@fromDate						DATETIME = NULL
						        ,@toDate						DATETIME = NULL
								,@EmployeeCardId				NVARCHAR(100) = ''
	                  	        ,@ActiveStatus					INT = -1          	                     	                 	                   								 								 	
AS

BEGIN
	
							 SET NOCOUNT ON;


							 INSERT INTO [dbo].[EarnLeaveSummary]
								   ([EmployeeId]
								   ,[EmployeeCardId]
								   ,[EmployeeName]
								   ,[EmployeeDesignation]
								   ,[EmployeeTypeId]
								   ,[EmployeeType]
								   ,[DepartmentId]
								   ,[Department]
								   ,[SectionId]
								   ,[SectionName]
								   ,[LineId]
								   ,[LineName]
								   ,[GrossSalary]
								   ,[Amount]
								   ,[JoiningDate]
								   ,[ExecuteDate]
								   ,[CreateDate]
								   ,[CreatedBy]
								   ,[IsActive])

							 SELECT 
							 Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.Name								AS Employeename
						    ,EmployeeDesignation.Title					AS Designation
							,employeeType.Id
							,EmployeeType.Title							AS EmployeeType	
							,branchUnitDepartment.BranchUnitDepartmentId
							,Department.Name		  					AS DepartmentName
							,departmentSection.DepartmentSectionId
							,Section.Name								AS SectionName
							,departmentLine.DepartmentLineId
							,line.Name									AS LineName
							,EmployeeSalary.GrossSalary					AS GrossSalary
							,CAST((employeeSalary.GrossSalary/30 * CAST((CAST(CAST((SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 ) AS DECIMAL(18,2))/18.00 AS DECIMAL(18,2)) + ISNULL((SELECT [PreviousEarnLeave] FROM [PreviousEarnLeave] WHERE EmployeeId = Employee.EmployeeId),0) - ISNULL((SELECT SUM(ConsumedTotalDays) FROM [dbo].[EmployeeLeave] WHERE LeaveTypeId = 5 AND EmployeeId = employee.EmployeeId GROUP BY EmployeeId),0) - ISNULL((SELECT SUM(ApprovedTotalDays) FROM EmployeeLeave WHERE EmployeeId = Employee.EmployeeId AND LeaveTypeId = 5 AND ApprovalStatus = 1 AND IsActive = 1 AND CAST(ApprovedFromDate AS DATE) >= @FromDate AND CAST(ApprovedToDate AS DATE) <= @ToDate ),0) + ISNULL((SELECT SUM(Days) FROM [dbo].[EarnLeaveConsumed] WHERE EmployeeId = Employee.EmployeeId AND IsActive = 1 GROUP BY EmployeeId), 0))/2 AS DECIMAL(18,2)))  AS DECIMAL(18,2)) AS NetPayable
							,JoiningDate
							,'2019-05-31'
							,GETDATE()
							,'DA761627-2D8C-462D-A168-BDA5FEDC1F78'
							,1

				
							FROM dbo.Employee
                            
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= GETDATE()) AND (employeeCompanyInfo.IsActive = 1))) employeeCompanyInfo 
							ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1
							LEFT JOIN 													
							(SELECT EmployeeSalary.EmployeeId,EmployeeSalary.BasicSalary,EmployeeSalary.GrossSalary, 
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						 
							FROM EmployeeSalary AS EmployeeSalary 
							WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= GETDATE()) AND EmployeeSalary.IsActive = 1)) employeeSalary 
							ON employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNumSal = 1  
							
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
							LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
							LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
							LEFT JOIN BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
							LEFT JOIN Unit AS unit ON branchUnit.UnitId = unit.UnitId
							LEFT JOIN Department AS department ON unitDepartment.DepartmentId = department.Id
							LEFT JOIN Branch AS branch ON branchUnit.BranchId = branch.Id
							LEFT JOIN Company AS company ON branch.CompanyId = company.Id
							LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
							LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId
							LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
							LEFT JOIN Line line ON departmentLine.LineId = line.LineId
							
							WHERE   																																		
								(company.Id = @CompanyId OR @CompanyId = -1)
							AND (branch.Id = @BranchId OR @BranchId = -1)
							AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
							AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1)
							AND ((departmentSection.DepartmentSectionId = @DepartmentSectionId) OR (@DepartmentSectionId = -1))
							AND ((departmentLine.DepartmentLineId = @DepartmentLineId) OR (@DepartmentLineId = -1))					
							AND ((employeeDesignation.Id = @EmployeeDesignationId) OR (@EmployeeDesignationId = -1))
							AND ((employeeGrade.Id = @EmployeeGradeId) OR (@EmployeeGradeId = -1))
							AND ((employeeType.Id = @EmployeeTypeId) OR (@EmployeeTypeId = -1))
							AND ((employee.Name LIKE '%' + @EmployeeName + '%') OR (@EmployeeName IS NULL))					        
							AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId = '')   														
							AND departmentSection.DepartmentSectionId <> 35 
							AND branchUnit.BranchUnitId IN (1,2)
							AND employeeType.Id IN (4,5)
							AND employeeCompanyInfo.rowNum = 1 						
							AND ((employee.Status = @ActiveStatus) OR (@ActiveStatus = -1))
							AND employee.IsActive = 1						
							AND JoiningDate <= @fromDate
																									
						    ORDER BY Employee.EmployeeCardId
		
														  														  						  											  							
END
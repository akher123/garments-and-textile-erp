-- ================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPEarnLeaveSheet -1, -1, -1, -1, -1, -1, -1, -1, -1, '', 'superadmin', '2019-01-01', '2019-12-31', '', 1
-- ================================================================================================================================

CREATE PROCEDURE [dbo].[SPEarnLeaveSheet]
			
								    
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

							 TRUNCATE TABLE [EarnLeaveReportTable]

							 INSERT INTO [dbo].[EarnLeaveReportTable]
							   ([EmployeeId]
							   ,[CardId]
							   ,[Employeename]
							   ,[Designation]
							   ,[BranchName]
							   ,[UnitName]
							   ,[DepartmentName]
							   ,[SectionName]
							   ,[LineName]
							   ,[EmployeeType]
							   ,[JoiningDate]
							   ,[CountDate]
							   ,[GrossSalary]
							   ,[PerDayGross]
							   ,[TotalPresent]
							   ,[EarnLeaveDays]
							   ,[PreviousEarnDays]
							   ,[EarnLeaveConsumed]
							   ,[AvailableEarnLeaveDays]
							   ,[PayableEarnLeaveDays]
							   ,[Stamp]
							   ,[NetPayable]
							   ,[FromDate]
							   ,[Todate]
							   ,[Year]
							   ,[LineId]
							   ,[Status])

							 SELECT 
							 Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.Name								AS Employeename
						    ,EmployeeDesignation.Title					AS Designation
							,branch.Name								AS BranchName
							,unit.Name									AS UnitName															
							,Department.Name		  					AS DepartmentName
							,Section.Name								AS SectionName
							,line.Name									AS LineName
							,EmployeeType.Title							AS EmployeeType										
							,CONVERT(NVARCHAR(12), JoiningDate, 106)	AS JoiningDate 
							,CONVERT(NVARCHAR(12), @toDate, 106)		AS CountDate
							,EmployeeSalary.GrossSalary					AS GrossSalary
							,CAST(employeeSalary.GrossSalary/30	AS DECIMAL(18,2))	AS PerDayGross
												
							,(SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 ) + ISNULL((SELECT [TotalDays] FROM [EarnLeaveDyeing] WHERE [EarnLeaveDyeing].EmployeeCardId = Employee.EmployeeCardId), 0)  AS TotalPresent									
							--,CAST(CAST((SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 ) AS DECIMAL(18,2))/18.00 as DECIMAL(18,2))  AS EarnLeaveDays	
							,0 AS EarnLeaveDays																												
							,ISNULL((SELECT Days FROM [EarnLeavegivenByYear] WHERE EmployeeId = Employee.EmployeeId AND (YEAR(FromDate)) + 1 = YEAR(@fromDate)), 0) + ISNULL((SELECT [Previous Balance] FROM [EarnLeaveDyeing] WHERE [EarnLeaveDyeing].EmployeeCardId = Employee.EmployeeCardId), 0)  AS PreviousEarnDays							
							,ISNULL((SELECT SUM(ConsumedTotalDays) FROM [dbo].[EmployeeLeave] WHERE LeaveTypeId = 5 AND EmployeeId = employee.EmployeeId GROUP BY EmployeeId),0) AS EarnLeaveConsumed
																															
							,0 AS AvailableEarnLeaveDays
							,0 AS PayableEarnLeaveDays													
							,'10'	AS Stamp														
							,0 AS NetPayable

							,CONVERT(NVARCHAR(12), @fromDate, 106)		AS FromDate
							,CONVERT(NVARCHAR(12), @toDate, 106)		AS Todate
							,YEAR(@toDate)								AS Year
							,@DepartmentLineId							AS LineId
							
							,Status = CASE   
									WHEN @ActiveStatus = 1 THEN '(Active Employee)'   
									WHEN @ActiveStatus = 2 THEN '(Quit Employee)'
									WHEN @ActiveStatus = -1 THEN '(All Employee)'
							END 

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
														   																																		
							WHERE (company.Id = @CompanyId OR @CompanyId = -1)
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
							AND branchUnit.BranchUnitId IN (1,2,3)
							AND employeeType.Id IN (4,5)
							AND employeeCompanyInfo.rowNum = 1 						
							AND ((employee.Status = @ActiveStatus) OR (@ActiveStatus = -1))
							AND employee.IsActive = 1						
							AND JoiningDate <= @fromDate
							

							UPDATE [EarnLeaveReportTable]
							SET [EarnLeaveDays] = CAST(TotalPresent/18 AS DECIMAL(18,2))
							
							UPDATE [EarnLeaveReportTable]	
							SET [AvailableEarnLeaveDays] = [EarnLeaveDays] + [PreviousEarnDays] - [EarnLeaveConsumed]												
	
							UPDATE [EarnLeaveReportTable]	
							SET [PayableEarnLeaveDays] = CAST([AvailableEarnLeaveDays]/2 AS DECIMAL(18,2))

							UPDATE [EarnLeaveReportTable]	
							SET [NetPayable] = CAST([AvailableEarnLeaveDays]/2 AS DECIMAL(18,2)) * [PerDayGross]
							

							SELECT [EmployeeId]
								  ,[CardId]
								  ,[Employeename]
								  ,[Designation]
								  ,[BranchName]
								  ,[UnitName]
								  ,[DepartmentName]
								  ,[SectionName]
								  ,[LineName]
								  ,[EmployeeType]
								  ,[JoiningDate]
								  ,[CountDate]
								  ,[GrossSalary]
								  ,[PerDayGross]
								  ,[TotalPresent]
								  ,[EarnLeaveDays]  
								  ,[PreviousEarnDays] 
								  ,[EarnLeaveConsumed]
								  ,[AvailableEarnLeaveDays]
								  ,[PayableEarnLeaveDays]
								  ,[Stamp]
								  ,[NetPayable]
								  ,[FromDate]
								  ,[Todate]
								  ,[Year]
								  ,[LineId]
								  ,[Status]
							  FROM [dbo].[EarnLeaveReportTable]
							  ORDER BY [CardId]		
														  														  						  											  							
END
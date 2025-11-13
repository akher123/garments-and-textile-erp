-- ================================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPEarnLeaveSheetSummary -1, -1, -1, -1, -1, -1, -1, -1, -1, '', 'superadmin', '2017-01-01', '2017-12-31', ''
-- ================================================================================================================================

CREATE PROCEDURE [dbo].[SPEarnLeaveSheetSummary]
			
								    
							     @CompanyId						INT = -1,
	                             @BranchId						INT = -1,
	                             @BranchUnitId					INT = -1,
	                             @BranchUnitDepartmentId		INT = -1,
	                             @DepartmentSectionId			INT = -1,
	                             @DepartmentLineId				INT = -1,
	                             @EmployeeTypeId				INT = -1,
	                             @EmployeeGradeId				INT = -1,
	                             @EmployeeDesignationId			INT = -1,	                      
	                             @EmployeeName					NVARCHAR(100) = NULL,	          	     
	                             @UserName						NVARCHAR(100) = NULL
								,@fromDate						DATETIME = NULL
						        ,@toDate						DATETIME = NULL
								,@EmployeeCardId				NVARCHAR(100) = ''
	                  	                  	                     	                 	                   								 								 	
AS

BEGIN
	
							 SET NOCOUNT ON;

							 DELETE FROM [EarnLeaveConsumed]
							 WHERE [ExecuteDate] = @toDate


							INSERT INTO [dbo].[EarnLeaveConsumed]
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
							   ,[Days]
							   ,[JoiningDate]
							   ,[ExecuteDate]
							   ,[FromDate]
							   ,[ToDate]
							   ,[CreateDate]
							   ,[CreatedBy]
							   ,[IsActive])

							 SELECT 
							 Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.Name								AS Employeename
						    ,EmployeeDesignation.Title					AS Designation
							,EmployeeType.Id							AS EmployeeTypeId
							,EmployeeType.Title							AS EmployeeType	
							,branchUnitDepartment.BranchUnitDepartmentId AS DepartmentId
							,Department.Name		  					AS DepartmentName
							,departmentSection.DepartmentSectionId		AS SectionId
							,Section.Name								AS SectionName
							,departmentLine.DepartmentLineId			AS LineId
							,line.Name									AS LineName
							,EmployeeSalary.GrossSalary					AS GrossSalary
							
							,CAST((employeeSalary.GrossSalary/30 * (SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 )/18.00/2) AS DECIMAL(18,2)) AS NetPayable
							,CAST(((SELECT COUNT(1) FROM EmployeeInOut WHERE EmployeeId = Employee.EmployeeId AND (Status = 'Present' OR Status = 'Late') AND CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND IsActive = 1 )/18.00/2) AS DECIMAL(18,2)) AS Days
							,CONVERT(NVARCHAR(12), JoiningDate, 106)	AS JoiningDate 
																										
							,CONVERT(NVARCHAR(12), @toDate, 106)		AS CountDate
							,CONVERT(NVARCHAR(12), @fromDate, 106)		AS FromDate
							,CONVERT(NVARCHAR(12), @toDate, 106)		AS ToDate
							,GETDATE()
							,'5146fd70-8cee-4022-a606-7cfafeb7874c'
							,1
																			
							FROM [dbo].Employee                            
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
							AND employee.IsActive = 1
							AND employee.[Status] = 1
							AND JoiningDate <= @fromDate

						    ORDER BY Employee.EmployeeCardId
		


						DECLARE @CheckDate DATE = @toDate


						UPDATE [EmployeeSalarySummary]    -- Empty Table
						   SET [EmployeeSalarySummary].NetAmount = 0
							  ,[EmployeeSalarySummary].SumEmployee = 0
							  ,[EmployeeSalarySummary].ExtraOT = 0
							  ,[EmployeeSalarySummary].RegularOT = 0


						UPDATE [EmployeeSalarySummary]    -- Line

						SET    [EmployeeSalarySummary].NetAmount = SalaryLine.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalaryLine.SumOfEmployee

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
							SELECT    LineId										 AS LineId
									 ,ISNULL(SUM(Amount), 0)						 AS NetAmount
									 ,COUNT(EmployeeCardId)							 AS SumOfEmployee			
									  FROM [dbo].[EarnLeaveConsumed]
									  WHERE [ExecuteDate] = @CheckDate AND EmployeeTypeId NOT IN(1,2,3)
			 
									  GROUP BY LineId

						) AS SalaryLine ON [EmployeeSalarySummary].LineId = SalaryLine.LineId


						UPDATE [EmployeeSalarySummary]   -- Section

						SET    [EmployeeSalarySummary].NetAmount = SalarySection.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalarySection.SumOfEmployee
	 

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
							SELECT    SectionId										 AS SectionId
									 ,ISNULL(SUM(Amount), 0)						 AS NetAmount
									 ,COUNT(EmployeeCardId)							 AS SumOfEmployee	
									  FROM [dbo].[EarnLeaveConsumed]
									  WHERE [ExecuteDate] = @CheckDate AND EmployeeTypeId NOT IN(1,2,3)		
									  GROUP BY SectionId

						) AS SalarySection ON CAST([EmployeeSalarySummary].SectionId AS INT) = SalarySection.SectionId


						UPDATE [EmployeeSalarySummary]  -- Department

						SET    [EmployeeSalarySummary].NetAmount = SalaryDepartment.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalaryDepartment.SumOfEmployee
	

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
							SELECT    DepartmentId									 AS DepartmentId
									 ,ISNULL(SUM(Amount), 0)						 AS NetAmount
									 ,COUNT(EmployeeCardId)							 AS SumOfEmployee	
									  FROM [dbo].[EarnLeaveConsumed]
									  WHERE [ExecuteDate] = @CheckDate AND EmployeeTypeId NOT IN(1,2,3)	AND SectionId <> 35		
									  GROUP BY DepartmentId

						) AS SalaryDepartment ON CAST([EmployeeSalarySummary].DepartmentId AS INT) = SalaryDepartment.DepartmentId


						UPDATE [EmployeeSalarySummary]  -- EmployeeType

						SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeType.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeType.SumOfEmployee

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
							SELECT    EmployeeTypeId							     AS EmployeeTypeId
									 ,ISNULL(SUM(Amount), 0)						 AS NetAmount
									 ,COUNT(EmployeeCardId)							 AS SumOfEmployee
									  FROM [dbo].[EarnLeaveConsumed]
									  WHERE [ExecuteDate] = @CheckDate 			
									  GROUP BY EmployeeTypeId

						) AS SalaryEmployeeType ON CAST([EmployeeSalarySummary].EmployeeTypeId AS INT) = SalaryEmployeeType.EmployeeTypeId

	

						UPDATE [EmployeeSalarySummary]  -- Security

						SET    [EmployeeSalarySummary].NetAmount = SalaryEmployeeType.NetAmount
							  ,[EmployeeSalarySummary].SumEmployee = SalaryEmployeeType.SumOfEmployee

						FROM   [EmployeeSalarySummary] INNER JOIN 
						(	   
										  SELECT SectionID
												,SUM(Amount) AS NetAmount
												,COUNT(1) AS SumOfEmployee
												FROM [dbo].[EarnLeaveConsumed]
												WHERE SectionID = 35 AND EmployeeTypeId NOT IN (1,2,3)
												AND [ExecuteDate] = @CheckDate
												GROUP BY SectionID

						) AS SalaryEmployeeType ON CAST([EmployeeSalarySummary].Id AS INT) = 17



						SELECT [Id]
							  ,[SLNO]
							  ,[EmployeeTypeId]
							  ,[EmployeeType]
							  ,[DepartmentId]
							  ,[Department]
							  ,[SectionId]
							  ,[Section]
							  ,[LineId]
							  ,[Line]
							  ,[SumEmployee]
							  ,[NetAmount]  AS AdvanceAmount	    
						  FROM [dbo].[EmployeeSalarySummary]
						  WHERE Id NOT IN (14,15,16,18)
						  ORDER BY [SLNO],[Id]
     
	  
						SELECT COUNT(1) AS SumOfEmployee 
							  ,SUM(Amount) AS TotalAmount       
							  FROM [dbo].[EarnLeaveConsumed]
							  WHERE [ExecuteDate] = @CheckDate
	

						SELECT SUM(SumEmployee) AS SumOfEmployee 
							  ,SUM(NetAmount) AS TotalAmount       
							  FROM [dbo].employeeSalarySummary
														  														  						  											  							
END

-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2016-11-22>					
-- Description:	<> EXEC [SPSalaryIncrementReport] 1, 1, 1, 6, 7, -1, '2016-12-26','2017-01-25', -1, '1718','superadmin', 5, 0
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPSalaryIncrementReport]

					    @CompanyId INT = -1
					   ,@BranchId INT = -1
					   ,@BranchUnitId INT = -1
					   ,@BranchUnitDepartmentId INT = -1
					   ,@DepartmentSectionId INT = -1 
					   ,@DepartmentLineId INT = -1
					   ,@FromDate DATETIME = NULL
					   ,@ToDate DATETIME = NULL				
					   ,@EmployeeTypeId INT = -1
					   ,@EmployeeCardId NVARCHAR(100) = ''
					   ,@UserName NVARCHAR(100) = ''
					   ,@IncrementPercent FLOAT
					   ,@OtherIncrement	DECIMAL(18,2) = 0.0
AS
BEGIN
			
						SET XACT_ABORT ON;
						SET NOCOUNT ON;
		 			
						IF(@FromDate IS NULL)
						BEGIN
							SET @FromDate = CAST(CURRENT_TIMESTAMP AS DATE)
						END
						ELSE
						BEGIN
							SET @FromDate = CAST(@FromDate AS DATE)
						END
		
						IF(@ToDate IS NULL)
						BEGIN
						SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
						END
						ELSE
						BEGIN
							SET @ToDate = CAST(@ToDate AS DATE)
						END


						DECLARE @TempEmployee TABLE (EmployeeId UNIQUEIDENTIFIER)

						INSERT INTO @TempEmployee
						SELECT employee.EmployeeId
												
							FROM Employee AS  employee						
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

							LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, employeeSalary.FromDate,												
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							FROM EmployeeSalary AS employeeSalary 
							WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
							ON employeeCompanyInfo.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

							LEFT JOIN EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 									
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
							LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
							LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
							LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
							LEFT JOIN Unit  AS unit ON branchUnit.UnitId = unit.UnitId
							LEFT JOIN Department  AS department ON unitDepartment.DepartmentId = department.Id
							LEFT JOIN Branch  AS branch ON branchUnit.BranchId = branch.Id
							LEFT JOIN Company  AS company ON branch.CompanyId = company.Id		
							LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
							LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
							LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
							LEFT JOIN Line line on departmentLine.LineId = line.LineId

							WHERE (employee.IsActive = 1
							AND ((employee.[Status] = 1) OR 
								((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))

							AND (company.Id = @CompanyId OR @CompanyId = -1)
							AND (branch.Id = @BranchId OR @BranchId = -1)
							AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
							AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1)
							AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
							AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
							AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)

							AND (employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')		
							AND (CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))			
							AND employeeType.Id <> 1	
							AND CAST(employeeSalary.FromDate AS DATE) BETWEEN CAST(DATEADD(YEAR, -1, @fromDate) AS DATE) AND CAST(DATEADD(YEAR, -1, @toDate) AS DATE) 	
							AND employeeType.Title LIKE '%Team Member%'
							--AND section.Name NOT LIKE '%Security%'							
						)

						DELETE FROM EmployeeIncrement							-- Delete existing data
						WHERE CAST(FromDate AS DATE) = CAST(@FromDate AS DATE) 
						AND CAST(ToDate AS DATE) = CAST(@ToDate AS DATE)
						AND EmployeeId IN
						(
							SELECT EmployeeId FROM @TempEmployee
						)

						INSERT INTO EmployeeIncrement
						(  
								[CompanyName]
							   ,[CompanyAddress]
							   ,[BranchName]
							   ,[UnitName]
							   ,[DepartmentName]
							   ,[SectionName]
							   ,[LineName]
							   ,[EmployeeId]
							   ,[EmployeeCardId]
							   ,[EmployeeName]
							   ,[EmployeeType]
							   ,[GradeName]
							   ,[DesignationName]
							   ,[JoiningDate]
							   ,[QuitDate]
							   ,[MobilePhone]
							   ,[Percent]
							   ,[GrossSalary]
							   ,[BasicSalary]
							   ,[HouseRent]
							   ,[MedicalAllowance]
							   ,[EntertainmentAllowance]
							   ,[FoodAllowance]
							   ,[Conveyance]
							   ,[LastIncrementDate]
							   ,[NewGross]
							   ,[NewBasic]
							   ,[NewHouseRent]
							   ,[OtherBenefit]
							   ,[OtherIncrement]
							   ,[TotalIncrement]
							   ,[ApprovedIncrement]
							   ,[Remarks]
							   ,[FromDate]
							   ,[ToDate]
							   ,[CreatedDate]														
							   ,[IsActive]
						)												 											
							 SELECT	company.NameInBengali	     											
							,company.FullAddressInBengali AS CompanyAddress		
							,branch.NameInBengali AS BranchName			
							,unit.NameInBengali  AS UnitName			
							,department.NameInBengali AS DepartmentName					
							,section.Name AS SectionName				
							,line.NameInBengali AS LineName
							,employee.EmployeeId
							,employee.EmployeeCardId
							,employee.NameInBengali AS EmployeeName			
							,employeeType.TitleInBengali AS EmployeeType		
							,employeeGrade.NameInBengali AS GradeName					
							,employeeDesignation.TitleInBengali AS DesignationName
							,employee.JoiningDate
							,employee.QuitDate
							,presentAddress.MobilePhone
							,CAST(@IncrementPercent AS NVARCHAR(3))+ '%' AS [Percent] 
							,employeeSalary.GrossSalary
							,employeeSalary.BasicSalary
							,employeeSalary.HouseRent 
							,employeeSalary.MedicalAllowance
							,employeeSalary.EntertainmentAllowance
							,employeeSalary.FoodAllowance
							,employeeSalary.Conveyance
							,employeeSalary.FromDate AS LastIncrementDate
							,'NewGross' = 	
								CASE		
									WHEN branchUnit.BranchUnitId = 1 THEN 	(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 1850				
									WHEN branchUnit.BranchUnitId = 2 THEN 	(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 850
									ELSE (employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 850
								END
							--,(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 1850  As NewGross

							,employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01  AS NewBasic
							,employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01 AS NewHouseRent
						
							,employeeSalary.GrossSalary - employeeSalary.BasicSalary - employeeSalary.HouseRent AS OtherBenefit	
							,@OtherIncrement AS OtherIncrement	
							
							,'TotalIncrement' = 	
								CASE		
									WHEN branchUnit.BranchUnitId = 1 THEN 	(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 1850 - employeeSalary.GrossSalary				
									WHEN branchUnit.BranchUnitId = 2 THEN 	(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 850 - employeeSalary.GrossSalary
									ELSE (employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 850 - employeeSalary.GrossSalary
								END
																											
							--,(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) + 1850 - employeeSalary.GrossSalary  AS TotalIncrement

							,' ' AS ApprovedIncrement
							,' ' AS Remarks
							,@FromDate AS FromDate
							,@ToDate AS ToDate
							,GETDATE()
							,1
							
							FROM					
							Employee AS  employee						
							LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

							LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, employeeSalary.FromDate,											
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							FROM EmployeeSalary AS employeeSalary 
							WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
							ON employeeCompanyInfo.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

							LEFT JOIN EmployeePresentAddress presentAddress ON Employee.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 									
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
							LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
							LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
							LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
							LEFT JOIN Unit  AS unit ON branchUnit.UnitId = unit.UnitId
							LEFT JOIN Department  AS department ON unitDepartment.DepartmentId = department.Id
							LEFT JOIN Branch  AS branch ON branchUnit.BranchId = branch.Id
							LEFT JOIN Company  AS company ON branch.CompanyId = company.Id		
							LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
							LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
							LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
							LEFT JOIN Line line on departmentLine.LineId = line.LineId

							WHERE (employee.IsActive = 1
							AND ((employee.[Status] = 1) OR 
								((employee.[Status] = 2) AND (employee.QuitDate >= @FromDate) AND (employee.QuitDate <= (DATEADD(DAY, 30, @ToDate)))))
							AND (company.Id = @CompanyId OR @CompanyId = -1)
							AND (branch.Id = @BranchId OR @BranchId = -1)
							AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
							AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1)
							AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
							AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
							AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)
							AND (employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')		
							AND (CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))			
							AND employeeType.Id <> 1	
							AND CAST(employeeSalary.FromDate AS DATE) BETWEEN CAST(DATEADD(YEAR, -1, @fromDate) AS DATE) AND CAST(DATEADD(YEAR, -1, @toDate) AS DATE) 	
							AND employeeType.Title LIKE '%Team Member%'
							--AND section.Name NOT LIKE '%Security%'	
							)	
								
							SELECT [CompanyName]
								  ,[CompanyAddress]
								  ,[BranchName]
								  ,[UnitName]
								  ,[DepartmentName]
								  ,[SectionName]
								  ,[LineName]
								  ,[EmployeeIncrement].[EmployeeId]
								  ,[EmployeeCardId]
								  ,[EmployeeName]
								  ,[EmployeeType]
								  ,[GradeName]
								  ,[DesignationName]
								  ,[JoiningDate]
								  ,[QuitDate]
								  ,[EmployeeIncrement].[MobilePhone]
								  ,[Percent]
								  ,[EmployeeIncrement].[GrossSalary]
								  ,[EmployeeIncrement].[BasicSalary]
								  ,[EmployeeIncrement].[HouseRent]
								  ,[EmployeeIncrement].[MedicalAllowance]
								  ,[EmployeeIncrement].[EntertainmentAllowance]
								  ,[EmployeeIncrement].[FoodAllowance]
								  ,[EmployeeIncrement].[Conveyance]
								  ,[LastIncrementDate]
								  ,[NewGross]
								  ,[NewBasic]
								  ,[NewHouseRent]
								  ,[OtherBenefit]
								  ,[OtherIncrement]
								  ,[TotalIncrement]
								  ,[ApprovedIncrement]
								  ,[Remarks]
								  ,[EmployeeIncrement].[FromDate]
								  ,[ToDate]
								  								
							  FROM [dbo].[EmployeeIncrement]

							  LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
							  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							  FROM EmployeeCompanyInfo AS employeeCompanyInfo 
							  WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
							  ON [EmployeeIncrement].EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 

							  LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, employeeSalary.FromDate,												
							  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							  FROM EmployeeSalary AS employeeSalary 
							  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
							  ON employeeCompanyInfo.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

								LEFT JOIN EmployeePresentAddress presentAddress ON EmployeeIncrement.EmployeeId = presentAddress.EmployeeId AND presentAddress.IsActive = 1 									
								LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id
								LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
								LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
								LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
								LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId
								LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId
								LEFT JOIN Unit  AS unit ON branchUnit.UnitId = unit.UnitId
								LEFT JOIN Department  AS department ON unitDepartment.DepartmentId = department.Id
								LEFT JOIN Branch  AS branch ON branchUnit.BranchId = branch.Id
								LEFT JOIN Company  AS company ON branch.CompanyId = company.Id		
								LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
								LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
								LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
								LEFT JOIN Line line on departmentLine.LineId = line.LineId

							  WHERE CAST([EmployeeIncrement].FromDate AS DATE) = CAST(@FromDate AS DATE)
							  AND CAST([EmployeeIncrement].ToDate AS DATE) = CAST(@ToDate AS DATE)
							  AND [EmployeeIncrement].IsActive = 1		
							  					
							  AND (company.Id = @CompanyId OR @CompanyId = -1)
							  AND (branch.Id = @BranchId OR @BranchId = -1)
							  AND (branchUnit.BranchUnitId = @BranchUnitId OR @BranchUnitId = -1)
							  AND (branchUnitDepartment.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = -1)
							  AND (departmentSection.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = -1)
							  AND (departmentLine.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = -1)			 							
							  AND (employeeType.Id = @EmployeeTypeId OR @EmployeeTypeId = -1)
							  AND ([EmployeeIncrement].EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')		

							  ORDER BY [EmployeeCardId]
							 


			 END	
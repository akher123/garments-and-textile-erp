
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2016-11-22>					
-- Description:	<> EXEC [SPSalaryIncrementReportManual] '2017-03-26','2017-04-25', '0305','superadmin', 10,21600,9900,7700,0,1600
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPSalaryIncrementReportManual]

			
					    @FromDate DATETIME = NULL
					   ,@ToDate DATETIME = NULL				
					   ,@EmployeeCardId NVARCHAR(100) = ''
					   ,@UserName NVARCHAR(100) = ''
					   ,@IncrementPercent FLOAT
					   ,@NewGross FLOAT
					   ,@NewBasic FLOAT
					   ,@NewHouseRent FLOAT
					   ,@OtherBenefit decimal
					   ,@TotalIncrement FLOAT
	

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


						DELETE FROM EmployeeIncrementManual							-- Delete existing data
						WHERE CAST(FromDate AS DATE) = CAST(@FromDate AS DATE) 
						AND CAST(ToDate AS DATE) = CAST(@ToDate AS DATE)
						AND EmployeeCardId = @EmployeeCardId						


						INSERT INTO EmployeeIncrementManual
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
							   	,[CreatedBy]												
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

							,@NewGross as NewGross
							,@NewBasic as NewBasic
							,@NewHouseRent as NewHouseRent
							,@OtherBenefit as OtherBenefit
							,0 AS OtherIncrement
							,@TotalIncrement as TotalIncrement
																
							--,(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) +4000  As NewGross
							--,employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01  AS NewBasic
							--,employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01 AS NewHouseRent
						
							--,employeeSalary.GrossSalary - employeeSalary.BasicSalary - employeeSalary.HouseRent AS OtherBenefit	
							--,0 AS OtherIncrement																						
							--,(employeeSalary.BasicSalary  + employeeSalary.BasicSalary * @IncrementPercent * .01) + (employeeSalary.HouseRent + employeeSalary.HouseRent * @IncrementPercent * .01) +4000 - employeeSalary.GrossSalary  AS TotalIncrement

							,' ' AS ApprovedIncrement
							,' ' AS Remarks
							,@FromDate AS FromDate
							,@ToDate AS ToDate
							,GETDATE()
							,@UserName as CreatedBy
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
							AND (employee.EmployeeCardId = @EmployeeCardId)		
							AND (CAST(employee.JoiningDate AS DATE) <= CAST(@ToDate AS DATE))			
							AND employeeType.Id <> 1	
							--AND CAST(employeeSalary.FromDate AS DATE) BETWEEN CAST(DATEADD(YEAR, -1, @fromDate) AS DATE) AND CAST(DATEADD(YEAR, -1, @toDate) AS DATE) 	
							--AND employeeType.Title LIKE '%Team Member%'
							AND section.Name NOT LIKE '%Security%'					
							)																			
			 END		
-- =================================================================================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <2019-02-06>
-- Description: EXEC [SPFinalSettlement] '627DE633-DA86-4451-8237-FB91FBBEDFFA', 'superadmin','2019-02-25', 0 
-- =================================================================================================================

CREATE PROCEDURE [dbo].[SPFinalSettlement]


							 @employeeId			UNIQUEIDENTIFIER
							,@userName				NVARCHAR(100)
							,@prepareDate			DATETIME
							,@OtherDeduction		DECIMAL(18,2)

AS

BEGIN
				
		SET NOCOUNT ON;
			

							DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);			
							DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);

							INSERT INTO @ListOfBranchUnitDepartmentIds
							SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
							WHERE UserName = @userName;

							INSERT INTO @ListOfEmployeeTypeIds
							SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
							WHERE UserName = @UserName;

							DECLARE @FromDate DATETIME;
							SELECT @FromDate=JoiningDate FROM Employee employee
							WHERE employee.EmployeeId = @employeeId AND employee.IsActive =1 

							SET @FromDate = @prepareDate
			
							DECLARE @WeekEndCount INT = 0;
							DECLARE @OverTimeRate DECIMAL(18,2) = 0.00

							SELECT @WeekEndCount = COUNT(*) FROM Weekends WHERE Weekends.IsActive = 1
							SET @OverTimeRate = dbo.fnGetOverTimeRate(CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)



							DELETE FROM [FinalSettlement]
							WHERE EmployeeId = @employeeId


							INSERT INTO [dbo].[FinalSettlement]
						   ([CompanyName]
						   ,[CompanyAddress]
						   ,[Month]
						   ,[Year]
						   ,[NameInBangla]
						   ,EmployeeId
						   ,[EmployeeCardId]
						   ,[Designation]
						   ,[Section]
						   ,[Department]
						   ,[JoiningDate]
						   ,[QuitDate]
						   ,[ServiceDuration]
						   ,[EarnLeave]
						   ,[DailyGrossSalary]
						   ,[ServiceBenefit]
						   ,[TotalWorkingDays]
						   ,[TotalOTHour]
						   ,[TotalPayDays]
						   ,[OverTimeRate]
						   ,[GrossSalary]
						   ,[DailyBasicSalary]
						   ,[BasicSalary]
						   ,[AbsentDays]
						   ,[BenefitGiven]
						   ,[EarnLeaveAmount]
						   ,[ServiceBenefitAmount]
						   ,[TotalAmountPaid]
						   ,[AttendanceBonus]
						   ,[AbsentFee]
						   ,[Advance]
						   ,[OtherDeduction]
						   ,[StampAmount]
						   ,[TotalDeduction]
						   ,[NetAmount]
						   ,[NetAmountInWord]
						   ,[PreparationDate])

				   SELECT Company.NameInBengali AS CompanyName
						 ,Company.FullAddressInBengali AS CompanyAddress
						 ,FORMAT(@prepareDate, 'MMMM') AS Month
						 ,FORMAT(@prepareDate, 'yyyy') AS Year
						 ,Employee.NameInBengali 
						 ,Employee.EmployeeId
						 ,Employee.EmployeeCardId
						 ,EmployeeDesignation.TitleInBengali AS Designation	
						 ,Section.NameInBengali AS Section	
						 ,Department.NameInBengali AS Department
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate 	 					 
						 ,CONVERT(VARCHAR(10),Employee.QuitDate, 103) QuitDate 
						 ,[dbo].[fnGetFinalSettlementServiceDuration](Employee.EmployeeId,Employee.JoiningDate, Employee.QuitDate) AS ServiceDuration
						 ,[dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) AS EarnLeave
						 ,CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DailyGrossSalary
						 ,0.0 AS ServiceBenefit
						 ,EmployeeJobCard.WorkingDays AS TotalWorkingDays
						 ,(EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) AS TotalOTHours
						 ,EmployeeJobCard.PayDays AS TotalPayDays
						 ,EmployeeJobCard.EmployeeOTRate AS OverTimeRate
						 ,EmployeeSalary.GrossSalary
						 ,CONVERT(DECIMAL(18,2),EmployeeSalary.BasicSalary/30) AS DailyBasicSalary
						 ,EmployeeSalary.BasicSalary

						 ,CAST((EmployeeJobCard.AbsentDays + EmployeeJobCard.TotalPenaltyAttendanceDays) AS INT) AS AbsentDays

						 ,CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) AS BenefitGiven
						 ,CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) AS EarnLeaveAmount
						 ,0.0 AS ServiceBenefitAmount
						 ,(CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus) AS TotalAmountPaid
						 ,EmployeeProcessedSalary.AttendanceBonus
						 ,EmployeeProcessedSalary.AbsentFee
						 ,EmployeeProcessedSalary.Advance
						 ,@OtherDeduction AS OtherDeduction
						 ,EmployeeProcessedSalary.Stamp AS StampAmount
						 ,(EmployeeProcessedSalary.TotalDeduction + @OtherDeduction) AS TotalDeduction
						 
						 ,CAST((CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT) AS NetAmount
						 ,dbo.fnIntegerToBengaliWords(CAST((CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT)) AS NetAmountInWord
						 ,CONVERT(VARCHAR(10),@prepareDate, 103) PreparationDate 
																																						 																												
FROM			    	Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate, JobTypeId, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId 
		
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id

						LEFT JOIN (SELECT EmployeeId, FromDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, EntertainmentAllowance, GrossSalary,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSalary = 1

						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
	
						LEFT JOIN EmployeeJobCard ON EmployeeJobCard.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCard.FromDate AND EmployeeJobCard.ToDate
						LEFT JOIN EmployeeProcessedSalary ON EmployeeProcessedSalary.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeProcessedSalary.FromDate AND EmployeeProcessedSalary.ToDate

						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)			



			SELECT		  Company.NameInBengali AS CompanyName
						 ,Company.FullAddressInBengali AS CompanyAddress
						 ,FORMAT(@prepareDate, 'MMMM') AS Month
						 ,FORMAT(@prepareDate, 'yyyy') AS Year
						 ,Employee.NameInBengali 
						 ,Employee.EmployeeCardId
						 ,EmployeeDesignation.TitleInBengali AS Designation	
						 ,Section.NameInBengali AS Section	
						 ,Department.NameInBengali AS Department
						 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) JoiningDate 	 					 
						 ,CONVERT(VARCHAR(10),Employee.QuitDate, 103) QuitDate 
						 ,[dbo].[fnGetFinalSettlementServiceDuration](Employee.EmployeeId,Employee.JoiningDate, Employee.QuitDate) AS ServiceDuration
						 ,[dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) AS EarnLeave
						 ,CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DailyGrossSalary
						 ,0.0 AS ServiceBenefit
						 ,EmployeeJobCard.WorkingDays AS TotalWorkingDays
						 ,(EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) AS TotalOTHours
						 ,EmployeeJobCard.PayDays AS TotalPayDays
						 ,EmployeeJobCard.EmployeeOTRate AS OverTimeRate
						 ,EmployeeSalary.GrossSalary
						 ,CONVERT(DECIMAL(18,2),EmployeeSalary.BasicSalary/30) AS DailyBasicSalary
						 ,EmployeeSalary.BasicSalary

						 ,CAST((EmployeeJobCard.AbsentDays + EmployeeJobCard.TotalPenaltyAttendanceDays) AS INT) AS AbsentDays

						 ,CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) AS BenefitGiven
						 ,CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) AS EarnLeaveAmount
						 ,0.0 AS ServiceBenefitAmount
						 ,(CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus) AS TotalAmountPaid
						 ,EmployeeProcessedSalary.AttendanceBonus
						 ,EmployeeProcessedSalary.AbsentFee
						 ,EmployeeProcessedSalary.Advance
						 ,@OtherDeduction AS OtherDeduction
						 ,EmployeeProcessedSalary.Stamp AS StampAmount
						 ,(EmployeeProcessedSalary.TotalDeduction + @OtherDeduction) AS TotalDeduction
						 
						 ,CAST((CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT) AS NetAmount
						 ,dbo.fnIntegerToBengaliWords(CAST((CAST((EmployeeProcessedSalary.GrossSalary/EmployeeProcessedSalary.TotalDays * (EmployeeProcessedSalary.Paydays + EmployeeProcessedSalary.AbsentDays)) +((EmployeeJobCard.TotalOTHours + EmployeeJobCard.TotalExtraOTHours + EmployeeJobCard.TotalWeekendOTHours + EmployeeJobCard.TotalHolidayOTHours) * EmployeeJobCard.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeSalary.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeProcessedSalary.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT)) AS NetAmountInWord
						 ,CONVERT(VARCHAR(10),@prepareDate, 103) PreparationDate 
																																						 																												
FROM			    	Employee AS  employee

						LEFT JOIN (SELECT EmployeeId, FromDate, JobTypeId, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId 
		
						INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
						INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
						INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
						INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
						INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
						INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
						INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
						INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
						INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
						INNER JOIN Company  AS company ON branch.CompanyId=company.Id

						LEFT JOIN (SELECT EmployeeId, FromDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, EntertainmentAllowance, GrossSalary,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
						FROM EmployeeSalary AS EmployeeSalary 
						WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
						ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSalary = 1

						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
	
						LEFT JOIN EmployeeJobCard ON EmployeeJobCard.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCard.FromDate AND EmployeeJobCard.ToDate
						LEFT JOIN EmployeeProcessedSalary ON EmployeeProcessedSalary.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeProcessedSalary.FromDate AND EmployeeProcessedSalary.ToDate

						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)					
						
						ORDER BY employee.EmployeeCardId ASC		

END
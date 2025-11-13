-- ===============================================================================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <2019-03-05>
-- Description: EXEC [SPFinalSettlement_08PM] '9710544D-2EA3-497F-950D-1805D79BB252', 'superadmin','2019-06-25', 0 
-- ===============================================================================================================

CREATE PROCEDURE [dbo].[SPFinalSettlement_08PM]


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

							DECLARE @BranchUnitId INT

							SELECT @BranchUnitId = BranchUnitId FROM EmployeeJobCard WHERE @prepareDate BETWEEN EmployeeJobCard.FromDate AND EmployeeJobCard.ToDate

			IF(@BranchUnitId = 1)
			BEGIN

				SELECT	  Company.NameInBengali AS CompanyName
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
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCardModel.GrossSalary/30) AS DailyGrossSalary
						 ,0.0 AS ServiceBenefit
						 ,EmployeeJobCardModel.WorkingDays AS TotalWorkingDays
						 ,(EmployeeJobCardModel.TotalOTHours) AS TotalOTHours
						 ,EmployeeJobCardModel.PayDays AS TotalPayDays
						 ,EmployeeJobCardModel.EmployeeOTRate AS OverTimeRate
						 ,EmployeeJobCardModel.GrossSalary
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCardModel.BasicSalary/30) AS DailyBasicSalary
						 ,EmployeeJobCardModel.BasicSalary

						 ,CAST((EmployeeJobCardModel.AbsentDays + EmployeeJobCard.TotalPenaltyAttendanceDays) AS INT) AS AbsentDays

						 ,CAST((EmployeeJobCardModel.GrossSalary/EmployeeJobCardModel.TotalDays * (EmployeeJobCardModel.Paydays + EmployeeJobCardModel.AbsentDays )) +((EmployeeJobCardModel.TotalOTHours) * EmployeeJobCardModel.EmployeeOTRate) AS DECIMAL(18,2)) AS BenefitGiven

						 ,CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModel.GrossSalary/30) AS DECIMAL(18,2)) AS EarnLeaveAmount
						 ,0.0 AS ServiceBenefitAmount
						 ,(CAST((EmployeeJobCardModel.GrossSalary/EmployeeJobCardModel.TotalDays * (EmployeeJobCardModel.Paydays + EmployeeJobCardModel.AbsentDays )) +((EmployeeJobCardModel.TotalOTHours) * EmployeeJobCardModel.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModel.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModel.AttendanceBonus) AS TotalAmountPaid
						 ,EmployeeJobCardModel.AttendanceBonus
						 ,(EmployeeJobCardModel.AbsentFee + (EmployeeJobCard.TotalPenaltyAttendanceDays * EmployeeJobCardModel.BasicSalary/30)) AS AbsentFee
						 ,EmployeeProcessedSalary.Advance
						 ,@OtherDeduction AS OtherDeduction
						 ,EmployeeProcessedSalary.Stamp AS StampAmount
						 ,(EmployeeProcessedSalary.TotalDeduction + @OtherDeduction) AS TotalDeduction
						 
						 ,CAST((CAST((EmployeeJobCardModel.GrossSalary/EmployeeJobCardModel.TotalDays * (EmployeeJobCardModel.Paydays + EmployeeJobCardModel.AbsentDays)) +((EmployeeJobCardModel.TotalOTHours) * EmployeeJobCardModel.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModel.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModel.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT) AS NetAmount
						 ,dbo.fnIntegerToBengaliWords(CAST((CAST((EmployeeJobCardModel.GrossSalary/EmployeeJobCardModel.TotalDays * (EmployeeJobCardModel.Paydays + EmployeeJobCardModel.AbsentDays )) +((EmployeeJobCardModel.TotalOTHours) * EmployeeJobCardModel.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModel.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModel.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT)) AS NetAmountInWord
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
	
						LEFT JOIN EmployeeJobCardModel ON EmployeeJobCardModel.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCardModel.FromDate AND EmployeeJobCardModel.ToDate
						LEFT JOIN EmployeeJobCard ON EmployeeJobCard.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCard.FromDate AND EmployeeJobCard.ToDate
						LEFT JOIN EmployeeProcessedSalary ON EmployeeProcessedSalary.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeProcessedSalary.FromDate AND EmployeeProcessedSalary.ToDate

						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)					
						
						ORDER BY employee.EmployeeCardId ASC		

			END

			ELSE

			BEGIN
				SELECT	  Company.NameInBengali AS CompanyName
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
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.GrossSalary/30) AS DailyGrossSalary
						 ,0.0 AS ServiceBenefit
						 ,EmployeeJobCardModelKnittingDyeing.WorkingDays AS TotalWorkingDays
						 ,(EmployeeJobCardModelKnittingDyeing.TotalOTHours) AS TotalOTHours
						 ,EmployeeJobCardModelKnittingDyeing.PayDays AS TotalPayDays
						 ,EmployeeJobCardModelKnittingDyeing.EmployeeOTRate AS OverTimeRate
						 ,EmployeeJobCardModelKnittingDyeing.GrossSalary
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.BasicSalary/30) AS DailyBasicSalary
						 ,EmployeeJobCardModelKnittingDyeing.BasicSalary

						 ,CAST((EmployeeJobCardModelKnittingDyeing.AbsentDays + EmployeeJobCard.TotalPenaltyAttendanceDays) AS INT) AS AbsentDays

						 ,CAST((EmployeeJobCardModelKnittingDyeing.GrossSalary/EmployeeJobCardModelKnittingDyeing.TotalDays * (EmployeeJobCardModelKnittingDyeing.Paydays + EmployeeJobCardModelKnittingDyeing.AbsentDays )) +((EmployeeJobCardModelKnittingDyeing.TotalOTHours) * EmployeeJobCardModelKnittingDyeing.EmployeeOTRate) AS DECIMAL(18,2)) AS BenefitGiven

						 ,CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.GrossSalary/30) AS DECIMAL(18,2)) AS EarnLeaveAmount
						 ,0.0 AS ServiceBenefitAmount
						 ,(CAST((EmployeeJobCardModelKnittingDyeing.GrossSalary/EmployeeJobCardModelKnittingDyeing.TotalDays * (EmployeeJobCardModelKnittingDyeing.Paydays + EmployeeJobCardModelKnittingDyeing.AbsentDays )) +((EmployeeJobCardModelKnittingDyeing.TotalOTHours) * EmployeeJobCardModelKnittingDyeing.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModelKnittingDyeing.AttendanceBonus) AS TotalAmountPaid
						 ,EmployeeJobCardModelKnittingDyeing.AttendanceBonus
						 ,(EmployeeJobCardModelKnittingDyeing.AbsentFee + (EmployeeJobCard.TotalPenaltyAttendanceDays * EmployeeJobCardModelKnittingDyeing.BasicSalary/30)) AS AbsentFee
						 ,EmployeeProcessedSalary.Advance
						 ,@OtherDeduction AS OtherDeduction
						 ,EmployeeProcessedSalary.Stamp AS StampAmount
						 ,(EmployeeProcessedSalary.TotalDeduction + @OtherDeduction) AS TotalDeduction
						 
						 ,CAST((CAST((EmployeeJobCardModelKnittingDyeing.GrossSalary/EmployeeJobCardModelKnittingDyeing.TotalDays * (EmployeeJobCardModelKnittingDyeing.Paydays + EmployeeJobCardModelKnittingDyeing.AbsentDays)) +((EmployeeJobCardModelKnittingDyeing.TotalOTHours) * EmployeeJobCardModelKnittingDyeing.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModelKnittingDyeing.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT) AS NetAmount
						 ,dbo.fnIntegerToBengaliWords(CAST((CAST((EmployeeJobCardModelKnittingDyeing.GrossSalary/EmployeeJobCardModelKnittingDyeing.TotalDays * (EmployeeJobCardModelKnittingDyeing.Paydays + EmployeeJobCardModelKnittingDyeing.AbsentDays )) +((EmployeeJobCardModelKnittingDyeing.TotalOTHours) * EmployeeJobCardModelKnittingDyeing.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCardModelKnittingDyeing.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCardModelKnittingDyeing.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT)) AS NetAmountInWord
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
	
						LEFT JOIN EmployeeJobCardModelKnittingDyeing ON EmployeeJobCardModelKnittingDyeing.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCardModelKnittingDyeing.FromDate AND EmployeeJobCardModelKnittingDyeing.ToDate
						LEFT JOIN EmployeeJobCard ON EmployeeJobCard.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCard.FromDate AND EmployeeJobCard.ToDate
						LEFT JOIN EmployeeProcessedSalary ON EmployeeProcessedSalary.Employeeid = employee.EmployeeId AND @prepareDate BETWEEN EmployeeProcessedSalary.FromDate AND EmployeeProcessedSalary.ToDate

						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)					
						
						ORDER BY employee.EmployeeCardId ASC
			END

END
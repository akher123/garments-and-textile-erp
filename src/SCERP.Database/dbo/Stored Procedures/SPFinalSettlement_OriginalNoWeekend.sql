-- ===============================================================================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <2019-03-06>
-- Description: EXEC [SPFinalSettlement_OriginalNoWeekend] '627DE633-DA86-4451-8237-FB91FBBEDFFA', 'superadmin','2019-02-25', 0 
-- ===============================================================================================================

CREATE PROCEDURE [dbo].[SPFinalSettlement_OriginalNoWeekend]


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
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.GrossSalary/30) AS DailyGrossSalary
						 ,0.0 AS ServiceBenefit
						 ,EmployeeJobCard_Original_NoWeekend.WorkingDays AS TotalWorkingDays
						 ,(EmployeeJobCard_Original_NoWeekend.TotalOTHours + EmployeeJobCard_Original_NoWeekend.TotalExtraOTHours + EmployeeJobCard_Original_NoWeekend.TotalHolidayOTHours) AS TotalOTHours
						 ,EmployeeJobCard_Original_NoWeekend.PayDays AS TotalPayDays
						 ,EmployeeJobCard_Original_NoWeekend.EmployeeOTRate AS OverTimeRate
						 ,EmployeeJobCard_Original_NoWeekend.GrossSalary
						 ,CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.BasicSalary/30) AS DailyBasicSalary
						 ,EmployeeJobCard_Original_NoWeekend.BasicSalary

						 ,CAST((EmployeeJobCard_Original_NoWeekend.AbsentDays) AS INT) AS AbsentDays

						 ,CAST((EmployeeJobCard_Original_NoWeekend.GrossSalary/EmployeeJobCard_Original_NoWeekend.TotalDays * (EmployeeJobCard_Original_NoWeekend.Paydays + EmployeeJobCard_Original_NoWeekend.AbsentDays)) +((EmployeeJobCard_Original_NoWeekend.TotalOTHours + EmployeeJobCard_Original_NoWeekend.TotalExtraOTHours + EmployeeJobCard_Original_NoWeekend.TotalHolidayOTHours) * EmployeeJobCard_Original_NoWeekend.EmployeeOTRate) AS DECIMAL(18,2)) AS BenefitGiven
						 ,CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.GrossSalary/30) AS DECIMAL(18,2)) AS EarnLeaveAmount
						 ,0.0 AS ServiceBenefitAmount
						 ,(CAST((EmployeeJobCard_Original_NoWeekend.GrossSalary/EmployeeJobCard_Original_NoWeekend.TotalDays * (EmployeeJobCard_Original_NoWeekend.Paydays + EmployeeJobCard_Original_NoWeekend.AbsentDays)) +((EmployeeJobCard_Original_NoWeekend.TotalOTHours + EmployeeJobCard_Original_NoWeekend.TotalExtraOTHours + EmployeeJobCard_Original_NoWeekend.TotalHolidayOTHours) * EmployeeJobCard_Original_NoWeekend.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCard_Original_NoWeekend.AttendanceBonus) AS TotalAmountPaid
						 ,EmployeeJobCard_Original_NoWeekend.AttendanceBonus
						 ,EmployeeJobCard_Original_NoWeekend.AbsentFee
						 ,EmployeeProcessedSalary.Advance
						 ,@OtherDeduction AS OtherDeduction
						 ,EmployeeProcessedSalary.Stamp AS StampAmount
						 ,(EmployeeProcessedSalary.TotalDeduction + @OtherDeduction) AS TotalDeduction
						 
						 ,CAST((CAST((EmployeeJobCard_Original_NoWeekend.GrossSalary/EmployeeJobCard_Original_NoWeekend.TotalDays * (EmployeeJobCard_Original_NoWeekend.Paydays + EmployeeJobCard_Original_NoWeekend.AbsentDays)) +((EmployeeJobCard_Original_NoWeekend.TotalOTHours + EmployeeJobCard_Original_NoWeekend.TotalExtraOTHours + EmployeeJobCard_Original_NoWeekend.TotalHolidayOTHours) * EmployeeJobCard_Original_NoWeekend.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCard_Original_NoWeekend.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT) AS NetAmount
						 ,dbo.fnIntegerToBengaliWords(CAST((CAST((EmployeeJobCard_Original_NoWeekend.GrossSalary/EmployeeJobCard_Original_NoWeekend.TotalDays * (EmployeeJobCard_Original_NoWeekend.Paydays + EmployeeJobCard_Original_NoWeekend.AbsentDays)) +((EmployeeJobCard_Original_NoWeekend.TotalOTHours + EmployeeJobCard_Original_NoWeekend.TotalExtraOTHours + EmployeeJobCard_Original_NoWeekend.TotalHolidayOTHours) * EmployeeJobCard_Original_NoWeekend.EmployeeOTRate) AS DECIMAL(18,2)) + CAST([dbo].[fnGetEarnLeaveDays](Employee.EmployeeId,'2017-01-01', Employee.QuitDate) * CONVERT(DECIMAL(18,2),EmployeeJobCard_Original_NoWeekend.GrossSalary/30) AS DECIMAL(18,2)) + EmployeeJobCard_Original_NoWeekend.AttendanceBonus - EmployeeProcessedSalary.TotalDeduction - @OtherDeduction) AS INT)) AS NetAmountInWord
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
	
						LEFT JOIN EmployeeJobCard_Original_NoWeekend ON EmployeeJobCard_Original_NoWeekend.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeJobCard_Original_NoWeekend.FromDate AND EmployeeJobCard_Original_NoWeekend.ToDate
						LEFT JOIN EmployeeProcessedSalary ON EmployeeProcessedSalary.EmployeeId = employee.EmployeeId AND @prepareDate BETWEEN EmployeeProcessedSalary.FromDate AND EmployeeProcessedSalary.ToDate

						WHERE employee.IsActive = 1 					
						AND employeeCompanyInfo.rowNum = 1 
						AND ((employee.EmployeeId = @employeeId) OR (@employeeId IS NULL))						
						AND branchUnitDepartment.BranchUnitDepartmentId IN (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
						AND employeeType.Id IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)					
						
						ORDER BY employee.EmployeeCardId ASC		

END
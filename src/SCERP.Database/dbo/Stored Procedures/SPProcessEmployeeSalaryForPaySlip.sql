

-- ===============================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-04>
-- Description:	<> EXEC [SPProcessEmployeeSalaryForPaySlip] 1, 1, 2, NULL, '2015-09-01','2015-09-30', 1, NULL
-- ===============================================================================================================


CREATE PROCEDURE [dbo].[SPProcessEmployeeSalaryForPaySlip]
	   @CompanyId INT,
	   @BranchId INT,
	   @BranchUnitId INT,
	   @BranchUnitDepartmentId INT = NULL, 
	   @FromDate DateTime,
	   @ToDate DateTime,
	   @EmployeeTypeId INT = NULL,
	   @EmployeeCardId NVARCHAR(100) = NULL  	
AS
BEGIN
	
		  SET NOCOUNT ON;
			
		  Declare @StartDate Datetime = @FromDate

		  INSERT INTO [dbo].[EmployeeProcessedSalaryForPaySlip]
           ([Row]
           ,[EmployeeId]
           ,[EmployeeCardId]
           ,[CompanyId]
           ,[CompanyName]
           ,[CompanyNameInBengali]
           ,[CompanyAddress]
           ,[CompanyAddressInBengali]
           ,[Name]
           ,[NameInBengali]
           ,[Designation]
           ,[DesignationInBengali]
           ,[Grade]
           ,[GradeInBengali]
           ,[JoiningDate]
           ,[BranchId]
           ,[Branch]
           ,[BranchUnitId]
           ,[Unit]
           ,[UnitInBengali]
           ,[Department]
           ,[DepartmentInBengali]
           ,[Section]
           ,[SectionInBengali]
           ,[Line]
           ,[LineInBengali]
           ,[TotalDays]
           ,[WorkingDays]
           ,[Paydays]
           ,[WeekendDays]
           ,[HolidayDays]
           ,[PresentDays]
           ,[AbsentDays]
           ,[LateDays]
           ,[LeaveDays]
           ,[LWPDays]
           ,[CasualLeave]
           ,[SickLeave]
           ,[MaternityLeave]
           ,[EarnLeave]
           ,[BasicSalary]
           ,[HouseRent]
           ,[MedicalAllowance]
           ,[Conveyance]
           ,[FoodAllowance]
           ,[EntertainmentAllowance]
           ,[GrossSalary]
           ,[LWPFee]
           ,[AbsentFee]
           ,[Advance]
           ,[Stamp]
           ,[TotalDeduction]
           ,[AttendanceBonus]
           ,[ShiftingBonus]
           ,[TotalBonus]
           ,[TotalPaid]
           ,[Rate]
           ,[OTHours]
           ,[OTRate]
           ,[TotalOTAmount]
           ,[NetAmount]
           ,[Month]
           ,[Year]
           ,[DepartmentId]
           ,[SectionId]
           ,[LineId]
           ,[EmployeeTypeId]
           ,[EmployeeType]
           ,[EmployeeTypeInBengali]
           ,[MonthName]
           ,[FromDate]
           ,[ToDate]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[EditedDate]
           ,[EditedBy]
           ,[IsActive])
					  
			SELECT		  CONVERT(VARCHAR(10), ROW_NUMBER() OVER(ORDER BY Employee.EmployeeCardId)) AS Row
						 ,eps.EmployeeId
						 ,eps.EmployeeCardId
						 ,eps.CompanyId
						 ,eps.CompanyName
						 ,company.NameInBengali
						 ,eps.CompanyAddress
						 ,company.FullAddressInBengali
						 ,eps.Name 
						 ,employee.NameInBengali
						 ,eps.Designation
						 ,employeeDesignation.TitleInBengali
						 ,eps.Grade
						 ,employeeGrade.NameInBengali
						 ,eps.JoiningDate
						 ,eps.BranchId
						 ,eps.Branch
						 ,eps.BranchUnitId
						 ,eps.Unit
						 ,unit.NameInBengali
						 ,eps.Department
						 ,department.NameInBengali
						 ,eps.Section
						 ,section.NameInBengali
						 ,eps.Line
						 ,line.NameInBengali
						 ,eps.[TotalDays]
						 ,eps.[WorkingDays]
						 ,eps.[Paydays]
						 ,eps.[WeekendDays]
						 ,eps.[HolidayDays]
						 ,eps.[PresentDays]
						 ,eps.[AbsentDays]
						 ,eps.[LateDays]
						 ,eps.[LeaveDays]
						 ,eps.[LWPDays]
						 ,eps.[CasualLeave]
						 ,eps.[SickLeave]
						 ,eps.[MaternityLeave]
						 ,eps.[EarnLeave]
						 ,eps.[BasicSalary]
						 ,eps.[HouseRent]
						 ,eps.[MedicalAllowance]
						 ,eps.[Conveyance]
						 ,eps.[FoodAllowance]
						 ,eps.[EntertainmentAllowance]
						 ,eps.[GrossSalary]
						 ,eps.[LWPFee]
						 ,eps.[AbsentFee]
						 ,eps.[Advance]
						 ,eps.[Stamp]
						 ,eps.[TotalDeduction]
						 ,eps.[AttendanceBonus]
						 ,eps.[ShiftingBonus]
						 ,eps.[TotalBonus]
						 ,eps.[TotalPaid]
						 ,2
						 ,eps.[OTHours]
						 ,eps.[OTRate]
						 ,eps.[TotalOTAmount]
						 ,eps.[NetAmount]
						 ,eps.[Month]
						 ,eps.[Year]
						 ,eps.[DepartmentId]
						 ,eps.[SectionId]
						 ,eps.[LineId]
						 ,eps.[EmployeeTypeId]
						 ,employeeType.Title
						 ,employeeType.TitleInBengali
						 ,eps.[MonthName]
						 ,eps.[FromDate]
					     ,eps.[ToDate]
						 ,CURRENT_TIMESTAMP
						 ,'3DEDBF63-DC40-456C-B931-28EC10D29BD9'
						 ,NULL
						 ,NULL
						 ,1		
						
						FROM EmployeeProcessedSalary eps
						INNER JOIN Employee employee ON eps.EmployeeId = employee.EmployeeId

						LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,IsEligibleForOvertime,
						ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
						FROM EmployeeCompanyInfo AS employeeCompanyInfo 
						WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @toDate) OR (@toDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
						ON employee.EmployeeId = employeeCompanyInfo.EmployeeId	 AND employeeCompanyInfo.rowNum = 1 
										
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
						LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
						LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
						LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
						LEFT JOIN Line line on departmentLine.LineId = line.LineId
						
						WHERE 
					     eps.[CompanyId] = @CompanyId
						AND eps.BranchId = @BranchId
						AND eps.BranchUnitId = @BranchUnitId
						AND (eps.DepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId IS NULL)
						AND eps.EmployeeTypeId = @EmployeeTypeId
						AND (eps.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)
																	
						ORDER BY eps.EmployeeCardId ASC	
END







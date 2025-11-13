-- =============================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <10/06/2018>
-- Description:	<> EXEC Utility_InoutAndJobCardProcess   3, '2018-05-01', '2018-05-26', 2018, 05, '2018-05-01', '2018-05-31' 
-- =============================================================================================================================

CREATE PROCEDURE [dbo].[Utility_InoutAndJobCardProcess]
			

							    @BranchUnitId		INT = 3
							   ,@fromDate			DATETIME = '2018-01-09'
							   ,@toDate				DATETIME = '2018-01-13'
							   ,@Year				INT		 = 2018
							   ,@Month				INT		 = 01
							   ,@MonthStart			DATETIME = '2017-12-26'
							   ,@MonthEnd			DATETIME = '2018-01-25'
							   						   
AS
BEGIN
	
			SET XACT_ABORT ON;
			SET NOCOUNT ON;
		 	
			BEGIN TRAN
					  												 
						
			  DELETE FROM [July].[dbo].[EmployeeInOut]
			  WHERE CAST([TransactionDate] AS DATE) NOT BETWEEN @MonthStart AND @MonthEnd

			  DELETE FROM [July].[dbo].[EmployeeInOut]
			  WHERE CAST([TransactionDate] AS DATE) BETWEEN @fromDate AND @toDate

			  DELETE FROM [July].[dbo].[EmployeeDailyAttendance]
			  WHERE CAST([TransactionDateTime] AS DATE) NOT BETWEEN @fromDate AND @toDate

			  DELETE FROM [July].[dbo].EmployeeWorkShift
			  WHERE CAST([ShiftDate] AS DATE) NOT BETWEEN @fromDate AND @toDate

			  DELETE FROM [July].[dbo].[OvertimeEligibleEmployee]
			  WHERE CAST([OvertimeDate] AS DATE) NOT BETWEEN @fromDate AND @toDate

			  TRUNCATE TABLE [July].[dbo].EmployeeJobCard


			  EXEC [July].[dbo].[SPProcessEmployeeInOut] 1, 1, @BranchUnitId, NULL, NULL, NULL, @fromDate, @toDate, 0, 0, NULL, '','superadmin'

			  EXEC [July].[dbo].[SPProcessEmployeeJobCard]  1, 1, @BranchUnitId, -1, -1, -1, -1, '', @Year, @Month, @MonthStart, @MonthEnd,'superadmin'


			  DELETE FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]
			  WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate
			  AND BranchUnitId = @BranchUnitId

			  DELETE FROM [SCERPDB_PILOT].[dbo].[EmployeeJobCard]
			  WHERE Year = @Year AND Month = @Month AND BranchUnitId = @BranchUnitId


			  INSERT INTO [SCERPDB_PILOT].[dbo].[EmployeeInOut]
				   ([CompanyId]
				   ,[CompanyName]
				   ,[CompanyAddress]
				   ,[BranchId]
				   ,[BranchName]
				   ,[BranchUnitId]
				   ,[UnitName]
				   ,[BranchUnitDepartmentId]
				   ,[DepartmentName]
				   ,[DepartmentSectionId]
				   ,[SectionName]
				   ,[DepartmentLineId]
				   ,[LineName]
				   ,[EmployeeId]
				   ,[EmployeeCardId]
				   ,[EmployeeName]
				   ,[EmployeeTypeId]
				   ,[EmployeeType]
				   ,[EmployeeGradeId]
				   ,[EmployeeGrade]
				   ,[EmployeeDesignationId]
				   ,[EmployeeDesignation]
				   ,[JoiningDate]
				   ,[QuitDate]
				   ,[BranchUnitWorkShiftId]
				   ,[WorkShiftName]
				   ,[EmployeeWorkShiftId]
				   ,[MobileNo]
				   ,[TransactionDate]
				   ,[InTime]
				   ,[OutTime]
				   ,[LastDayOutTime]
				   ,[Status]
				   ,[LateInMinutes]
				   ,[TotalContinuousAbsentDays]
				   ,[OTHours]
				   ,[LastDayOTHours]
				   ,[ExtraOTHours]
				   ,[LastDayExtraOTHours]
				   ,[WeekendOTHours]
				   ,[HolidayOTHours]
				   ,[Remarks]
				   ,[CreatedDate]
				   ,[CreatedBy]
				   ,[IsActive])
				SELECT 
				   [CompanyId]
				  ,[CompanyName]
				  ,[CompanyAddress]
				  ,[BranchId]
				  ,[BranchName]
				  ,[BranchUnitId]
				  ,[UnitName]
				  ,[BranchUnitDepartmentId]
				  ,[DepartmentName]
				  ,[DepartmentSectionId]
				  ,[SectionName]
				  ,[DepartmentLineId]
				  ,[LineName]
				  ,[EmployeeId]
				  ,[EmployeeCardId]
				  ,[EmployeeName]
				  ,[EmployeeTypeId]
				  ,[EmployeeType]
				  ,[EmployeeGradeId]
				  ,[EmployeeGrade]
				  ,[EmployeeDesignationId]
				  ,[EmployeeDesignation]
				  ,[JoiningDate]
				  ,[QuitDate]
				  ,[BranchUnitWorkShiftId]
				  ,[WorkShiftName]
				  ,[EmployeeWorkShiftId]
				  ,[MobileNo]
				  ,[TransactionDate]
				  ,[InTime]
				  ,[OutTime]
				  ,[LastDayOutTime]
				  ,[Status]
				  ,[LateInMinutes]
				  ,[TotalContinuousAbsentDays]
				  ,[OTHours]
				  ,[LastDayOTHours]
				  ,[ExtraOTHours]
				  ,[LastDayExtraOTHours]
				  ,[WeekendOTHours]
				  ,[HolidayOTHours]
				  ,[Remarks]
				  ,[CreatedDate]
				  ,[CreatedBy]
				  ,[IsActive]
			  FROM [July].[dbo].[EmployeeInOut]
			  WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate
			  AND BranchUnitId = @BranchUnitId


			  INSERT INTO [SCERPDB_PILOT].[dbo].[EmployeeJobCard]
				   ([EmployeeId]
				   ,[Year]
				   ,[Month]
				   ,[MonthName]
				   ,[FromDate]
				   ,[ToDate]
				   ,[CompanyId]
				   ,[CompanyName]
				   ,[CompanyNameInBengali]
				   ,[CompanyAddress]
				   ,[CompanyAddressInBengali]
				   ,[BranchId]
				   ,[BranchName]
				   ,[BranchNameInBengali]
				   ,[BranchUnitId]
				   ,[UnitName]
				   ,[UnitNameInBengali]
				   ,[BranchUnitDepartmentId]
				   ,[DepartmentName]
				   ,[DepartmentNameInBengali]
				   ,[DepartmentSectionId]
				   ,[SectionName]
				   ,[SectionNameInBengali]
				   ,[DepartmentLineId]
				   ,[LineName]
				   ,[LineNameInBengali]
				   ,[EmployeeCardId]
				   ,[EmployeeName]
				   ,[EmployeeNameInBengali]
				   ,[MobileNo]
				   ,[EmployeeTypeId]
				   ,[EmployeeType]
				   ,[EmployeeTypeInBengali]
				   ,[EmployeeGradeId]
				   ,[EmployeeGrade]
				   ,[EmployeeGradeInBengali]
				   ,[EmployeeDesignationId]
				   ,[EmployeeDesignation]
				   ,[EmployeeDesignationInBengali]
				   ,[EmployeeActiveStatusId]
				   ,[EmployeeCategoryId]
				   ,[JoiningDate]
				   ,[QuitDate]
				   ,[TotalDays]
				   ,[WorkingDays]
				   ,[PresentDays]
				   ,[LateDays]
				   ,[OSDDays]
				   ,[AbsentDays]
				   ,[LeaveDays]
				   ,[LWPDays]
				   ,[Holidays]
				   ,[WeekendDays]
				   ,[PayDays]
				   ,[CasualLeave]
				   ,[SickLeave]
				   ,[MaternityLeave]
				   ,[EarnLeave]
				   ,[GrossSalary]
				   ,[BasicSalary]
				   ,[HouseRent]
				   ,[MedicalAllowance]
				   ,[Conveyance]
				   ,[FoodAllowance]
				   ,[EntertainmentAllowance]
				   ,[PerDayBasicSalary]
				   ,[LWPFee]
				   ,[AbsentFee]
				   ,[AttendanceBonus]
				   ,[ShiftingBonus]
				   ,[TotalOTHours]
				   ,[TotalExtraOTHours]
				   ,[TotalWeekendOTHours]
				   ,[TotalHolidayOTHours]
				   ,[OTRate]
				   ,[EmployeeOTRate]
				   ,[TotalOTAmount]
				   ,[TotalExtraOTAmount]
				   ,[TotalWeekendOTAmount]
				   ,[TotalHolidayOTAmount]
				   ,[TotalPenaltyOTHours]
				   ,[TotalPenaltyOTAmount]
				   ,[TotalPenaltyAttendanceDays]
				   ,[TotalPenaltyAbsentFee]
				   ,[TotalPenaltyLeaveDays]
				   ,[TotalPenaltyFinancialAmount]
				   ,[CreatedDate]
				   ,[CreatedBy]
				   ,[IsActive])
				SELECT 
				   [EmployeeId]
				  ,[Year]
				  ,[Month]
				  ,[MonthName]
				  ,[FromDate]
				  ,[ToDate]
				  ,[CompanyId]
				  ,[CompanyName]
				  ,[CompanyNameInBengali]
				  ,[CompanyAddress]
				  ,[CompanyAddressInBengali]
				  ,[BranchId]
				  ,[BranchName]
				  ,[BranchNameInBengali]
				  ,[BranchUnitId]
				  ,[UnitName]
				  ,[UnitNameInBengali]
				  ,[BranchUnitDepartmentId]
				  ,[DepartmentName]
				  ,[DepartmentNameInBengali]
				  ,[DepartmentSectionId]
				  ,[SectionName]
				  ,[SectionNameInBengali]
				  ,[DepartmentLineId]
				  ,[LineName]
				  ,[LineNameInBengali]
				  ,[EmployeeCardId]
				  ,[EmployeeName]
				  ,[EmployeeNameInBengali]
				  ,[MobileNo]
				  ,[EmployeeTypeId]
				  ,[EmployeeType]
				  ,[EmployeeTypeInBengali]
				  ,[EmployeeGradeId]
				  ,[EmployeeGrade]
				  ,[EmployeeGradeInBengali]
				  ,[EmployeeDesignationId]
				  ,[EmployeeDesignation]
				  ,[EmployeeDesignationInBengali]
				  ,[EmployeeActiveStatusId]
				  ,[EmployeeCategoryId]
				  ,[JoiningDate]
				  ,[QuitDate]
				  ,[TotalDays]
				  ,[WorkingDays]
				  ,[PresentDays]
				  ,[LateDays]
				  ,[OSDDays]
				  ,[AbsentDays]
				  ,[LeaveDays]
				  ,[LWPDays]
				  ,[Holidays]
				  ,[WeekendDays]
				  ,[PayDays]
				  ,[CasualLeave]
				  ,[SickLeave]
				  ,[MaternityLeave]
				  ,[EarnLeave]
				  ,[GrossSalary]
				  ,[BasicSalary]
				  ,[HouseRent]
				  ,[MedicalAllowance]
				  ,[Conveyance]
				  ,[FoodAllowance]
				  ,[EntertainmentAllowance]
				  ,[PerDayBasicSalary]
				  ,[LWPFee]
				  ,[AbsentFee]
				  ,[AttendanceBonus]
				  ,[ShiftingBonus]
				  ,[TotalOTHours]
				  ,[TotalExtraOTHours]
				  ,[TotalWeekendOTHours]
				  ,[TotalHolidayOTHours]
				  ,[OTRate]
				  ,[EmployeeOTRate]
				  ,[TotalOTAmount]
				  ,[TotalExtraOTAmount]
				  ,[TotalWeekendOTAmount]
				  ,[TotalHolidayOTAmount]
				  ,[TotalPenaltyOTHours]
				  ,[TotalPenaltyOTAmount]
				  ,[TotalPenaltyAttendanceDays]
				  ,[TotalPenaltyAbsentFee]
				  ,[TotalPenaltyLeaveDays]
				  ,[TotalPenaltyFinancialAmount]
				  ,[CreatedDate]
				  ,[CreatedBy]
				  ,[IsActive]
			  FROM [July].[dbo].[EmployeeJobCard]
			  WHERE Year = @Year AND Month = @Month AND BranchUnitId = @BranchUnitId													 					  					  														  						  											  							

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;

END
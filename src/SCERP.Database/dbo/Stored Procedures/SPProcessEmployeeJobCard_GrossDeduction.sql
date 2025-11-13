
-- ==========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>						*** Running Time : 3 minutes ***
-- Description:	<> EXEC [SPProcessEmployeeJobCard_GrossDeduction]   2017, 7, '2017-06-26', '2017-07-25', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeJobCard_GrossDeduction]

									 @Year INT,
									 @Month INT,
								     @FromDate DATE 
									,@ToDate DATE 
									,@EmployeeCardId NVARCHAR(10)
AS
BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM EmployeeJobCardGrossDeduction 
							WHERE Year = @Year 
							AND Month = @Month
							AND CAST(fromDate AS DATE) = @FromDate
							AND CAST(toDate AS DATE) = @ToDate
							AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
			

							INSERT INTO [dbo].[EmployeeJobCardGrossDeduction]
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
						 
						SELECT [EmployeeId]
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
						  FROM [dbo].[EmployeeJobCard]
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
						  AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)

						
						  UPDATE [EmployeeJobCardGrossDeduction]
						  SET AbsentFee = CONVERT(DECIMAL(18,2), ISNULL((((EmployeeJobCardGrossDeduction.GrossSalary /30) * (dbo.fnGetAbsentDays(EmployeeJobCardGrossDeduction.EmployeeId, @FromDate, @ToDate)))), 0))
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
						  AND EmployeeType LIKE '%Team Member%'
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
  

						  UPDATE [EmployeeJobCardGrossDeduction]
						  SET TotalPenaltyOTHours = CONVERT(DECIMAL(18,2), dbo.fnGetEmployeeTotalPenaltyOTHours(EmployeeJobCardGrossDeduction.EmployeeId, EmployeeJobCardGrossDeduction.JoiningDate, EmployeeJobCardGrossDeduction.QuitDate, @fromDate, @toDate))
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
						  AND EmployeeType LIKE '%Team Member%'
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)


						  UPDATE [EmployeeJobCardGrossDeduction]
						  SET TotalPenaltyOTAmount = CONVERT(DECIMAL(18,2), (CAST((((EmployeeJobCardGrossDeduction.GrossSalary/208.00)* (dbo.fnGetOverTimeRate(@fromDate, @toDate))) 
								* (dbo.fnGetEmployeeTotalPenaltyOTHours(EmployeeJobCardGrossDeduction.EmployeeId, EmployeeJobCardGrossDeduction.JoiningDate, EmployeeJobCardGrossDeduction.QuitDate, @fromDate, @toDate))) AS DECIMAL(18,2))))
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
						  AND EmployeeType LIKE '%Team Member%'
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)

						  
						  UPDATE [EmployeeJobCardGrossDeduction]
						  SET TotalPenaltyAbsentFee = CONVERT(DECIMAL(18,2), (((EmployeeJobCardGrossDeduction.GrossSalary /30) * (dbo.fnGetEmployeeTotalPenaltyAttendanceDays(EmployeeJobCardGrossDeduction.EmployeeId, EmployeeJobCardGrossDeduction.JoiningDate, EmployeeJobCardGrossDeduction.QuitDate, @fromDate, @toDate))))) 
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
						  AND EmployeeType LIKE '%Team Member%'
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
 		
		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END







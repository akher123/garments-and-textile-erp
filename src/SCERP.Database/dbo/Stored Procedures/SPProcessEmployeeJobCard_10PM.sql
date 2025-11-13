
-- ==========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>						*** Running Time : 12:18:13 ***
-- Description:	<> EXEC [SPProcessEmployeeJobCard_10PM]   2020, 05, '2020-04-26', '2020-05-25', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeJobCard_10PM]


									 @Year INT,
									 @Month INT,
								     @FromDate DATE = '2017-04-26'
									,@ToDate DATE = '2017-05-25'
									,@EmployeeCardId NVARCHAR(10)
AS
BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM [EmployeeJobCard_10PM] 
							WHERE Year = @Year 
							AND Month = @Month
							AND CAST(fromDate AS DATE) = @FromDate
							AND CAST(toDate AS DATE) = @ToDate
							AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
			

							INSERT INTO [dbo].[EmployeeJobCard_10PM]
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


						  UPDATE [EmployeeJobCard_10PM]
						  SET TotalExtraOTHours = ISNULL((SELECT SUM([ExtraOTHours]) FROM [dbo].[EmployeeInOut_10PM_NoWeekend] WHERE [EmployeeInOut_10PM_NoWeekend].EmployeeId = [EmployeeJobCard_10PM].EmployeeId AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate GROUP BY EmployeeId), 0)		
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
  

						  UPDATE [EmployeeJobCard_10PM]
					      SET TotalExtraOTAmount = TotalExtraOTHours * EmployeeOTRate
					      WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)


						  UPDATE [EmployeeJobCard_10PM]
					      SET TotalPenaltyOTHours = 0
							 ,TotalPenaltyOTAmount = 0
							 ,TotalHolidayOTHours = 0
					      WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId) 					


						-- Attendance Penalty do not have OT and Extra OT : Penalty Days -> Absent Days 
						   DECLARE @EmployeeId UNIQUEIDENTIFIER
						   DECLARE @Days INT
						   DECLARE @PenaltyCountCursor AS CURSOR
		
						   SET @PenaltyCountCursor = CURSOR FOR 

								   SELECT EmployeeInOut.EmployeeId
								   ,COUNT(Penalty)	    
								   FROM [dbo].[HrmPenalty]  
								   LEFT JOIN EmployeeInOut ON EmployeeInOut.EmployeeId = [HrmPenalty].EmployeeId  AND CAST(EmployeeInOut.TransactionDate AS DATE) = CAST([HrmPenalty].PenaltyDate AS DATE) 
								   WHERE CAST([HrmPenalty].PenaltyDate AS DATE) BETWEEN @FromDate AND @ToDate AND [HrmPenalty].[PenaltyTypeId] = 2 
								   AND (OTHours = 0 AND ExtraOTHours = 0) 
								   GROUP BY EmployeeInOut.EmployeeId
						
						   OPEN @PenaltyCountCursor
						   FETCH NEXT FROM @PenaltyCountCursor INTO @EmployeeId, @Days

						   WHILE @@FETCH_STATUS = 0
						   BEGIN
 								  UPDATE EmployeeJobCard_10PM
								  SET AbsentDays = AbsentDays + @Days
								     ,TotalPenaltyAttendanceDays = TotalPenaltyAttendanceDays - @Days
								  WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
								  AND EmployeeId = @EmployeeId

						   FETCH NEXT FROM @PenaltyCountCursor INTO @EmployeeId, @Days
						   END

						   CLOSE @PenaltyCountCursor
						   DEALLOCATE @PenaltyCountCursor

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END







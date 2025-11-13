
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2015-10-12>						*** Running Time : 12:18:13 ***
-- Description:	<> EXEC [SPProcessEmployeeJobCard_NoPenalty]  3, 2019, 10, '2019-10-01', '2019-10-31', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeJobCard_NoPenalty]


									 @BranchUnitId INT,
									 @Year INT,
									 @Month INT,
								     @FromDate DATE = '2017-04-26'
									,@ToDate DATE = '2017-05-25'
									,@EmployeeCardId NVARCHAR(10)

AS

BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM [EmployeeJobCard_NoPenalty] 
							WHERE Year = @Year 
							AND Month = @Month
							AND CAST(fromDate AS DATE) = @FromDate
							AND CAST(toDate AS DATE) = @ToDate
							AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
							AND BranchUnitId = @BranchUnitId


							INSERT INTO [dbo].[EmployeeJobCard_NoPenalty]
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
						  AND BranchUnitId = @BranchUnitId


						  UPDATE [EmployeeJobCard_NoPenalty]
						  SET TotalExtraOTHours = ISNULL((SELECT SUM([ExtraOTHours]) FROM [dbo].[EmployeeInOut_NoPenalty] WHERE [EmployeeInOut_NoPenalty].EmployeeId = [EmployeeJobCard_NoPenalty].EmployeeId AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate GROUP BY EmployeeId), 0)		
						  WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
						  AND BranchUnitId = @BranchUnitId


						  UPDATE [EmployeeJobCard_NoPenalty]
					      SET TotalExtraOTAmount = TotalExtraOTHours * EmployeeOTRate
					      WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
						  AND BranchUnitId = @BranchUnitId


						  UPDATE [EmployeeJobCard_NoPenalty]
					      SET TotalPenaltyOTHours = 0
							 ,TotalPenaltyOTAmount = 0
							
					      WHERE Year = @Year 
						  AND Month = @Month
						  AND CAST(fromDate AS DATE) = @FromDate
						  AND CAST(toDate AS DATE) = @ToDate
					      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId) 					
						  AND BranchUnitId = @BranchUnitId

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
 								  UPDATE EmployeeJobCard_NoPenalty
								  SET AbsentDays = AbsentDays + @Days
								      --,PresentDays = PresentDays -@Days
								     ,TotalPenaltyAttendanceDays = TotalPenaltyAttendanceDays - @Days
								  WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
								  AND EmployeeId = @EmployeeId
								  AND BranchUnitId = @BranchUnitId

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







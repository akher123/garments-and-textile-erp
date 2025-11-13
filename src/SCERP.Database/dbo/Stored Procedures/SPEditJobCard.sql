
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPEditJobCard]  
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPEditJobCard]
						

							  @JobCardName					NVARCHAR(50)	
							 ,@EmployeeCardId				NVARCHAR(10)
							 ,@fromDate						DATETIME
							 ,@toDate						DATETIME
							 ,@PresentDays					INT
							 ,@PayDays						INT
							 ,@LateDays						INT
							 ,@TotalOTHours					DECIMAL(18,2)
							 ,@OSDDays						INT
							 ,@TotalExtraOTHours			DECIMAL(18,2)
							 ,@AbsentDays					INT
							 ,@TotalWeekendOTHours			DECIMAL(18,2)
							 ,@LeaveDays					INT
							 ,@TotalHolidayOTHours			DECIMAL(18,2)
							 ,@LWPDays						INT	
							 ,@TotalPenaltyOTHours			DECIMAL(18,2)
							 ,@WeekendDays					INT
							 ,@TotalPenaltyAttendanceDays	INT
							 ,@Holidays						INT
							 ,@TotalPenaltyLeaveDays		INT
							 ,@TotalDays					INT
							 ,@TotalPenaltyFinancialAmount  DECIMAL(18,2)
							 ,@WorkingDays					INT		
AS

BEGIN
	
	SET NOCOUNT ON;
			 

					IF(@JobCardName = 'EmployeeJobCardModel')
					BEGIN

							UPDATE EmployeeJobCardModel
							SET PresentDays = @PresentDays
							   ,PayDays = @PayDays
							   ,LateDays  = @LateDays
							   ,TotalOTHours = @TotalOTHours
							   ,OSDDays = @OSDDays			
							   ,AbsentDays = @AbsentDays		
							   ,LeaveDays = @LeaveDays	
							   ,LWPDays = @LWPDays	
							   ,WeekendDays = @WeekendDays
							   ,Holidays = @Holidays				
							   ,TotalDays = @TotalDays			
							   ,WorkingDays = @WorkingDays
							
							WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
							AND EmployeeCardId = @EmployeeCardId
					END


					IF(@JobCardName = 'EmployeeJobCard_10PM')
					BEGIN

							UPDATE EmployeeJobCard_10PM
							SET PresentDays = @PresentDays
							   ,PayDays = @PayDays
							   ,LateDays  = @LateDays
							   ,TotalOTHours = @TotalOTHours
							   ,OSDDays = @OSDDays
							   ,TotalExtraOTHours = @TotalExtraOTHours
							   ,AbsentDays = @AbsentDays
							   ,TotalWeekendOTHours = @TotalWeekendOTHours
							   ,LeaveDays = @LeaveDays
							   ,TotalHolidayOTHours = @TotalHolidayOTHours
							   ,LWPDays = @LWPDays
							   ,TotalPenaltyOTHours = @TotalPenaltyOTHours
							   ,WeekendDays = @WeekendDays
							   ,TotalPenaltyAttendanceDays = @TotalPenaltyAttendanceDays
							   ,Holidays = @Holidays
							   ,TotalPenaltyLeaveDays = @TotalPenaltyLeaveDays
							   ,TotalDays = @TotalDays
							   ,TotalPenaltyFinancialAmount = @TotalPenaltyFinancialAmount
							   ,WorkingDays = @WorkingDays
							
							WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
							AND EmployeeCardId = @EmployeeCardId
					END


					IF(@JobCardName = 'EmployeeJobCard_10PM_NoWeekend')
					BEGIN

							UPDATE EmployeeJobCard_10PM_NoWeekend
							SET PresentDays = @PresentDays
							   ,PayDays = @PayDays
							   ,LateDays  = @LateDays
							   ,TotalOTHours = @TotalOTHours
							   ,OSDDays = @OSDDays
							   ,TotalExtraOTHours = @TotalExtraOTHours
							   ,AbsentDays = @AbsentDays
							   ,TotalWeekendOTHours = @TotalWeekendOTHours
							   ,LeaveDays = @LeaveDays
							   ,TotalHolidayOTHours = @TotalHolidayOTHours
							   ,LWPDays = @LWPDays
							   ,TotalPenaltyOTHours = @TotalPenaltyOTHours
							   ,WeekendDays = @WeekendDays
							   ,TotalPenaltyAttendanceDays = @TotalPenaltyAttendanceDays
							   ,Holidays = @Holidays
							   ,TotalPenaltyLeaveDays = @TotalPenaltyLeaveDays
							   ,TotalDays = @TotalDays
							   ,TotalPenaltyFinancialAmount = @TotalPenaltyFinancialAmount
							   ,WorkingDays = @WorkingDays
							
							WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
							AND EmployeeCardId = @EmployeeCardId
					END 

					IF(@JobCardName = 'EmployeeJobCard_Original_NoPenalty')
					BEGIN

							UPDATE EmployeeJobCard_NoPenalty
							SET PresentDays = @PresentDays
							   ,PayDays = @PayDays
							   ,LateDays  = @LateDays
							   ,TotalOTHours = @TotalOTHours
							   ,OSDDays = @OSDDays
							   ,TotalExtraOTHours = @TotalExtraOTHours
							   ,AbsentDays = @AbsentDays
							   ,TotalWeekendOTHours = @TotalWeekendOTHours
							   ,LeaveDays = @LeaveDays
							   ,TotalHolidayOTHours = @TotalHolidayOTHours
							   ,LWPDays = @LWPDays
							   ,TotalPenaltyOTHours = @TotalPenaltyOTHours
							   ,WeekendDays = @WeekendDays
							   ,TotalPenaltyAttendanceDays = @TotalPenaltyAttendanceDays
							   ,Holidays = @Holidays
							   ,TotalPenaltyLeaveDays = @TotalPenaltyLeaveDays
							   ,TotalDays = @TotalDays
							   ,TotalPenaltyFinancialAmount = @TotalPenaltyFinancialAmount
							   ,WorkingDays = @WorkingDays
							
							WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
							AND EmployeeCardId = @EmployeeCardId
					END 

					IF(@JobCardName = 'EmployeeJobCard_Original_NoWeekend')
					BEGIN

							UPDATE EmployeeJobCard_Original_NoWeekend
							SET PresentDays = @PresentDays
							   ,PayDays = @PayDays
							   ,LateDays  = @LateDays
							   ,TotalOTHours = @TotalOTHours
							   ,OSDDays = @OSDDays
							   ,TotalExtraOTHours = @TotalExtraOTHours
							   ,AbsentDays = @AbsentDays
							   ,TotalWeekendOTHours = @TotalWeekendOTHours
							   ,LeaveDays = @LeaveDays
							   ,TotalHolidayOTHours = @TotalHolidayOTHours
							   ,LWPDays = @LWPDays
							   ,TotalPenaltyOTHours = @TotalPenaltyOTHours
							   ,WeekendDays = @WeekendDays
							   ,TotalPenaltyAttendanceDays = @TotalPenaltyAttendanceDays
							   ,Holidays = @Holidays
							   ,TotalPenaltyLeaveDays = @TotalPenaltyLeaveDays
							   ,TotalDays = @TotalDays
							   ,TotalPenaltyFinancialAmount = @TotalPenaltyFinancialAmount
							   ,WorkingDays = @WorkingDays
							
							WHERE CAST(FromDate AS DATE) = @fromDate AND CAST(ToDate AS DATE) = @toDate
							AND EmployeeCardId = @EmployeeCardId
					END 


					DECLARE @Result INT = 1

					IF (@@ERROR <> 0)
						SET @Result = 0
		
					SELECT @Result
					

END
-- ====================================================================================================================
-- Author	   :  Yasir
-- Create date :  2018-03-07
-- Description :  EXEC [SPGetSalaryXL] 1, 1, 2017, 09, '2017-08-26', '2017-09-25'
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SPGetSalaryXL] 


									 @CompanyId			INT
									,@BranchId			INT	
									,@Year				INT				
									,@Month				INT			
									,@FromDate			DATETIME
									,@ToDate			DATETIME					


AS
BEGIN

	SET NOCOUNT ON;

							
									SELECT 								
										   [EmployeeCardId]
										  ,[Name]
										  ,[NameInBengali]
										  ,[MobileNo]
										  ,[Year]
										  ,[Month]
										  ,[MonthName]
										  ,[FromDate]
										  ,[ToDate]					
										  ,[CompanyName]
										  ,[CompanyNameInBengali]
										  ,[CompanyAddress]
										  ,[CompanyAddressInBengali]					
										  ,[Branch]
										  ,[BranchInBengali]							
										  ,[Unit]
										  ,[UnitInBengali]								
										  ,[Department]
										  ,[DepartmentInBengali]							
										  ,[Section]
										  ,[SectionInBengali]				
										  ,[Line]
										  ,[LineInBengali]								
										  ,[EmployeeType]
										  ,[EmployeeTypeInBengali]							
										  ,[Grade]
										  ,[GradeInBengali]								
										  ,[Designation]
										  ,[DesignationInBengali]							
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
										  ,[HolidayDays]
										  ,[WeekendDays]
										  ,[Paydays]
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
										  ,[LWPFee]
										  ,[AbsentFee]
										  ,[PenaltyFee]
										  ,[Advance]
										  ,[Stamp]
										  ,[TotalDeduction]
										  ,[AttendanceBonus]
										  ,[ShiftingBonus]
										  ,[TotalBonus]
										  ,[TotalPaid]
										  ,[Rate]
										  ,[OTRate]
										  ,[OTHours]
										  ,[TotalOTAmount]
										  ,[NetAmount]
										  ,[TotalExtraOTHours]
										  ,[TotalExtraOTAmount]
										  ,[TotalWeekendOTHours]
										  ,[TotalWeekendOTAmount]
										  ,[TotalHolidayOTHours]
										  ,[TotalHolidayOTAmount]
										  ,[AdvancedIncomeTax]
										  ,[CreatedDate]
										  ,[CreatedBy]
										  ,[EditedDate]
										  ,[EditedBy]
										  ,[IsActive]
									  FROM [dbo].[EmployeeProcessedSalary]
									  WHERE Year     = @Year
									    AND Month    = @Month
									    AND FromDate = @FromDate
									    AND ToDate   = @ToDate
									  ORDER BY EmployeeCardId
									  			  														  												
END
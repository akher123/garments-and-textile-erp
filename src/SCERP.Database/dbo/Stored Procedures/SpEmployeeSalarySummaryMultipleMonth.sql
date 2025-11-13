-- ====================================================================================================================
-- Author	   :  Yasir
-- Create date :  2017-11-05
-- Description :  EXEC [SpEmployeeSalarySummaryMultipleMonth] 1, 1, 2017, 2017, 09, 10, '2016-03-26', '2016-04-25', ''
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SpEmployeeSalarySummaryMultipleMonth] 


					 @CompanyId			INT
					,@BranchId			INT	
					,@FromYear			INT
					,@ToYear			INT
					,@FromMonth			INT
					,@ToMonth			INT
					,@FromDate			DATETIME
					,@ToDate			DATETIME
					,@UserName			NVARCHAR(100) = ''	

AS
BEGIN

	SET NOCOUNT ON;

			SELECT 		
				   [Year]
				  ,[Month]
				  ,[MonthName]
				  ,[FromDate]
				  ,[ToDate]		
				  ,[CompanyName]
				  ,[CompanyAddress]
				  ,[Branch]
				  ,[Unit]	
				  ,[Department]
				  ,[Section]
				  ,[Line]					
				  ,CASE EmployeeCategoryId
						WHEN 1 THEN 'Regular Employee' 
						WHEN 2 THEN 'Quit Employee'
						WHEN 3 THEN 'New Joining Employee'
						ELSE 'New Joining and Quit Employee' END AS EmployeeCategory					
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
				 
			  FROM [dbo].[EmployeeProcessedSalary]
			  
			  WHERE IsActive = 1
			  AND (Year BETWEEN @FromYear AND @FromYear)
			  AND (Month BETWEEN @FromMonth AND @ToMonth)

			  ORDER BY FromDate, EmployeeCategoryId

END

-- ===========================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <14/05/2017>
-- Description:	<> EXEC SPMISInsertSalarySummary  2019, 05
-- ===========================================================

CREATE PROCEDURE [dbo].[SPMISInsertSalarySummary]
				
						   @Year  INT  = 2018
						  ,@Month INT  = 07
																				 
AS

BEGIN
	
	SET NOCOUNT ON;

					   DELETE FROM MIS_SalarySummary
					   WHERE [YearCode] = @Year AND [MonthCode] = @Month 


					   INSERT INTO MIS_SalarySummary
					   (    [YearCode]
						   ,[MonthCode]
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					    )						   						
					   SELECT 
					   @Year AS Year
					  ,@Month AS Month
					  ,DepartmentId
					  ,Department
					  ,SUM(NetAmount) AS NetAmount
					  ,SUM(TotalExtraOTAmount) AS ExtraOTAmount
					  ,SUM(TotalWeekendOTAmount) AS WeekendOTAmount
					  ,SUM(TotalHolidayOTAmount) AS HolidayOTAmount
					  ,(SELECT Percentage FROM MIS_DepartmentPercent WHERE MIS_DepartmentPercent.DepartmentId = EmployeeProcessedSalary.DepartmentId) 
					  ,'D'
					  FROM [dbo].[EmployeeProcessedSalary]
					  WHERE Year = @Year AND Month = @Month AND DepartmentId NOT IN(1, 5, 8, 11, 13, 14)
					  GROUP BY Department, DepartmentId	
					  		
														 	
					  INSERT INTO MIS_SalarySummary
					   (
							[YearCode]
						   ,[MonthCode]
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					   ) 						   						
					   SELECT 
					   @Year AS Year
					  ,@Month AS Month
					  ,DepartmentId
					  ,Department
					  ,SUM(NetAmount) AS NetAmount
					  ,SUM(TotalExtraOTAmount) AS ExtraOTAmount
					  ,SUM(TotalWeekendOTAmount) AS WeekendOTAmount
					  ,SUM(TotalHolidayOTAmount) AS HolidayOTAmount
					  ,(SELECT Percentage FROM MIS_DepartmentPercent WHERE MIS_DepartmentPercent.DepartmentId = EmployeeProcessedSalary.DepartmentId) 
					  ,'D'					  	
					  FROM [dbo].[EmployeeProcessedSalary]
					  WHERE Year = @Year AND Month = @Month AND DepartmentId IN(1, 5, 8, 11, 13)
					  GROUP BY Department, DepartmentId															



					  INSERT INTO MIS_SalarySummary
					  (
							[YearCode]
						   ,[MonthCode]
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					   ) 						   						
					   SELECT 
					   @Year AS Year
					  ,@Month AS Month
					  ,98
					  ,'Security'
					  ,400000 AS NetAmount
					  ,0 AS ExtraOTAmount
					  ,0 AS WeekendOTAmount
					  ,0 AS HolidayOTAmount
					  ,100
					  ,'I'


					  INSERT INTO MIS_SalarySummary
					   (
							[YearCode]
						   ,[MonthCode]
						   ,[DepartmentId]
						   ,[DepartmentName]
						   ,[NetAmount]
						   ,[ExtraOTAmount]
						   ,[WeekendOTAmount]
						   ,[HolidayOTAmount]
						   ,[Percentage]
						   ,[GroupCode]
					   ) 						   						
					   SELECT 
					   @Year AS Year
					  ,@Month AS Month
					  ,99
					  ,'Total Other Overhead'
					  ,23712004 AS NetAmount
					  ,0 AS ExtraOTAmount
					  ,0 AS WeekendOTAmount
					  ,0 AS HolidayOTAmount
					  ,100
					  ,'I'



					  DECLARE @Amount DECIMAL(18,2) 

					  SELECT @Amount = NetAmount FROM MIS_SalarySummary WHERE DepartmentId = 16 AND YearCode = @Year AND MonthCode = @Month

					  UPDATE MIS_SalarySummary 
					  SET NetAmount = NetAmount + ISNULL(@Amount, 0)
					  WHERE DepartmentId = 6
					  

					  DELETE MIS_SalarySummary WHERE DepartmentId = 16 AND YearCode = @Year AND MonthCode = @Month	
					  
					  
					  DECLARE @Result INT = 1;

					  IF (@@ERROR <> 0)
						  SET @Result = 0;
		
					  SELECT @Result;					 
					  																																																
END
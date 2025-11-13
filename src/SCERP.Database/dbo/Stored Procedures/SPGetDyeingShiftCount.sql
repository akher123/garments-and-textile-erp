-- ========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/08/2019>
-- Description:	<> EXEC [SPGetDyeingShiftCount] '2020-01-01', '2020-01-31'
-- ========================================================================

CREATE PROCEDURE [dbo].[SPGetDyeingShiftCount]
			
									
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  												 
						DECLARE @fromDate2  DATE 

						SET @fromDate2 = @fromDate

						DECLARE @listOfFriday TABLE (FridayDate Date)

						WHILE(@fromDate2 <= @toDate)
						BEGIN
								IF(DATENAME(dw, @fromDate2) = 'Friday') 
								INSERT @listOfFriday(FridayDate) VALUES(@fromDate2)

								SET @fromDate2 = DATEADD(DAY, 1, @fromDate2)
						END

						SELECT EmployeeCardId, EmployeeName, SectionName, EmployeeDesignation, EmployeeType, JoiningDate

						,(SELECT COUNT(1) FROM [EmployeeInOut] WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND EmployeeCardId = EmployeeTemp.EmployeeCardId AND WorkShiftName = 'Morning' AND Status IN('Present', 'Late') AND CAST(TransactionDate AS DATE) NOT IN(SELECT FridayDate FROM @listOfFriday)) AS Morning
						,(SELECT COUNT(1) FROM [EmployeeInOut] WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND EmployeeCardId = EmployeeTemp.EmployeeCardId AND WorkShiftName = 'Evening' AND Status IN('Present', 'Late') AND CAST(TransactionDate AS DATE) NOT IN(SELECT FridayDate FROM @listOfFriday)) AS Evening

						FROM

						 (SELECT EmployeeCardId, EmployeeName, SectionName, EmployeeDesignation, EmployeeType, JoiningDate
						  FROM [dbo].[EmployeeInOut]
						  WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate AND BranchUnitId = 3
 
						  GROUP BY EmployeeCardId, EmployeeName, SectionName, EmployeeDesignation, EmployeeType, JoiningDate

						  ) EmployeeTemp
						  ORDER BY EmployeeCardId
													 					  					  														  						  											  							
END
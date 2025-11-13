-- ========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/07/2019>
-- Description:	<> EXEC Utility_SMS_HRM '2019-08-26'
-- ========================================================================

CREATE PROCEDURE [dbo].[Utility_SMS_HRM]
			
									 
						   @EffectiveDate		DATETIME
						
												   
AS

BEGIN
	
			SET NOCOUNT ON;
					  			
									DECLARE @PreviousDay   DATETIME = DATEADD(DAY, -1, @EffectiveDate)
																	 
									SELECT	 CAST(@EffectiveDate AS DATE) AS Date
											,(SELECT SUM(1)		   FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate ) AS TotalEmployee
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate  AND Status IN( 'Present','Late')),0) AS TotalPresent
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate AND Status= 'Late'),0) AS TotalLate
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate  AND Status= 'Leave'),0) AS TotalLeave
											,ISNULL((SELECT SUM(1) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate  AND Status= 'Absent'),0) AS TotalAbsent
											,ISNULL((SELECT SUM(OTHours + ExtraOTHours) FROM  [dbo].[EmployeeInOut] AS EMP WHERE CAST([TransactionDate] AS DATE) = @PreviousDay),0) AS PreviousDayOTHours																																															

										FROM [dbo].[EmployeeInOut]

										LEFT JOIN

											(SELECT EmployeeId,BasicSalary,GrossSalary, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSal						
											FROM EmployeeSalary AS employeeSalary
											WHERE ((CAST(employeeSalary.FromDate AS Date) <= @EffectiveDate) OR (@EffectiveDate IS NULL))
											AND employeeSalary.IsActive = 1) employeeSalaryInfo 
											ON EmployeeInOut.EmployeeId = employeeSalaryInfo.EmployeeId AND employeeSalaryInfo.rowNumSal = 1	

										WHERE CAST([TransactionDate] AS DATE) = @EffectiveDate
										GROUP BY CAST([TransactionDate] AS DATE)
													 					  					  														  						  											  							
END
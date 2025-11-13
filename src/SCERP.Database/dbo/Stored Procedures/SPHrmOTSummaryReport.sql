-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <02/11/2016>
-- Description:	<> EXEC SPHrmOTSummaryReport '2016-09-26','2016-10-25'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPHrmOTSummaryReport]
								

						    @fromDate DATETIME = '2016-09-26'
						   ,@toDate DATETIME = '2016-10-25'				

AS
BEGIN
	
			SET NOCOUNT ON;
				

					  SELECT EmployeeInOut.TransactionDate
					  ,COUNT(EmployeeInOut.EmployeeId) AS NoOfEmployee
					  ,SUM([OTHours]) AS OTHours
					  ,SUM([OTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2))) AS OTAmount

					  ,SUM([ExtraOTHours]) AS ExtraOTHours
					  ,SUM([ExtraOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2))) AS ExtraOTAmount

					  ,SUM([WeekendOTHours]) AS WeekendOTHours
					  ,SUM([WeekendOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2))) AS WeekendOTAmount

					  ,SUM([HolidayOTHours]) AS HolidayOTHours
					  ,SUM([HolidayOTHours] * CAST(((employeeSalary.BasicSalary/208.00)*(dbo.fnGetOverTimeRate(@fromDate, @toDate))) AS DECIMAL(18,2))) AS HolidayOTAmount
 	  
					  FROM [dbo].[EmployeeInOut]
  
					  LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
												employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
												employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
												ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
												FROM EmployeeSalary AS employeeSalary 
												WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive=1) employeeSalary 
												ON [EmployeeInOut].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1

					  WHERE CAST(EmployeeInOut.TransactionDate AS DATE) BETWEEN @fromDate AND @toDate
					  AND (OTHours > 0 OR WeekendOTHours > 0 OR HolidayOTHours > 0)    
					  AND BranchUnitId IN(1,2)
					  AND IsActive = 1

					  GROUP BY EmployeeInOut.TransactionDate

					  ORDER BY [TransactionDate] 	  														  						  											  							
END



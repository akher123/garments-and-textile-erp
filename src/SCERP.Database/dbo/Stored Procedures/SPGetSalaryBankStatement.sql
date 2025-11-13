
-- ==========================================================================
-- Author:		<Md. Yasir Arafat>
-- Create date: <18/03/2015>
-- Description:	<> EXEC [SPGetSalaryBankStatement] '2019-02-26', '2019-03-25'
-- ==========================================================================

CREATE PROCEDURE [dbo].[SPGetSalaryBankStatement]

						
						   @fromDate DATETIME  = '2017-06-26'
					      ,@toDate DATETIME = '2017-07-25'

AS

BEGIN
				

			SET NOCOUNT ON;

						   SELECT ProcessSalary.EmployeeCardId AS [IDNO]
						  ,[AccountNumber] AS BRANCH
						  ,ProcessSalary.NetAmount AS AMOUNT
						  ,'PlummyFashionsLtd' AS REFERENCE
						  ,ProcessSalary.Name AS DESC1
	    
						  FROM 
						  	(SELECT EmployeeId, [AccountNumber], IsActive, ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
							FROM EmployeeBankInfo AS EmployeeBankInfo
							WHERE ((CAST(EmployeeBankInfo.FromDate AS Date) <= @toDate) OR (@fromDate IS NULL))
							AND EmployeeBankInfo.IsActive = 1) EmployeeBankInfo

						  LEFT JOIN EmployeeProcessedSalary AS ProcessSalary ON ProcessSalary.EmployeeId = EmployeeBankInfo.EmployeeId
						  WHERE ProcessSalary.FromDate = @fromDate 
						  AND ProcessSalary.ToDate = @toDate 
						  AND EmployeeBankInfo.IsActive = 1 
						  AND ProcessSalary.IsActive = 1
						  AND EmployeeBankInfo.rowNum = 1

						  AND (QuitDate IS NULL OR CAST(ProcessSalary.QuitDate AS DATE) NOT BETWEEN @fromDate AND @toDate)
	
						  ORDER BY ProcessSalary.EmployeeCardId	

END
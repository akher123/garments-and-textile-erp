-- ========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_LeaveButPresent '2020-06-26', '2020-07-25'
-- ========================================================================

CREATE PROCEDURE [dbo].[Utility_LeaveButPresent]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS
BEGIN
	
			SET NOCOUNT ON;
					  												 
						SELECT CompanyName
							  ,CompanyAddress
							  ,[EmployeeCardId]
							  ,[EmployeeName]    
							  ,[EmployeeDesignation]
							  ,CONVERT(VARCHAR(12),[TransactionDate], 106) AS [TransactionDate]	
							  ,[InTime]
							  ,[OutTime]   
							  ,[Remarks]
   
						  FROM [dbo].[EmployeeInOut]
						  WHERE CAST([TransactionDate] AS DATE) BETWEEN @fromDate AND @toDate AND QuitDate IS NULL AND BranchUnitId IN(1,2)
						  AND Status = 'Leave' AND (InTime IS NOT NULL OR OutTime IS NOT NULL)  AND EmployeeTypeId NOT IN(1,2,3)
						  ORDER BY EmployeeCardId
													 					  					  														  						  											  							
END
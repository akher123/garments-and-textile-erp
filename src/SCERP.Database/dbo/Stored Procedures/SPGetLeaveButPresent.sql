-- ========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC SPGetLeaveButPresent '2019-12-26', '2020-01-25'
-- ========================================================================

CREATE PROCEDURE [dbo].[SPGetLeaveButPresent]
			
									 
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
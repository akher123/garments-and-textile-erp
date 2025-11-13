-- ==========================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_MiddleManagementGotOverTimeWrongly '2020-08-01', '2020-08-31'
-- ==========================================================================================

CREATE PROCEDURE [dbo].[Utility_MiddleManagementGotOverTimeWrongly]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS
BEGIN
	
			SET NOCOUNT ON;
					  												 
							SELECT [Id]    
								  ,[EmployeeCardId]
								  ,[EmployeeName]    
								  ,[EmployeeType]       
								  ,[TransactionDate]
								  ,[InTime]
								  ,[OutTime]
								  ,[LastDayOutTime]
								  ,[Status]    
								  ,[Remarks]   
								  ,[IsActive]
							  FROM [dbo].[EmployeeInOut]
							  WHERE CAST(TransactionDate AS DATE) BETWEEN @fromDate AND @toDate
							  AND EmployeeTypeId IN(1,2,3) 
							  AND (OTHours > 0 OR ExtraOTHours > 0 OR WeekendOTHours > 0 OR HolidayOTHours > 0)
							  AND IsActive = 1
													 					  					  														  						  											  							
END
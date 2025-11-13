-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/11/2018>
-- Description:	<> EXEC Utility_WeekendInTimeOutTime '2019-01-26', '2019-02-25'
-- ==============================================================================

CREATE PROCEDURE [dbo].[Utility_WeekendInTimeOutTime]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  												 
							  -- Set Intime
							  UPDATE [EmployeeInOut]
							  SET InTime = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 2 AS INT), CAST('09:00:00.0000000' AS TIME))
							      ,LateInMinutes = 0
							  WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate 
							  AND (InTime IS NULL OR InTime > '16:00:00.0000000')
							  AND  WeekendOTHours  > 1
							  AND BranchUnitId IN (1,2)


							  --Set OutTime after Lunch
							  UPDATE [EmployeeInOut]
							  SET OutTime = DATEADD(HOUR,(WeekendOTHours + 1), DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 2 AS INT), CAST(InTime AS TIME)))
							  WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
							  AND OutTime IS NULL
							  AND WeekendOTHours > 4
							  AND BranchUnitId IN(1,2)


							  -- Set Weekend before Lunch
							  UPDATE [EmployeeInOut]
							  SET OutTime = DATEADD(HOUR,(WeekendOTHours), DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 2 AS INT), CAST(InTime AS TIME)))
							  WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate 
							  AND OutTime IS NULL
							  AND WeekendOTHours BETWEEN 1 AND 4
							  AND BranchUnitId IN(1,2)
													 					  					  														  						  											  							
END
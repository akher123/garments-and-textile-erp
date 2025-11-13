-- ===================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_NoLateForPreviousDayOutTime '2020-08-01', '2020-08-31'
-- ===================================================================================

CREATE PROCEDURE [dbo].[Utility_NoLateForPreviousDayOutTime]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;

					  								
						DECLARE @CountDate DATE = @fromdate

						WHILE @CountDate <= @toDate
						BEGIN
				
								DECLARE @PreviousDate DATE = @CountDate
								DECLARE @PresentDate DATE

								SET @PresentDate = DATEADD(DAY, 1, @PreviousDate)
 
								UPDATE EmployeeInOut
								SET Status = 'Present'
								   ,LateInMinutes = 0
								   ,Remarks = ''
								WHERE CAST(TransactionDate AS DATE) = @PresentDate
								AND BranchUnitId <> 3
								AND Status = 'late' AND EmployeeId IN 
								(
									SELECT EmployeeId
									FROM EmployeeInOut
									WHERE CAST(TransactionDate AS DATE) = @PreviousDate AND CAST(OutTime AS TIME) BETWEEN '02:30:00.0000000' AND '07:00:00.0000000'
								)

								SET @CountDate = DATEADD(DAY, 1, @CountDate)

					END	
									 					  					  														  						  											  							
END
-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_AbsentOnJoiningDate '2020-06-26', '2020-07-25'
-- ==============================================================================

CREATE PROCEDURE [dbo].[Utility_AbsentOnJoiningDate]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS
BEGIN
	
			SET NOCOUNT ON;
					  												 
							SELECT EmployeeId
								  ,EmployeeCardId
								  ,Name
								  ,CONVERT(VARCHAR(12), JoiningDate, 106) JoinDate
								  ,CONVERT(VARCHAR(12), FirstDayOfPresent, 106)  FirstDateOfPresent
							FROM  vwFirstTimePunchedEmployee

				WHERE        ((FirstDayOfPresent IS NULL) OR
							 (CAST(JoiningDate AS DATE) < CAST(FirstDayOfPresent AS DATE))) AND CAST(JoiningDate AS DATE) BETWEEN @fromDate AND @toDate
							 ORDER BY EmployeeCardId	
													 					  					  														  						  											  							
END
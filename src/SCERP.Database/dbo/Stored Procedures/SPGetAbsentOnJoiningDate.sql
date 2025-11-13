-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC [SPGetAbsentOnJoiningDate] '2019-11-26', '2020-01-25'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPGetAbsentOnJoiningDate]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS
BEGIN
	
			SET NOCOUNT ON;
					  												 
							SELECT EmployeeId
								  ,EmployeeCardId
								  ,Name
								  ,CONVERT(VARCHAR(10), JoiningDate, 106) JoinDate
								  ,CONVERT(VARCHAR(10), FirstDayOfPresent, 106)  FirstDateOfPresent
							FROM  vwFirstTimePunchedEmployee

				WHERE        ((FirstDayOfPresent IS NULL) OR
							 (CAST(JoiningDate AS DATE) < CAST(FirstDayOfPresent AS DATE))) AND CAST(JoiningDate AS DATE) BETWEEN @fromDate AND @toDate
							 ORDER BY EmployeeCardId	
													 					  					  														  						  											  							
END
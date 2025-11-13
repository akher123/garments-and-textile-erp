-- ===========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <11/09/2019>
-- Description:	<> EXEC Utility_DyeingWeekendChange '2020-08-01', '2020-08-31'
-- ===========================================================================

CREATE PROCEDURE [dbo].[Utility_DyeingWeekendChange]
			
									 
								   @FromDate		DATETIME
								  ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  			  									 
								  --UPDATE  EmployeeInOut
								  --SET InTime = NULL
									 --,OutTime =  NULL
									 --,LateInMinutes = 0
									 --,WeekendOTHours = 0
								  --WHERE Status = 'Weekend' AND WorkShiftName = 'Morning' AND BranchUnitId = 3
								  --AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
								  --AND EmployeeId IN
								  --(
										--SELECT [EmployeeId]           
										--FROM [dbo].[WorkGroupRoster]
										--WHERE GroupName <> 'Group-1'
										--AND [UnitName] = 'Dyeing'
								  --)


						DECLARE @EmployeeCardId		NVARCHAR(20)
						DECLARE @Date				DATETIME 						


						DECLARE @BusinessCursor AS CURSOR;

						SET @BusinessCursor = CURSOR FOR
									
							SELECT EmployeeCardId, TransactionDate
							FROM EmployeeInOut
							WHERE WorkShiftName = 'Evening' AND ExtraOTHours > 1 AND BranchUnitId = 3
							AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate						
							AND EmployeeId IN
							(
								SELECT [EmployeeId]           
								FROM [dbo].[WorkGroupRoster]
								WHERE GroupName <> 'Group-1'
								AND [UnitName] = 'Dyeing'
							)
  
						OPEN @BusinessCursor;
						FETCH NEXT FROM @BusinessCursor INTO @EmployeeCardId, @Date

						WHILE @@FETCH_STATUS = 0
						BEGIN 	 	
								
								IF EXISTS(SELECT EmployeecardId FROM EmployeeInOut WHERE EmployeeCardId = @EmployeeCardId AND TransactionDate = DATEADD(DAY, 1, @Date)  AND Status ='Weekend')
								BEGIN
														
									UPDATE EmployeeInOut										
									SET InTime = '09:00:00'
									   ,OutTime = (SELECT OutTime FROM EmployeeInOut WHERE TransactionDate = @Date AND EmployeeCardId = @EmployeeCardId )
									   ,WeekendOTHours = (SELECT ISNULL((ExtraOTHours -1), 0) FROM EmployeeInOut WHERE TransactionDate = @Date AND EmployeeCardId = @EmployeeCardId)
									   ,LateInMinutes = NULL
									WHERE TransactionDate = DATEADD(DAY, 1, @Date)
									AND EmployeeCardId = @EmployeeCardId
								

									UPDATE EmployeeInOut										
									SET OutTime = '09:00:00'
									   ,ExtraOTHours = 1
									WHERE TransactionDate = @Date
									AND EmployeeCardId = @EmployeeCardId

								END									

						FETCH NEXT FROM @BusinessCursor INTO @EmployeeCardId, @Date
						END

						CLOSE @BusinessCursor;
						DEALLOCATE @BusinessCursor;
													 					  					  														  						  											  							
END
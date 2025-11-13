-- =========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_DyeingShiftRepeat '2019-08-01', '2019-08-31'
-- =========================================================================

CREATE PROCEDURE [dbo].[Utility_DyeingShiftRepeat]
			
									 
								     @FromDate		    DATETIME
								    ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  												 
						
							DECLARE  @EmployeeCarId		NVARCHAR(10)
							DECLARE  @Date				DATE							
									,@StatusToday		NVARCHAR(20)
									,@StatusTomorrow	NVARCHAR(20)
									,@Count				INT = 0



							DECLARE @BusinessCursor AS CURSOR;

							SET @BusinessCursor = CURSOR FOR

							SELECT EmployeeCardId FROM EmployeeJobCardModelKnittingDyeing WHERE FromDate = @FromDate AND ToDate = @ToDate
                
							OPEN @BusinessCursor;
							FETCH NEXT FROM @BusinessCursor INTO @EmployeeCarId

							WHILE @@FETCH_STATUS = 0
							BEGIN 	 	
		
									SET @fromDate	= '2019-08-01'
									SET @toDate		= '2019-08-31'

									WHILE(@fromDate < @toDate)
									BEGIN
				
											IF(DATENAME(dw, @fromDate) = 'Friday' OR DATENAME(dw, DATEADD(DAY, 1, @fromDate)) = 'Friday' ) 
											BEGIN
												SET @fromDate = DATEADD(DAY, 1, @fromDate)	
												CONTINUE
											END

											SELECT @StatusToday = WorkShiftName FROM EmployeeInOutModelKnittingDyeing WHERE TransactionDate = @fromDate AND EmployeeCardId = @EmployeeCarId AND Status <> 'Weekend'
											SELECT @StatusTomorrow = WorkShiftName FROM EmployeeInOutModelKnittingDyeing WHERE TransactionDate = DATEADD(DAY, 1, @fromDate) AND EmployeeCardId = @EmployeeCarId AND Status <> 'Weekend'
				
											IF(@StatusToday <> @StatusTomorrow)
											BEGIN

													PRINT @EmployeeCarId + ' => ' + CAST(@fromDate AS NVARCHAR(20))						
											END
			
											SET @fromDate = DATEADD(DAY, 1, @fromDate)
									END

								FETCH NEXT FROM @BusinessCursor INTO @EmployeeCarId
							END

							CLOSE @BusinessCursor;
							DEALLOCATE @BusinessCursor;
													 					  					  														  						  											  							
END
-- ==================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_EmployeeManualOverTime  '2020-06-26', '2020-07-25'
-- ==================================================================================

CREATE PROCEDURE [dbo].[Utility_EmployeeManualOverTime]
			
									 
							@FromDate		DATETIME
						   ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  												 
							DECLARE @EmployeeId uniqueidentifier = NULL	
							DECLARE @Date DATETIME 
							DECLARE @OTHour NUMERIC(18,2)		


							DECLARE ManualOverTime CURSOR FOR

									SELECT EmployeeId, Date, OverTimeHours 
									FROM EmployeeManualOverTime
									WHERE CAST(Date AS DATE) BETWEEN @FromDate AND @ToDate
									AND IsActive = 1

							OPEN ManualOverTime

							FETCH NEXT FROM ManualOverTime INTO @EmployeeId, @Date, @OTHour

							WHILE @@FETCH_STATUS = 0
							BEGIN
								BEGIN TRANSACTION TR_OverTime

										-- Weekend 
										IF(DATENAME(dw, @Date) = 'Friday' )   
										BEGIN							
											 UPDATE EmployeeInOut
											 SET WeekendOTHours = WeekendOTHours + @OTHour								
											 WHERE EmployeeId = @EmployeeId AND CAST(TransactionDate AS DATE) = CAST(@Date AS DATE)
										END

										-- Holiday
										ELSE IF EXISTS( SELECT 1 FROM HolidaysSetup WHERE CAST(StartDate AS DATE) <= @Date AND CAST(EndDate AS DATE) >= @Date)
										BEGIN
											 UPDATE EmployeeInOut
											 SET HolidayOTHours = HolidayOTHours + @OTHour								
											 WHERE EmployeeId = @EmployeeId AND CAST(TransactionDate AS DATE) = CAST(@Date AS DATE)
										END

										ELSE
										-- General Day
										BEGIN
											 UPDATE EmployeeInOut
											 SET ExtraOTHours = ExtraOTHours + @OTHour																	
											 WHERE EmployeeId = @EmployeeId AND CAST(TransactionDate AS DATE) = CAST(@Date AS DATE)
										END						


									    --UPDATE EmployeeManualOverTime
									    --SET IsActive = 0
									    --WHERE EmployeeId = @EmployeeId AND CAST(Date AS DATE) = CAST(@Date AS DATE)

								FETCH NEXT FROM ManualOverTime INTO @EmployeeId, @Date, @OTHour
							COMMIT TRANSACTION TR_OverTime;
						END

					CLOSE ManualOverTime
					DEALLOCATE ManualOverTime								 					  					  														  						  											  							
END

-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2017-10-12>						*** Running Time : 12:18:13 ***
-- Description:	<> EXEC [SPProcessEmployeeInOut_NoPenalty]  '2019-12-26','2020-1-25', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOut_NoPenalty]


								     @FromDate DATE = '2017-04-26'
									,@ToDate DATE = '2017-05-25'
									,@EmployeeCardId NVARCHAR(10)


AS
BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM EmployeeInOut_NoPenalty 
							WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
							AND (@EmployeeCardId = '' OR @EmployeeCardId = @EmployeeCardId)
			

							INSERT INTO [dbo].[EmployeeInOut_NoPenalty]
							(
								[CompanyId]
							   ,[CompanyName]
							   ,[CompanyAddress]
							   ,[BranchId]
							   ,[BranchName]
							   ,[BranchUnitId]
							   ,[UnitName]
							   ,[BranchUnitDepartmentId]
							   ,[DepartmentName]
							   ,[DepartmentSectionId]
							   ,[SectionName]
							   ,[DepartmentLineId]
							   ,[LineName]
							   ,[EmployeeId]
							   ,[EmployeeCardId]
							   ,[EmployeeName]
							   ,[EmployeeTypeId]
							   ,[EmployeeType]
							   ,[EmployeeGradeId]
							   ,[EmployeeGrade]
							   ,[EmployeeDesignationId]
							   ,[EmployeeDesignation]
							   ,[JoiningDate]
							   ,[QuitDate]
							   ,[BranchUnitWorkShiftId]
							   ,[WorkShiftName]
							   ,[EmployeeWorkShiftId]
							   ,[MobileNo]
							   ,[TransactionDate]
							   ,[InTime]
							   ,[OutTime]
							   ,[LastDayOutTime]
							   ,[Status]
							   ,[LateInMinutes]
							   ,[TotalContinuousAbsentDays]
							   ,[OTHours]
							   ,[LastDayOTHours]
							   ,[ExtraOTHours]
							   ,[LastDayExtraOTHours]
							   ,[WeekendOTHours]
							   ,[HolidayOTHours]
							   ,[Remarks]
							   ,[CreatedDate]
							   ,[CreatedBy]
							   ,[IsActive]
						   )

						  SELECT 
							   [CompanyId]
							  ,[CompanyName]
							  ,[CompanyAddress]
							  ,[BranchId]
							  ,[BranchName]
							  ,[BranchUnitId]
							  ,[UnitName]
							  ,[BranchUnitDepartmentId]
							  ,[DepartmentName]
							  ,[DepartmentSectionId]
							  ,[SectionName]
							  ,[DepartmentLineId]
							  ,[LineName]
							  ,[EmployeeId]
							  ,[EmployeeCardId]
							  ,[EmployeeName]
							  ,[EmployeeTypeId]
							  ,[EmployeeType]
							  ,[EmployeeGradeId]
							  ,[EmployeeGrade]
							  ,[EmployeeDesignationId]
							  ,[EmployeeDesignation]
							  ,[JoiningDate]
							  ,[QuitDate]
							  ,[BranchUnitWorkShiftId]
							  ,[WorkShiftName]
							  ,[EmployeeWorkShiftId]
							  ,[MobileNo]
							  ,[TransactionDate]
							  ,[InTime]
							  ,[OutTime]
							  ,[LastDayOutTime]
							  ,[Status]
							  ,[LateInMinutes]
							  ,[TotalContinuousAbsentDays]
							  ,[OTHours]
							  ,[LastDayOTHours]
							  ,[ExtraOTHours]
							  ,[LastDayExtraOTHours]
							  ,[WeekendOTHours]
							  ,[HolidayOTHours]
							  ,[Remarks]
							  ,[CreatedDate]
							  ,[CreatedBy]
							  ,[IsActive]
				    FROM [dbo].[EmployeeInOut]
				    WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				    AND (@EmployeeCardId = '' OR [EmployeeCardId] = @EmployeeCardId)


					UPDATE EmployeeInOut_NoPenalty
					SET OTHours = 0
					WHERE OTHours IS NULL 
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate

					UPDATE EmployeeInOut_NoPenalty
					SET [ExtraOTHours] = 0
					WHERE [ExtraOTHours] IS NULL 
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
			

					-- ***************************** MORNING SHIFT *****************************

					--UPDATE EmployeeInOut_NoPenalty
					--SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('22:00:00.0000000' AS TIME))					
					--   ,[ExtraOTHours] = 2 
				 --   WHERE [ExtraOTHours] > 2 AND OTHours = 2
					--AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					--AND (Status = 'Present' OR Status = 'Late')
					--AND [WorkShiftName] = 'Morning'				
						
			
					--UPDATE EmployeeInOut_NoPenalty	-- Manual OverTime 				 
					--SET  [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('20:00:00.0000000' AS TIME))
					--   ,[ExtraOTHours] = 2
				 --   WHERE [ExtraOTHours] > 2 AND OTHours = 0
					--AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					--AND (Status = 'Present' OR Status = 'Late')
					--AND [WorkShiftName] = 'Morning'	

					
					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('20:00:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 2 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Morning'	
					

					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:30:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 1.5 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Morning'	


					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 1 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Morning'	


					--UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					--SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))					 
				 --   WHERE [ExtraOTHours] = 0 AND OTHours = 0 AND OutTime IS NOT NULL
					--AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					--AND (Status = 'Present' OR Status = 'Late')
					--AND [WorkShiftName] = 'Morning'	AND EmployeeTypeId IN(4,5)


					-- For middle management with in 10PM (Morning)

					UPDATE EmployeeInOut_NoPenalty  
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('22:00:00.0000000' AS TIME))					
				    WHERE OutTime > CAST('22:00:00.0000000' AS TIME)
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)	
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND EmployeeTypeId IN(1, 2, 3)
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Morning'	

					-- ***************************** EVENING SHIFT *****************************

					--UPDATE EmployeeInOut_NoPenalty
					--SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					
					--   ,[ExtraOTHours] = 2 
				 --   WHERE [ExtraOTHours] > 2 AND OTHours = 2
					--AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					--AND (Status = 'Present' OR Status = 'Late')
					--AND [WorkShiftName] = 'Evening'				
											

					--UPDATE EmployeeInOut_NoPenalty	-- Manual OverTime 				 
					--SET  [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))
					--   ,[ExtraOTHours] = 2
				 --   WHERE [ExtraOTHours] > 2 AND OTHours = 0
					--AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					--AND (Status = 'Present' OR Status = 'Late')
					--AND [WorkShiftName] = 'Evening'	

					
					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 2 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Evening'	
					

					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:30:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 1.5 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Evening'	


					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:00:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 1 AND OTHours = 0
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Evening'	


					UPDATE EmployeeInOut_NoPenalty   -- Manual OverTime
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:00:00.0000000' AS TIME))					 
				    WHERE [ExtraOTHours] = 0 AND OTHours = 0 AND OutTime IS NOT NULL
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Evening'	AND EmployeeTypeId IN(4,5)
							

					UPDATE EmployeeInOut_NoPenalty	-- Holiday
					SET InTime = NULL
					   ,OutTime = NULL
					   ,LateInMinutes = NULL				
					   ,HolidayOTHours = 0
					   ,ExtraOTHours = 0
					   ,OTHours = 0
					WHERE  Status = 'Holiday'
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate	
								
																					

					-- For middle management with in 10PM (Evening)

					UPDATE EmployeeInOut_NoPenalty  
					SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					
				    WHERE OutTime > CAST('08:00:00.0000000' AS TIME)
					AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)	
					AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
					AND EmployeeTypeId IN(1,2,3)
					AND (Status = 'Present' OR Status = 'Late')
					AND [WorkShiftName] = 'Evening'	



				  -- Attendance Penalty do not have OT and Extra OT : Present -> Absent 

					 DECLARE @EmployeeId UNIQUEIDENTIFIER
					 DECLARE @Date DATETIME
					 DECLARE @PenaltyCursor AS CURSOR

					 SET @PenaltyCursor = CURSOR FOR 
						SELECT EmployeeId, PenaltyDate FROM HrmPenalty WHERE CAST(PenaltyDate AS DATE) BETWEEN @FromDate AND @ToDate AND PenaltyTypeId = 2 AND IsActive = 1

					 OPEN @PenaltyCursor
					 FETCH NEXT FROM @PenaltyCursor INTO @EmployeeId, @Date

					 WHILE @@FETCH_STATUS = 0
					 BEGIN
 							UPDATE EmployeeInOut_NoPenalty   
							SET  Status = 'Absent'	
								,LateInMinutes = 0
								,InTime = NULL
								,OutTime = NULL												 
							WHERE OTHours = 0 AND ExtraOTHours = 0								
							AND CAST(TransactionDate AS DATE) = @Date 	
							AND EmployeeId = @EmployeeId

					 FETCH NEXT FROM @PenaltyCursor INTO @EmployeeId, @Date
					 END

					 CLOSE @PenaltyCursor
					 DEALLOCATE @PenaltyCursor
								
																												 
					--OUT TIME ROUNDING
				    --UPDATE EmployeeInOut_NoPenalty		
					--SET InTime = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:50:00.0000000' AS TIME))
					--WHERE CAST(InTime AS TIME) BETWEEN CAST('07:00:00.0000000' AS TIME) AND CAST('08:40:00.0000000' AS TIME)
					--AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate											

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END







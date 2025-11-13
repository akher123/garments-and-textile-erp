
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2019-10-15>						*** Running Time : 2 seconds ***
-- Description:	<> EXEC SPProcessEmployeeInOut_NoPenalty_Dyeing  3, '2019-10-01','2019-10-31', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOut_NoPenalty_Dyeing]


									 @BranchUnitId INT
								    ,@FromDate DATE 
									,@ToDate DATE 
									,@EmployeeCardId NVARCHAR(10)

AS

BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM EmployeeInOut_NoPenalty 
							WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
							AND (@EmployeeCardId = '' OR @EmployeeCardId = @EmployeeCardId)
							AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
			
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
				  AND EmployeeInOut.BranchUnitId = @BranchUnitId
				  


				  --FOR MORNING SHIFT (8-5)

				  --UPDATE EmployeeInOut_NoPenalty
				  --SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))	
				  --   ,OTHours = 2				   
				  --WHERE OTHours >= 2 
			   --   AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  --AND (Status = 'Present' OR Status = 'Late')
				  --AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  --AND [WorkShiftName] = 'Morning'
				  --AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId 
				  --AND BranchUnitWorkShiftId = 48


				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))
					  ,OTHours = 1.5					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48
					

				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))
				  ,OTHours = 1					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48
				   

				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:30:00.0000000' AS TIME))
				  ,OTHours = .5					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48


				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48


				  --FOR MORNING SHIFT -- GENERAL (9-6)

				  --UPDATE EmployeeInOut_NoPenalty
				  --SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('20:00:00.0000000' AS TIME))
				  --,OTHours = 2					   
				  --WHERE OTHours >= 2 
			   --   AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  --AND (Status = 'Present' OR Status = 'Late')
				  --AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  --AND [WorkShiftName] = 'Morning'
				  --AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId 
				  --AND BranchUnitWorkShiftId IN (43,46,47)


				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:30:00.0000000' AS TIME))
				  ,OTHours = 1.5					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)
					

				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))
				  ,OTHours = 1					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)
				   

				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))
				  ,OTHours = .5					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)	


				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)



				  --- EVENING SHIFT (9PM to 5AM)
				  
				  --UPDATE EmployeeInOut_NoPenalty
				  --SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:00:00.0000000' AS TIME))
				  --,OTHours = 2					   
				  --WHERE OTHours >= 2 
			   --   AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  --AND (Status = 'Present' OR Status = 'Late')
				  --AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  --AND [WorkShiftName] = 'Evening'
				  --AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  --AND BranchUnitWorkShiftId IN (49)
				  
				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('05:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (49)		

				  ----------------------------

				  --UPDATE EmployeeInOut_NoPenalty
				  --SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))
				  --,OTHours = 2					   
				  --WHERE OTHours >= 2 
			   --   AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  --AND (Status = 'Present' OR Status = 'Late')
				  --AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  --AND [WorkShiftName] = 'Evening'
				  --AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  --AND BranchUnitWorkShiftId IN (11)
				  
				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (11)

				  ------------------------------------------------

				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('10:00:00.0000000' AS TIME))
				  ,OTHours = 2					   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (50)
				  
				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (50)


				  UPDATE EmployeeInOut_NoPenalty   --- HOLIDAY				   
				  SET InTime = NULL
				     ,[OutTime] = NULL
				     ,OTHours = 0
				     ,ExtraOTHours = 0
				     ,LateInMinutes = NULL	
				     ,Remarks = ''				   				   
			      WHERE (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND Status = 'Holiday'
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				
				  -- Newly Added --
				  UPDATE EmployeeInOut_NoPenalty
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('09:00:00.0000000' AS TIME))
				     ,ExtraOTHours = 1
				  WHERE ExtraOTHours > 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND (DATENAME(dw, TransactionDate) = 'Thursday') 
				  AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId	


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
							AND EmployeeInOut_NoPenalty.BranchUnitId = @BranchUnitId

					 FETCH NEXT FROM @PenaltyCursor INTO @EmployeeId, @Date
					 END

					 CLOSE @PenaltyCursor
					 DEALLOCATE @PenaltyCursor

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END

-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2019-10-15>						*** Running Time : 2 seconds ***
-- Description:	<> EXEC [SPProcessEmployeeInOut_Original_NoWeekend_Dyeing]  3, '2019-09-01','2019-09-30', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOut_Original_NoWeekend_Dyeing]


									 @BranchUnitId INT
								    ,@FromDate DATE 
									,@ToDate DATE 
									,@EmployeeCardId NVARCHAR(10)

AS

BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM EmployeeInOut_Original_NoWeekend 
							WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
							AND (@EmployeeCardId = '' OR @EmployeeCardId = @EmployeeCardId)
							AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
			
							INSERT INTO [dbo].[EmployeeInOut_Original_NoWeekend]
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

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))	
				     ,OTHours = 2				   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId 
				  AND BranchUnitWorkShiftId = 48


				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))
					  ,OTHours = 1.5					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48
					

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))
				  ,OTHours = 1					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48
				   

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:30:00.0000000' AS TIME))
				  ,OTHours = .5					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48


				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId = 48


				  --FOR MORNING SHIFT -- GENERAL (9-6)

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('20:00:00.0000000' AS TIME))
				  ,OTHours = 2					   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId 
				  AND BranchUnitWorkShiftId IN (43,46,47)


				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:30:00.0000000' AS TIME))
				  ,OTHours = 1.5					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)
					

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))
				  ,OTHours = 1					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)
				   

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))
				  ,OTHours = .5					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)	


				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND [WorkShiftName] = 'Morning'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (43,46,47)



				  --- EVENING SHIFT (9PM to 5AM)
				  
				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:00:00.0000000' AS TIME))
				  ,OTHours = 2					   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (49)
				  
				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('05:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (49)		

				  ----------------------------

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))
				  ,OTHours = 2					   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (11)
				  
				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (11)

				  ------------------------------------------------

				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('10:00:00.0000000' AS TIME))
				  ,OTHours = 2					   
				  WHERE OTHours >= 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (50)
				  
				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId
				  AND BranchUnitWorkShiftId IN (50)


				  UPDATE EmployeeInOut_Original_NoWeekend   --- WEEKEND AND HOLIDAY				   
				  SET InTime = NULL
				  ,[OutTime] = NULL
				  ,OTHours = 0
				  ,ExtraOTHours = 0
				  ,WeekendOTHours = 0
				  ,LateInMinutes = NULL	
				  ,Remarks = ''				   				   
			      WHERE (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Weekend' OR Status = 'Holiday')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				
				  -- Newly Added --
				  UPDATE EmployeeInOut_Original_NoWeekend
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('09:00:00.0000000' AS TIME))
				     ,ExtraOTHours = 1
				  WHERE ExtraOTHours > 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'	
				  AND (DATENAME(dw, TransactionDate) = 'Thursday') 
				  AND EmployeeInOut_Original_NoWeekend.BranchUnitId = @BranchUnitId	

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END
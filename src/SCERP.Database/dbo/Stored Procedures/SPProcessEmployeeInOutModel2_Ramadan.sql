
-- ==========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>						*** Running Time : 2 seconds ***
-- Description:	<> EXEC [SPProcessEmployeeInOutModel2_Ramadan]  '2018-05-18','2018-05-25', ''
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOutModel2_Ramadan]


								     @FromDate DATE 
									,@ToDate DATE 
									,@EmployeeCardId NVARCHAR(10)

AS
BEGIN
		BEGIN TRAN

		BEGIN	
			
							DELETE FROM EmployeeInOutModel 
							WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
							AND (@EmployeeCardId = '' OR @EmployeeCardId = @EmployeeCardId)
			

							INSERT INTO [dbo].[EmployeeInOutModel]
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
							   --,[ExtraOTHours]
							   --,[LastDayExtraOTHours]
							   --,[WeekendOTHours]
							   --,[HolidayOTHours]
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
							  --,[ExtraOTHours]
							  --,[LastDayExtraOTHours]
							  --,[WeekendOTHours]
							  --,[HolidayOTHours]
							  ,[Remarks]
							  ,[CreatedDate]
							  ,[CreatedBy]
							  ,[IsActive]
				  FROM [dbo].[EmployeeInOut]
				  WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND (@EmployeeCardId = '' OR [EmployeeCardId] = @EmployeeCardId)
				  


				  -- ***************************** MORNING SHIFT(BUW - 24   Time- 7:00 AM - 3:30 PM) *****************************

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 24	


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 24	
					

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('16:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 24	
				   

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('16:00:00.0000000' AS TIME))					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 24	


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('15:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND BranchUnitWorkShiftId = 24	


				   -- ***************************** MORNING SHIFT(BUW - 25   Time- 8:00 AM - 4:30 PM) *****************************

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 25	


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 25	
					

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 25	
				   

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('17:00:00.0000000' AS TIME))					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 25


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('16:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND BranchUnitWorkShiftId = 25



				  -- ***************************** EVENING SHIFT (BUW - 45   Time- 22:00 PM - 06:00 AM) *****************************
				  
				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 45	


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 45	
					

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 45	
				   

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:30:00.0000000' AS TIME))					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 45	


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND BranchUnitWorkShiftId = 45				


				  UPDATE EmployeeInOutModel   --- WEEKEND AND HOLIDAY				   
				  SET InTime = NULL
				  ,[OutTime] = NULL
				  ,OTHours = 0
				  ,LateInMinutes = NULL					   				   
			      WHERE (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Weekend' OR Status = 'Holiday')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				

				  EXEC [SPProcessEmployeeInOutModel2Dyeing]  3, @FromDate, @ToDate, @EmployeeCardId

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END
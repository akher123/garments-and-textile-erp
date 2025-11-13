
-- ==========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>						*** Running Time : 2 seconds ***
-- Description:	<> EXEC [SPProcessEmployeeInOutModel2]  '2020-05-26','2020-06-25', '7952'
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeInOutModel2]


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
							AND BranchUnitId IN(1,2)

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
							  ,[Remarks]
							  ,[CreatedDate]
							  ,[CreatedBy]
							  ,[IsActive]
				  FROM [dbo].[EmployeeInOut]
				  WHERE CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND (@EmployeeCardId = '' OR [EmployeeCardId] = @EmployeeCardId)
				  AND BranchUnitId IN(1,2)


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('20:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
					

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('19:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'
				   

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:30:00.0000000' AS TIME))					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Morning'


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('18:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND InTime IS NOT NULL AND OutTime IS NOT NULL
				  AND [WorkShiftName] = 'Morning'


				  --- EVENING SHIFT 
				  

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('08:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 2 
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:30:00.0000000' AS TIME))					   
				  WHERE OTHours = 1.5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
					

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('07:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 1
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'
				   

				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:30:00.0000000' AS TIME))					   
				  WHERE OTHours = .5
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'


				  UPDATE EmployeeInOutModel
				  SET [OutTime] = DATEADD(MINUTE, CAST(ABS(CHECKSUM(NEWID()) % 4) + 3 AS INT), CAST('06:00:00.0000000' AS TIME))					   
				  WHERE OTHours = 0
			      AND (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Present' OR Status = 'Late')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				  AND [WorkShiftName] = 'Evening'			


				  UPDATE EmployeeInOutModel   --- WEEKEND AND HOLIDAY				   
				  SET InTime = NULL
				  ,[OutTime] = NULL
				  ,OTHours = 0
				  ,LateInMinutes = NULL	
				  ,Remarks = ''					   				   
			      WHERE (@EmployeeCardId = '' OR EmployeeCardId = @EmployeeCardId)
				  AND (Status = 'Weekend' OR Status = 'Holiday')
				  AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
				

				  -- EXEC [SPProcessEmployeeInOutModel2Dyeing]  3, @FromDate, @ToDate, @EmployeeCardId

		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END
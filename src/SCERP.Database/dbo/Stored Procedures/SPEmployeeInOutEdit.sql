
-- ===========================================================================================================
-- Author:		<Yasir>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPEmployeeInOutEdit] 'EmployeeInOut_10PM', '0835','2017-06-26'
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPEmployeeInOutEdit]
						

									  @JobCardName			NVARCHAR(50)
									 ,@EmployeeCardId		NVARCHAR(10)		
									 ,@FromDate				DATETIME					
									 ,@ToDate				DATETIME	

AS

BEGIN
	
	SET NOCOUNT ON;
			 

					IF(@JobCardName = 'EmployeeJobCardModel')
					BEGIN
							
							SELECT [Id]
								  ,[CompanyId]
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
							  FROM [dbo].[EmployeeInOutModel]
							  WHERE EmployeeCardId = @EmployeeCardId 			
							  AND CAST(TransactionDate AS DATE) BETWEEN  @FromDate AND @ToDate
							  AND IsActive = 1
							  ORDER BY TransactionDate
					  END

					IF(@JobCardName = 'EmployeeJobCard_10PM')
					BEGIN
							
							SELECT [Id]
								  ,[CompanyId]
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
							  FROM [dbo].[EmployeeInOut_10PM]
							  WHERE EmployeeCardId = @EmployeeCardId 			
							  AND CAST(TransactionDate AS DATE) BETWEEN  @FromDate AND @ToDate
							  AND IsActive = 1
							  ORDER BY TransactionDate
					  END

					IF(@JobCardName = 'EmployeeJobCard_10PM_NoWeekend')
					BEGIN
							SELECT [Id]
								  ,[CompanyId]
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
							  FROM [dbo].[EmployeeInOut_10PM_NoWeekend]
							  WHERE EmployeeCardId = @EmployeeCardId 
							  AND CAST(TransactionDate AS DATE) BETWEEN  @FromDate AND @ToDate
							  AND IsActive = 1
							  ORDER BY TransactionDate
					END

					IF(@JobCardName = 'EmployeeJobCard_Original_NoPenalty')
					BEGIN
							SELECT [Id]
								  ,[CompanyId]
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
							  FROM [dbo].[EmployeeInOut_NoPenalty]
							  WHERE EmployeeCardId = @EmployeeCardId 
							  AND CAST(TransactionDate AS DATE) BETWEEN  @FromDate AND @ToDate
							  AND IsActive = 1
							  ORDER BY TransactionDate
					END

					IF(@JobCardName = 'EmployeeJobCard_Original_NoWeekend')
					BEGIN
							SELECT [Id]
								  ,[CompanyId]
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
							  FROM [dbo].[EmployeeInOut_Original_NoWeekend]
							  WHERE EmployeeCardId = @EmployeeCardId 
							  AND CAST(TransactionDate AS DATE) BETWEEN  @FromDate AND @ToDate
							  AND IsActive = 1
							  ORDER BY TransactionDate
					END


END

-- ===========================================================================================================
-- Author:		<>
-- Create date: <15-Sep-15 2:09:40 PM>
-- Description:	<>	EXEC [SPGetJobCardSummary] 1, 1, 1, 6, 6, 11, 4, '', '2018-01-26', '2018-02-25'
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPGetJobCardSummary]
										
											
									 @CompanyId		            INT = NULL
									,@BranchId	      	        INT = NULL
									,@BranchUnitId		        INT = NULL
									,@BranchUnitDepartmentId    INT = NULL
									,@SectionId					INT = NULL
									,@LineId					INT = NULL
									,@EmployeeTypeId			INT = NULL
									,@employeeCardId			NVARCHAR(100) = NULL
								    ,@FromDate					DATETIME 
									,@ToDate					DATETIME 

AS


BEGIN
	
	SET NOCOUNT ON
						
								SELECT [EmployeeInOut].[Id]
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
									  ,[EmployeeInOut].[EmployeeId]
									  ,[EmployeeInOut].[EmployeeCardId]
									  ,[EmployeeName]
									  ,[EmployeeTypeId]
									  ,[EmployeeType]
									  ,[EmployeeGradeId]
									  ,[EmployeeGrade]
									  ,[EmployeeDesignationId]
									  ,[EmployeeDesignation]
									  ,[EmployeeInOut].[JoiningDate]
									  ,[EmployeeInOut].[QuitDate]
									  ,[BranchUnitWorkShiftId]
									  ,[WorkShiftName]
									  ,[EmployeeWorkShiftId]
									  ,[MobileNo]	
									  ,[TransactionDate]						  
									  ,DAY(TransactionDate) AS Date
									  ,CONVERT(VARCHAR(5),[InTime], 108) AS InTime						
									  ,OutTime =  
											  CASE   
												 WHEN [EmployeeInOut].Status = 'Absent' THEN 'A'  
												 WHEN [EmployeeInOut].Status = 'Weekend' AND [EmployeeInOut].OutTime IS NULL AND [EmployeeInOut].InTime IS NULL  THEN 'W'  
												 WHEN [EmployeeInOut].Status = 'Holiday' THEN 'H'  
												 WHEN [EmployeeInOut].Status = 'Leave' THEN 'L'  
												 WHEN HrmPenalty.IsActive = 1 THEN 'AP'												 
												 ELSE CONVERT(VARCHAR(5),[OutTime], 108) 
											  END
									  ,(ISNULL([OTHours], 0) + ISNULL([ExtraOTHours], 0) + ISNULL([WeekendOTHours], 0) + ISNULL([HolidayOTHours], 0)) AS TotalOTHours
									  ,DATENAME(MONTH, @ToDate)  AS Month
									  ,DATENAME(YEAR, @ToDate)  AS Year
									  ,[LastDayOutTime]
									  ,[EmployeeInOut].[Status]
									  ,[LateInMinutes]
									  ,[TotalContinuousAbsentDays]
									  ,[OTHours]
									  ,[LastDayOTHours]
									  ,[ExtraOTHours]
									  ,[LastDayExtraOTHours]
									  ,[WeekendOTHours]
									  ,[HolidayOTHours]
									  ,[Remarks]
									  ,[EmployeeInOut].[CreatedDate]
									  ,[EmployeeInOut].[CreatedBy]
									  ,[EmployeeInOut].[IsActive]
									 
								  FROM [dbo].[EmployeeInOut]
								  LEFT JOIN Employee ON Employee.EmployeeId = EmployeeInOut.EmployeeId
								  LEFT JOIN HrmPenalty ON HrmPenalty.EmployeeId = EmployeeInOut.EmployeeId AND CAST(HrmPenalty.PenaltyDate AS DATE) = CAST(EmployeeInOut.TransactionDate AS DATE) AND HrmPenalty.PenaltyTypeId = 2
								  
								  WHERE (EmployeeInOut.CompanyId = @CompanyId OR @CompanyId = 0)
									AND (EmployeeInOut.BranchId = @BranchId OR @BranchId = 0)
									AND (EmployeeInOut.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									AND ((EmployeeInOut.BranchUnitDepartmentId = @BranchUnitDepartmentId) OR (@BranchUnitDepartmentId = 0))
									AND ((EmployeeInOut.DepartmentSectionId = @SectionId) OR (@SectionId = 0))
									AND ((EmployeeInOut.DepartmentLineId = @LineId) OR (@LineId = 0))									
									AND ((EmployeeInOut.EmployeeTypeId = @EmployeeTypeId) OR (@EmployeeTypeId = 0))
									AND ((EmployeeInOut.EmployeeCardId = @employeeCardId) OR (@employeeCardId =''))
									AND CAST(TransactionDate AS DATE) BETWEEN @FromDate AND @ToDate
									AND (Employee.QuitDate >= @FromDate OR Employee.QuitDate IS NULL)
								  												  
								  ORDER BY TransactionDate,[EmployeeInOut].EmployeeCardId

END
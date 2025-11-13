-- ================================================================================================================================
-- Author	   : Yasir Arafat
-- Create date : 2018/04/29
-- Description : EXEC [SPGetEmployeeAttendanceSummaryByDesignation3]  1, 1, 1, 1, -1, -1, -1, 46, '2018-05-17', 'superadmin'
-- ================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeAttendanceSummaryByDesignation3]


								 @companyId INT = -1
								,@branchId INT = -1
								,@branchUnitId INT = -1
								,@branchUnitDepartmentId INT = -1
								,@departmentSectionId INT = -1
								,@departmentLineId INT = -1
								,@employeeTypeId INT = -1
								,@branchUnitWorkShiftId INT
								,@TransactionDate DateTime
								,@UserName NVARCHAR(100)

AS

BEGIN
	
				SET NOCOUNT ON;
							
							CREATE TABLE #EmployeeInOut
							(									
									[CompanyId] [int] NULL,
									[CompanyName] [nvarchar](100) NULL,
									[CompanyAddress] [nvarchar](max) NULL,
									[BranchId] [int] NULL,
									[BranchName] [nvarchar](100) NULL,
									[BranchUnitId] [int] NULL,
									[UnitName] [nvarchar](100) NULL,
									[BranchUnitDepartmentId] [int] NULL,
									[DepartmentName] [nvarchar](100) NULL,
									[DepartmentSectionId] [int] NULL,
									[SectionName] [nvarchar](100) NULL,
									[DepartmentLineId] [int] NULL,
									[LineName] [nvarchar](100) NULL,
									[EmployeeId] [uniqueidentifier] NOT NULL,
									[EmployeeCardId] [nvarchar](100) NOT NULL,
									[EmployeeName] [nvarchar](100) NOT NULL,
									[EmployeeTypeId] [int] NULL,
									[EmployeeType] [nvarchar](100) NULL,
									[EmployeeGradeId] [int] NULL,
									[EmployeeGrade] [nvarchar](100) NULL,
									[EmployeeDesignationId] [int] NULL,
									[EmployeeDesignation] [nvarchar](100) NULL,
									[JoiningDate] [datetime] NULL,
									[QuitDate] [datetime] NULL,
									[BranchUnitWorkShiftId] [int] NULL,
									[WorkShiftName] [nvarchar](100) NULL,
									[EmployeeWorkShiftId] [int] NULL,
									[MobileNo] [nvarchar](100) NULL,
									[TransactionDate] [date] NOT NULL,
									[InTime] [time](7) NULL,
									[OutTime] [time](7) NULL,
									[LastDayOutTime] [time](7) NULL,
									[Status] [nvarchar](100) NULL,
									[LateInMinutes] [int] NULL,
									[TotalContinuousAbsentDays] [int] NULL,
									[OTHours] [numeric](18, 2) NULL,
									[LastDayOTHours] [numeric](18, 2) NULL,
									[ExtraOTHours] [numeric](18, 2) NULL,
									[LastDayExtraOTHours] [numeric](18, 2) NULL,
									[WeekendOTHours] [numeric](18, 2) NULL,
									[HolidayOTHours] [numeric](18, 2) NULL,
									[Remarks] [nvarchar](100) NULL,
									[CreatedDate] [datetime] NULL,
									[CreatedBy] [uniqueidentifier] NULL,
									[IsActive] [bit] NULL
							)

							CREATE TABLE  #TempEmployeeInOut
							(
								 TransactionDate		DATETIME					
								,CompanyName			NVARCHAR(100)
								,CompanyAddress			NVARCHAR(100)
								,BranchName				NVARCHAR(100)
								,UnitName				NVARCHAR(100)
								,DepartmentName			NVARCHAR(100)
								,SectionName			NVARCHAR(100)
								,LineName				NVARCHAR(100)
								,EmployeeTypeName		NVARCHAR(100)
								,WorkShift				NVARCHAR(100)
								,EmployeeType			NVARCHAR(100)
								,EmployeeDesignation	NVARCHAR(100)
								,EmployeeDesignationId  INT
								,TotalEmployee			INT
								,TotalPresent			INT
								,TotalLate				INT
								,TotalAbsent			INT
								,TotalLeave				INT
								,TotalOSD				INT
								,PercentageOfPresent	NVARCHAR(100)						
							)

						INSERT INTO #EmployeeInOut						 
						SELECT [CompanyId]
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
							  ,ISNULL([LineName], 'Line')
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
						  WHERE (CompanyId = @companyId OR @companyId = -1)
							AND (BranchId = @branchId OR @branchId = -1)
							AND (BranchUnitId = @branchUnitId OR @branchUnitId = -1)
							AND (BranchUnitDepartmentId = @branchUnitDepartmentId OR @branchUnitDepartmentId = -1)
							AND (DepartmentSectionId = @departmentSectionId OR @departmentSectionId = -1)
							AND (DepartmentLineId = @departmentLineId OR @departmentLineId = -1)
							AND (EmployeeTypeId = @employeeTypeId OR @employeeTypeId = -1)
							AND (BranchUnitWorkShiftId = @branchUnitWorkShiftId OR @branchUnitWorkShiftId = -1)
							AND CAST(TransactionDate AS DATE) = @TransactionDate
							AND IsActive = 1

						INSERT INTO #TempEmployeeInOut					
						SELECT @TransactionDate							
							  ,[CompanyName]
							  ,[CompanyAddress]				
							  ,[BranchName]			
							  ,[UnitName]	
							  ,[DepartmentName]				
							  ,[SectionName]											
							  ,' '				
							  ,[EmployeeType]
							  ,[WorkShiftName]
							  ,[EmployeeType]						
							  ,[EmployeeDesignation]
							  ,EmployeeDesignationId
							  ,0
							  ,0
							  ,0
							  ,0
							  ,0
							  ,0
							  ,0.0												
						  FROM [dbo].[EmployeeInOut]

						  WHERE (CompanyId = @companyId OR @companyId = -1)
							AND (BranchId = @branchId OR @branchId = -1)
							AND (BranchUnitId = @branchUnitId OR @branchUnitId = -1)
							AND (BranchUnitDepartmentId = @branchUnitDepartmentId OR @branchUnitDepartmentId = -1)
							AND (DepartmentSectionId = @departmentSectionId OR @departmentSectionId = -1)
							AND (DepartmentLineId = @departmentLineId OR @departmentLineId = -1)
							AND (EmployeeTypeId = @employeeTypeId OR @employeeTypeId = -1)
							AND (BranchUnitWorkShiftId = @branchUnitWorkShiftId OR @branchUnitWorkShiftId = -1)
							AND CAST(TransactionDate AS DATE) = @TransactionDate
							AND IsActive = 1
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																			
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId
									

															-- Update Total Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalEmployee = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId	
						  AND  #EmployeeInOut.SectionName = #TempEmployeeInOut.SectionName								
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																		
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)

															-- Update Present Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalPresent = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId					
						  AND (#EmployeeInOut.Status = 'Present' OR #EmployeeInOut.Status = 'Late')
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																		
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)

															-- Update Late Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalLate = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId					
						  AND (#EmployeeInOut.Status = 'Late')
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																		
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)

															-- Update Absent Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalAbsent = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId						
						  AND (#EmployeeInOut.Status = 'Absent')
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																			
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)
						
															-- Update Leave Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalLeave = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId					
						  AND (#EmployeeInOut.Status = 'Leave')
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																		
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)

															-- Update OSD Employee
						  UPDATE #TempEmployeeInOut
						  SET TotalOSD = (SELECT SUM(1)																											
						  FROM #EmployeeInOut						   
						  WHERE #EmployeeInOut.EmployeeDesignationId = #TempEmployeeInOut.EmployeeDesignationId					
						  AND (#EmployeeInOut.Status = 'OSD')
						  GROUP BY     [CompanyName]
									  ,[CompanyAddress]				
									  ,[BranchName]			
									  ,[UnitName]	
									  ,[DepartmentName]				
									  ,[SectionName]																				
									  ,[EmployeeType]
									  ,[WorkShiftName]
									  ,[EmployeeType]						
									  ,[EmployeeDesignation]
									  ,EmployeeDesignationId)
								
															-- Update Present Percentage Employee
						UPDATE #TempEmployeeInOut
						SET PercentageOfPresent = (CONVERT(VARCHAR(10),ROUND(CONVERT(DECIMAL(18,2),(((TotalPresent)*100.00)/(TotalEmployee))),0),103) + '%')																	

						SELECT @TransactionDate				AS TransactionDate					
							  ,[CompanyName]
							  ,[CompanyAddress]				
							  ,[BranchName]			
							  ,[UnitName]	
							  ,[DepartmentName]				
							  ,[SectionName]						
							  ,[LineName]						
							  ,[EmployeeType]				AS EmployeeTypeName
							  ,WorkShift
							  ,[EmployeeType]						
							  ,[EmployeeDesignation]		
							  ,ISNULL(TotalEmployee, 0)		AS TotalEmployee		
							  ,ISNULL(TotalPresent, 0)		AS TotalPresent			
							  ,ISNULL(TotalLate, 0)			AS TotalLate			
							  ,ISNULL(TotalAbsent, 0)		AS TotalAbsent		
							  ,ISNULL(TotalLeave, 0)		AS TotalLeave				
							  ,ISNULL(TotalOSD, 0)			AS TotalOSD		
							  ,ISNULL(PercentageOfPresent, '00.0%' ) AS PercentageOfPresent														
						  FROM #TempEmployeeInOut
						  ORDER BY EmployeeDesignation	
								
		
				SET NOCOUNT OFF;							
																																																																																																																										 																																												 																												
END
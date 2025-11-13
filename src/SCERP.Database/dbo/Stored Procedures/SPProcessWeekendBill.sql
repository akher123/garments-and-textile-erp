
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2016-08-14>						
-- Description:	<> EXEC [SPProcessWeekendBill] '2018-11-09','2018-11-09','superadmin'
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessWeekendBill]

								
								   @FromDate DATETIME = NULL
								  ,@ToDate	 DATETIME = TIME
								  ,@UserName NVARCHAR(100)

AS

BEGIN
			
				BEGIN TRAN

								SET XACT_ABORT ON;
								SET NOCOUNT ON;
		 										
								IF(@FromDate IS NULL)
								BEGIN
									SET @FromDate = CAST(CURRENT_TIMESTAMP AS DATE)
								END
								ELSE
								BEGIN
									SET @FromDate = CAST(@FromDate AS DATE)
								END									
						
						SET FMTONLY OFF;
									
						DELETE FROM WeekendBill
						WHERE CAST(WeekendBill.Date AS DATE) = @fromDate

						DECLARE @UserID UNIQUEIDENTIFIER;
						SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;

						INSERT INTO WeekendBill
									(
									   [Date]
									  ,[EmployeeId]
									  ,[EmployeeCardId]
									  ,[EmployeeName]
									  ,[SectionName]
									  ,[EmployeeType]
									  ,[Designation]
									  ,[InTime]
									  ,[OutTime]
									  ,[BasicSalary]
									  ,[Allowance]
									  ,[CreatedDate]
									  ,[CreatedBy]
									  ,[EditedDate]
									  ,[EditedBy]
									  ,[IsActive]									
									)
									  SELECT		     
									   @fromDate
									  ,[EmployeeInOut].[EmployeeId]
									  ,[EmployeeInOut].[EmployeeCardId]
									  ,[EmployeeInOut].[EmployeeName]
									  ,[EmployeeInOut].[SectionName]
									  ,[EmployeeInOut].[EmployeeType]
									  ,[EmployeeInOut].[EmployeeDesignation]
									  ,[EmployeeInOut].[InTime]
									  ,[EmployeeInOut].[OutTime]
									  ,employeeSalary.BasicSalary
									  ,employeeSalary.BasicSalary/30
									  ,GETDATE()
									  ,@UserID
									  ,GETDATE()
									  ,@UserID
									  ,1		
									  							 																																																		 																	
									  FROM [dbo].[EmployeeInOut]
									  LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,
									  employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,
									  employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, 
									  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
									  FROM EmployeeSalary AS employeeSalary 
									  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
									  ON [EmployeeInOut].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

									  WHERE CAST([EmployeeInOut].[TransactionDate] AS DATE) = @fromDate
									  AND [EmployeeInOut].InTime IS NOT NULL
									  AND [EmployeeInOut].EmployeeId IN (SELECT DISTINCT EmployeeId FROM EmployeeDailyAttendance WHERE CAST(TransactionDateTime AS DATE) = @fromDate)
								      AND [EmployeeInOut].EmployeeTypeId IN (2,3)
									  AND [EmployeeInOut].BranchUnitId IN(1,2)
									  AND [EmployeeInOut].SectionName <> 'Security'
									  AND [EmployeeInOut].Status = 'Weekend'


									  UPDATE WeekendBill
										SET OutTime = (
										SELECT TOP(1) [TransactionDateTime] FROM EmployeeDailyAttendance 
										WHERE EmployeeId = WeekendBill.EmployeeId 
										AND CAST( EmployeeDailyAttendance.[TransactionDateTime] AS DATE) = CAST(DATEADD(DAY, 1, @FromDate) AS DATE) 
										AND CAST(EmployeeDailyAttendance.[TransactionDateTime] AS TIME) < '07:00'										
										ORDER BY TransactionDateTime
										)
									  WHERE OutTime IS NULL


									  UPDATE WeekendBill
									  SET [Allowance] = 500
									  WHERE [Allowance] > 500									 
									  
									  UPDATE WeekendBill
									  SET Allowance = 350
									  WHERE EmployeeType = 'Middle Management' AND Allowance > 350									
									  AND (Designation NOT LIKE '%Incharge%' AND Designation NOT LIKE '%Master%' AND Designation NOT LIKE '%Executive%' AND Designation NOT LIKE '%Officer%' AND Designation NOT LIKE '%Merchandiser%'  )			
									  AND CAST(WeekendBill.Date AS DATE) = @fromDate
									  
									  UPDATE WeekendBill
									  SET Allowance = 350
									  WHERE EmployeeType = 'Middle Management' AND Allowance > 350	
									  AND (Designation LIKE '%Junior%' OR Designation LIKE '%Trainee%' OR Designation LIKE '%Assistant%' )	
									  AND CAST(WeekendBill.Date AS DATE) = @fromDate
									  
				COMMIT TRAN

								 SELECT [WeekendBillId]
									  ,CONVERT(VARCHAR(15),[Date], 105) AS Date
									  ,[EmployeeId]
									  ,[EmployeeCardId]
									  ,[EmployeeName]
									  ,[SectionName]
									  ,[EmployeeType]
									  ,[Designation]
									  ,CONVERT(VARCHAR(5), [InTime], 108)  AS InTime
									  ,CONVERT(VARCHAR(5), [OutTime], 108) AS OutTime
									  ,[BasicSalary]
									  ,CEILING([Allowance]) AS [Allowance]
									  ,[CreatedDate]
									  ,[CreatedBy]
									  ,[EditedDate]
									  ,[EditedBy]
									  ,[IsActive]
								  FROM [dbo].[WeekendBill]
								  WHERE CAST([Date] AS DATE) = @FromDate
								  ORDER BY [EmployeeCardId] 

END
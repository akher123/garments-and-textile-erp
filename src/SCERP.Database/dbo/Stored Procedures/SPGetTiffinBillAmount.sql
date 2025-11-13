
-- ===========================================================================================================================
-- Author:		<>
-- Create date: <2015-10-12>						
-- Description:	<> EXEC SPGetTiffinBillAmount  1, 1, 1, 6, 8, 0, '2018-09-16', 'superadmin', false, false, false, true, false
-- ===========================================================================================================================

CREATE PROCEDURE [dbo].[SPGetTiffinBillAmount]
											
																    
								     @CompanyId					INT = 0
									,@BranchId					INT = 0
									,@BranchUnitId				INT = 0
									,@BranchUnitDepartmentId	INT = 0
									,@DepartmentSectionId		INT = 0
									,@DepartmentLineId			INT = 0
									,@FromDate					DATETIME = NULL					
								    ,@UserName					NVARCHAR(100)
									,@All						BIT
									,@Management				BIT
									,@MiddleManagement			BIT
									,@TeamMemberA				BIT
									,@TeamMemberB				BIT

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
									

									DECLARE @list TABLE (EmployeeTypeId INT)

									IF(@All = 1)
									BEGIN
											INSERT @list(EmployeeTypeId) values(2),(3),(4),(5)  
									END

									IF(@Management = 1)
									BEGIN
											INSERT @list(EmployeeTypeId) values(2) 
									END

									IF(@MiddleManagement = 1)
									BEGIN
											INSERT @list(EmployeeTypeId) values(3) 
									END

									IF(@TeamMemberA = 1)
									BEGIN
											INSERT @list(EmployeeTypeId) values(4) 
									END

									IF(@TeamMemberB = 1)
									BEGIN
											INSERT @list(EmployeeTypeId) values(5) 
									END


									DECLARE @UserID UNIQUEIDENTIFIER;
									SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;
									
									DELETE FROM TiffinBill
									WHERE CAST(TiffinBill.Date AS DATE) = @fromDate								

									INSERT INTO TiffinBill
									(							
									   [Date]
									  ,[EmployeeId]
									  ,[EmployeeCardId]
									  ,[EmployeeName]
									  ,[BranchName]
									  ,[UnitName]
									  ,[DepartmentName]
									  ,[LineName]
									  ,[SectionName]
									  ,[EmployeeType]
									  ,[Designation]
									  ,[InTime]
									  ,[OutTime]
									  ,[Amount]
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
									  ,[EmployeeInOut].BranchName
									  ,[EmployeeInOut].UnitName
									  ,[EmployeeInOut].DepartmentName
									  ,[EmployeeInOut].LineName
									  ,[EmployeeInOut].[SectionName]									 
									  ,[EmployeeInOut].[EmployeeType]
									  ,[EmployeeInOut].[EmployeeDesignation]
									  ,[EmployeeInOut].[InTime]
									  ,[EmployeeInOut].[OutTime]
									  ,0
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
									  WHERE (CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
									  ON EmployeeInOut.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  
									   
									  WHERE (EmployeeInOut.CompanyId = @CompanyId OR @CompanyId = 0)
									  AND (EmployeeInOut.BranchId = @BranchId OR @BranchId = 0)
									  AND (EmployeeInOut.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									  AND (EmployeeInOut.BranchUnitId = @BranchUnitId OR @BranchUnitId = 0)
									  AND (EmployeeInOut.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId = 0)									  
									  AND (EmployeeInOut.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId = 0)									  
									  AND (EmployeeInOut.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId = 0)									  
									  AND CAST(EmployeeInOut.TransactionDate AS DATE) = @fromDate

									  AND EmployeeInOut.OutTime IS NOT NULL
									  AND EmployeeInOut.EmployeeId IN (SELECT DISTINCT EmployeeId FROM EmployeeDailyAttendance WHERE CAST(TransactionDateTime AS DATE) = @fromDate)
									  AND EmployeeInOut.EmployeeTypeId NOT IN (1,2)
									  AND EmployeeInOut.BranchUnitId IN (1)
									  AND EmployeeInOut.SectionName <> 'Security'
									  AND EmployeeTypeId IN (SELECT EmployeeTypeId FROM @list)



									  					-- Management
									  --UPDATE TiffinBill
									  --SET Amount = 40
									  --WHERE EmployeeType = 'Management' 																	 
									  --AND CAST(Date AS DATE) = @fromDate
									  --AND OutTime BETWEEN CAST('21:50:00.000' AS TIME) AND CAST('23:49:00.000' AS TIME)

									  --UPDATE TiffinBill
									  --SET Amount = 60
									  --WHERE EmployeeType = 'Management'  																	 
									  --AND CAST(Date AS DATE) = @fromDate
									  --AND OutTime BETWEEN CAST('23:50:00.000' AS TIME) AND CAST('02:30:00.000' AS TIME)

									  --UPDATE TiffinBill
									  --SET Amount = 160
									  --WHERE EmployeeType = 'Management'  																	 
									  --AND CAST(Date AS DATE) = @fromDate
									  --AND OutTime BETWEEN CAST('02:30:01.000' AS TIME) AND CAST('07:00:00.000' AS TIME)
									 
									 									 
														-- Middle Management
									  UPDATE TiffinBill
									  SET Amount = 60
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('21:50:00.000' AS TIME) AND CAST('23:49:00.000' AS TIME)

									  UPDATE TiffinBill
									  SET Amount = 120
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND (OutTime BETWEEN CAST('23:50:00.000' AS TIME) AND CAST('23:59:59.000' AS TIME) OR OutTime BETWEEN CAST('00:00:00.000' AS TIME) AND CAST('02:30:00.000' AS TIME))

									  UPDATE TiffinBill
									  SET Amount = 240
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('02:30:01.000' AS TIME) AND CAST('07:00:00.000' AS TIME)



														-- Team Member
									  UPDATE TiffinBill
									  SET Amount = 20
									  WHERE EmployeeType LIKE '%Team%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('21:50:00.000' AS TIME) AND CAST('23:49:00.000' AS TIME)

									  UPDATE TiffinBill
									  SET Amount = 30
									  WHERE EmployeeType LIKE '%Team%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND (OutTime BETWEEN CAST('23:50:00.000' AS TIME) AND CAST('23:59:59.000' AS TIME) OR OutTime BETWEEN CAST('00:00:00.000' AS TIME) AND CAST('02:30:00.000' AS TIME))

									  UPDATE TiffinBill
									  SET Amount = 80
									  WHERE EmployeeType LIKE '%Team%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('02:30:01.000' AS TIME) AND CAST('07:00:00.000' AS TIME)


									  					--EXCEPTION : Middle Management to Management
									  UPDATE TiffinBill
									  SET Amount = 80
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('21:50:00.000' AS TIME) AND CAST('23:49:00.000' AS TIME)
									  AND (Designation LIKE '%Incharge%' AND Designation LIKE '%Master%' AND Designation LIKE '%Executive%' AND Designation LIKE '%Officer%' AND Designation LIKE '%Merchandiser%')			

									  UPDATE TiffinBill
									  SET Amount = 160
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND (OutTime BETWEEN CAST('23:50:00.000' AS TIME) AND CAST('23:59:59.000' AS TIME) OR OutTime BETWEEN CAST('00:00:00.000' AS TIME) AND CAST('02:30:00.000' AS TIME))
									  AND (Designation LIKE '%Incharge%' AND Designation LIKE '%Master%' AND Designation LIKE '%Executive%' AND Designation LIKE '%Officer%' AND Designation LIKE '%Merchandiser%')			

									  UPDATE TiffinBill
									  SET Amount = 320
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('02:30:01.000' AS TIME) AND CAST('07:00:00.000' AS TIME)
									  AND (Designation LIKE '%Incharge%' AND Designation LIKE '%Master%' AND Designation LIKE '%Executive%' AND Designation LIKE '%Officer%' AND Designation LIKE '%Merchandiser%')			
									  


									  UPDATE TiffinBill
									  SET Amount = 60
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('21:50:00.000' AS TIME) AND CAST('23:49:00.000' AS TIME)
									  AND (Designation LIKE '%Junior%' OR Designation LIKE '%Trainee%' OR Designation LIKE '%Assistant%' )	

									  UPDATE TiffinBill
									  SET Amount = 120
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND (OutTime BETWEEN CAST('23:50:00.000' AS TIME) AND CAST('23:59:59.000' AS TIME) OR OutTime BETWEEN CAST('00:00:00.000' AS TIME) AND CAST('02:30:00.000' AS TIME))
									  AND (Designation LIKE '%Incharge%' AND Designation LIKE '%Master%' AND Designation LIKE '%Executive%' AND Designation LIKE '%Officer%' AND Designation LIKE '%Merchandiser%')			

									  UPDATE TiffinBill
									  SET Amount = 240
									  WHERE EmployeeType LIKE '%Middle%' 																	 
									  AND CAST(Date AS DATE) = @fromDate
									  AND OutTime BETWEEN CAST('02:30:01.000' AS TIME) AND CAST('07:00:00.000' AS TIME)
									  AND (Designation LIKE '%Incharge%' AND Designation LIKE '%Master%' AND Designation LIKE '%Executive%' AND Designation LIKE '%Officer%' AND Designation LIKE '%Merchandiser%')			
									  									  															  													
								
							  SELECT   [TiffinBillId]
									  ,CONVERT(VARCHAR(15),[Date], 105) AS Date
									  ,[EmployeeId]
									  ,[EmployeeCardId]
									  ,[EmployeeName]
									  ,[BranchName]
									  ,[UnitName]
									  ,[DepartmentName]
									  ,[LineName]
									  ,[SectionName]
									  ,[EmployeeType]
									  ,[Designation]
									  ,CONVERT(VARCHAR(5), [InTime], 108)  AS InTime
									  ,CONVERT(VARCHAR(5), [OutTime], 108) AS OutTime
									  ,[Amount]
									  ,[CreatedDate]
									  ,[CreatedBy]
									  ,[EditedDate]
									  ,[EditedBy]
									  ,[IsActive]
								  FROM [dbo].[TiffinBill]
								  WHERE CAST([Date] AS DATE) = @FromDate 	
								  AND Amount > 0			
								  ORDER BY [EmployeeCardId], Amount DESC

			COMMIT TRAN

END
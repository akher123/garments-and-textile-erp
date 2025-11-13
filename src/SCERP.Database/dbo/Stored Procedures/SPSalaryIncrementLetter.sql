
-- ==========================================================================================================================================
-- Author:		<Yasir Arafat> 
-- Create date: <2016-11-22>					
-- Description:	<> EXEC [SPSalaryIncrementLetter] '2019-11-26','2019-12-25', '2355','superadmin'
-- ==========================================================================================================================================

CREATE PROCEDURE [dbo].[SPSalaryIncrementLetter]
					 

					    @FromDate DATETIME = NULL
					   ,@ToDate DATETIME = NULL								  
					   ,@EmployeeCardId NVARCHAR(100) = ''
					   ,@UserName NVARCHAR(100) = ''
					  
AS

BEGIN					-- Special Note : After adding new salary at employee salary table
			

			DECLARE 

						@CompanyId INT = -1
					   ,@BranchId INT = -1
					   ,@BranchUnitId INT = -1
					   ,@BranchUnitDepartmentId INT = -1
					   ,@DepartmentSectionId INT = -1 
					   ,@DepartmentLineId INT = -1
					   ,@EmployeeTypeId INT = -1
					   ,@IncrementPercent FLOAT = 5
					   ,@OtherIncrement	DECIMAL(18,2) = 0.0


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
		
						IF(@ToDate IS NULL)
						BEGIN
						SET @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)
						END
						ELSE
						BEGIN
							SET @ToDate = CAST(@ToDate AS DATE)
						END

						
						SELECT [IncrementId]
							  ,[CompanyName]
							  ,[CompanyAddress]
							  ,[BranchName]
							  ,[UnitName]
							  ,[DepartmentName]
							  ,[SectionName]
							  ,[LineName]
							  ,[EmployeeIncrement].[EmployeeId]
							  ,[EmployeeCardId]
							  ,[EmployeeName]
							  ,[EmployeeType]
							  ,[GradeName]
							  ,[DesignationName]
							  ,CONVERT(VARCHAR(10),JoiningDate, 103) JoiningDate
							  ,[QuitDate]
							  ,[MobilePhone]
							  ,[Percent]
							  ,[EmployeeIncrement].[GrossSalary]
							  ,[EmployeeIncrement].[BasicSalary]
							  ,[EmployeeIncrement].[HouseRent]
							  ,[EmployeeIncrement].[MedicalAllowance]
							  ,[EmployeeIncrement].[EntertainmentAllowance]
							  ,[EmployeeIncrement].[FoodAllowance]
							  ,[EmployeeIncrement].[Conveyance]							  
							  ,CONVERT(VARCHAR(10),DATEADD(YEAR, 1, LastIncrementDate), 103) AS LastIncrementDate
							  --,CONVERT(VARCHAR(10),DATEADD(DAY, -1, DATEADD(YEAR, 1, employeeSalary.FromDate)), 103) AS DateToday
							  ,CONVERT(VARCHAR(10),DATEADD(DAY, 1, DATEADD(YEAR, 0, employeeSalary.SalaryTodate)), 103) AS DateToday  
							  -- After adding new salary at employee salary table (for date today)
							  	
							  ,CAST([NewGross] AS DECIMAL(18)) AS [NewGross]
							  ,CAST([NewBasic] AS DECIMAL(18)) AS [NewBasic]
							  ,CAST([NewHouseRent] AS DECIMAL(18)) AS [NewHouseRent]
							  ,CAST([OtherBenefit] AS DECIMAL(18)) AS [OtherBenefit]
							  ,CAST([OtherIncrement] AS DECIMAL(18)) AS [OtherIncrement]
							  ,CAST([TotalIncrement] AS DECIMAL(18)) AS [TotalIncrement]
							  ,[ApprovedIncrement]
							  ,[Remarks]
							  ,[EmployeeIncrement].[FromDate]
							  ,[ToDate]		
							  					 
							   FROM [dbo].[EmployeeIncrement]	
							   LEFT JOIN (SELECT EmployeeId, employeeSalary.GrossSalary,  employeeSalary.BasicSalary,employeeSalary.HouseRent, employeeSalary.MedicalAllowance, employeeSalary.Conveyance,employeeSalary.FoodAllowance,employeeSalary.EntertainmentAllowance, employeeSalary.FromDate,employeeSalary.ToDate AS SalaryTodate,												
							   ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						 
							   FROM EmployeeSalary AS employeeSalary 
							   WHERE (CAST(EmployeeSalary.ToDate AS Date) <= @toDate) AND EmployeeSalary.IsActive = 1) employeeSalary 
							   ON [EmployeeIncrement].EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1  

							   WHERE EmployeeCardId = @EmployeeCardId
							   AND [EmployeeIncrement].FromDate = @FromDate
							   AND [EmployeeIncrement].ToDate = @ToDate	
							   AND [EmployeeIncrement].IsActive = 1		
							  													
			 END	
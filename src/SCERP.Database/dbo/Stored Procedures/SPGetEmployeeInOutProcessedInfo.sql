
-- ============================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>
-- Description:	<> EXEC [SPGetEmployeeInOutProcessedInfo] 0, 10000, -1, -1, -1, -1, -1, -1, '2016-05-26','2016-06-12', -1, '','superadmin'
-- ============================================================================================================================================

CREATE PROCEDURE [dbo].[SPGetEmployeeInOutProcessedInfo]

	   @StartRowIndex INT = NULL,
	   @MaxRows INT = NULL,

	   @CompanyId INT = -1,
	   @BranchId INT = -1,
	   @BranchUnitId INT = -1,
	   @BranchUnitDepartmentId INT = -1, 
	   @DepartmentSectionId INT = -1, 
	   @DepartmentLineId INT = -1, 
	   @FromDate DateTime,
	   @ToDate DateTime,
	   @EmployeeTypeId INT = -1,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @UserName NVARCHAR(100)

	   
AS
BEGIN
	
		SET XACT_ABORT ON;
		SET NOCOUNT ON;
		 
		DECLARE @ListOfCompanyIds TABLE(CompanyIDs INT);
		DECLARE @ListOfBranchIds TABLE(BranchIDs INT);
		DECLARE @ListOfBranchUnitIds TABLE(BranchUnitIDs INT);
		DECLARE @ListOfBranchUnitDepartmentIds TABLE(BranchUnitDepartmentIDs INT);
		DECLARE @ListOfEmployeeTypeIds TABLE(EmployeeTypeIDs INT);
		
		BEGIN TRAN

		IF(@CompanyId = -1)
		BEGIN
			INSERT INTO @ListOfCompanyIds
			SELECT DISTINCT CompanyId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfCompanyIds VALUES (@CompanyId)
		END

		IF(@BranchId = -1)
		BEGIN
			INSERT INTO @ListOfBranchIds
			SELECT DISTINCT BranchId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchIds VALUES (@BranchId)
		END

		IF(@BranchUnitId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitIds
			SELECT DISTINCT BranchUnitId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitIds VALUES (@BranchUnitId)
		END

		IF(@BranchUnitDepartmentId = -1)
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds
			SELECT DISTINCT BranchUnitDepartmentId FROM UserPermissionForDepartmentLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfBranchUnitDepartmentIds VALUES (@BranchUnitDepartmentId)
		END

		IF(@EmployeeTypeID = -1)
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds
			SELECT DISTINCT EmployeeTypeId FROM UserPermissionForEmployeeLevel
			WHERE UserName = @UserName;
		END  
		ELSE
		BEGIN
			INSERT INTO @ListOfEmployeeTypeIds VALUES (@EmployeeTypeID)
		END

		IF (@DepartmentSectionId  = -1)
		BEGIN
			SET @DepartmentSectionId = NULL
		END

		IF (@DepartmentLineId  = -1)
		BEGIN
			SET @DepartmentLineId = NULL
		END

		
		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
		END

		DECLARE @ReturnTable TABLE (
								RowID BIGINT,
								TotalRows INT,
								DepartmentName NVARCHAR(100) NULL,
								SectionName NVARCHAR(100) NULL,
								LineName NVARCHAR(100) NULL,
								EmployeeType NVARCHAR(100) NULL,
								FromDate NVARCHAR(100) NULL,
								ToDate NVARCHAR(100) NULL,
								TransactionDate NVARCHAR(100) NULL,
								EmployeeId UNIQUEIDENTIFIER NULL,
								EmployeeCardId NVARCHAR(100) NULL,
								EmployeeName NVARCHAR(100) NULL,
								EmployeeDesignation NVARCHAR(100) NULL,
								MobileNo NVARCHAR(100) NULL,
								JoiningDate NVARCHAR(100) NULL,
								WorkShiftName NVARCHAR(100) NULL,
								InTime NVARCHAR(100) NULL,
								OutTime NVARCHAR(100) NULL,
								LateInMinutes NVARCHAR(100) NULL,
								[Status] NVARCHAR(100) NULL,
								OTHours DECIMAL(18,2) NULL,
								ExtraOTHours DECIMAL(18,2) NULL,
								WeekendOTHours DECIMAL(18,2) NULL,
								Remarks NVARCHAR(MAX) NULL
								);

		 INSERT INTO @ReturnTable
		 SELECT DISTINCT
				 ROW_NUMBER() OVER (ORDER BY EmployeeCardId ASC) AS RowNumber	
				,0			
				,DepartmentName
				,SectionName
				,LineName
				,EmployeeType
				,CONVERT(VARCHAR(10), @FromDate, 103) AS FromDate
				,CONVERT(VARCHAR(10), @Todate, 103) AS ToDate
				,CONVERT(VARCHAR(10), TransactionDate, 103) AS TransactionDate
				,EmployeeId
				,EmployeeCardId
				,EmployeeName 
				,EmployeeDesignation 
				,MobileNo
				,CONVERT(VARCHAR(10), JoiningDate, 103) AS JoiningDate
				,WorkShiftName 
				,CONVERT(VARCHAR(10), InTime, 108)  AS InTime
				,CONVERT(VARCHAR(10), OutTime, 108)  AS OutTime	
				,CONVERT(VARCHAR(10),LateInMinutes,108) AS LateInMinutes	
				,CASE [Status] WHEN 'Late' THEN 'Present' ELSE [Status] END AS [Status]
				,eio.OTHours
				,eio.ExtraOTHours
				,eio.WeekendOTHours
				,eio.Remarks
				FROM EmployeeInOut eio
				WHERE 
				eio.IsActive = 1
				AND eio.CompanyId IN (SELECT CompanyIDs FROM @ListofCompanyIds)
				AND eio.BranchId IN (SELECT BranchIDs FROM @ListOfBranchIds)
				AND eio.BranchUnitId IN (SELECT BranchUnitIDs FROM @ListOfBranchUnitIds)
				AND eio.BranchUnitDepartmentId IN  (SELECT BranchUnitDepartmentIDs FROM @ListOfBranchUnitDepartmentIds)
				AND (eio.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
				AND (eio.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
				AND eio.EmployeeTypeId IN (SELECT EmployeeTypeIDs FROM @ListOfEmployeeTypeIds)
				AND ((eio.EmployeeCardId = @EmployeeCardId) OR (@EmployeeCardId IS NULL))
				AND CAST(eio.TransactionDate AS DATE) >= CAST(@FromDate AS DATE)
				AND CAST(eio.TransactionDate AS DATE) <= CAST(@ToDate AS DATE)	
				ORDER BY DepartmentName, EmployeeCardId, TransactionDate ASC

				DECLARE @TotalRecords INT = NULL;

				SELECT @TotalRecords = COUNT(*) FROM @ReturnTable

				UPDATE @ReturnTable
				SET TotalRows = @TotalRecords;			

				SELECT * FROM @ReturnTable
				WHERE RowID BETWEEN (@StartRowIndex * @MaxRows) + 1 AND ((@StartRowIndex+1) * @MaxRows)

		COMMIT TRAN


END







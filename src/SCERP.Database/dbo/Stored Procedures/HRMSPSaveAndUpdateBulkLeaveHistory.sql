
-- ======================================================================================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- ======================================================================================================

-- EXEC HRMSPSaveAndUpdateBulkLeaveHistory 2016

CREATE PROCEDURE [dbo].[HRMSPSaveAndUpdateBulkLeaveHistory]
	@Year INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

	DECLARE  @imax INT, @i INT  ---@year INT;		
	DECLARE  @EmployeeLeaveInfo  TABLE(
									 RowID	INT    IDENTITY ( 1 , 1 ),
									 EmployeeId UNIQUEIDENTIFIER,
									 EmployeeCardId NVARCHAR(100),
									 BranchUnitId INT,
									 EmployeeTypeId INT)

	INSERT INTO @EmployeeLeaveInfo
	SELECT DISTINCT el.EmployeeId, el.EmployeeCardId, branchUnit.BranchUnitId, employeeType.Id  
	FROM [dbo]. [EmployeeLeave]	el
	INNER JOIN 
	Employee employee ON el.EmployeeId = employee.EmployeeId
	LEFT JOIN
	(SELECT EmployeeId, FromDate, BranchUnitDepartmentId,DesignationId,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
		FROM EmployeeCompanyInfo AS employeeCompanyInfo
		WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= CURRENT_TIMESTAMP)
		AND employeeCompanyInfo.IsActive = 1)) employeeCompanyInfo 
	ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
	LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
	LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
	LEFT JOIN EmployeeDesignation employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
	LEFT JOIN EmployeeType employeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1
	
	
	SELECT @imax = COUNT(EmployeeId) FROM @EmployeeLeaveInfo
	SET @i = 1
			
			
	WHILE (@i <= @imax)
	BEGIN
		   DECLARE @EmployeeId UNIQUEIDENTIFIER,
				   @EmployeeCardId NVARCHAR(100),
				   @BranchUnitId INT,
				   @EmployeeTypeId INT;

		   SELECT @EmployeeId = EmployeeId,
		          @EmployeeCardId = EmployeeCardId,
				  @BranchUnitId =  BranchUnitId,
				  @EmployeeTypeId = EmployeeTypeId
				  FROM @EmployeeLeaveInfo
				  WHERE RowID = @i;

		   DECLARE @EmployeeLeaveHistoryId INT;
		   SELECT @EmployeeLeaveHistoryId = elh.EmployeeLeaveHistoryId
		   FROM EmployeeLeaveHistory elh
		   WHERE elh.EmployeeId = @EmployeeId
		   AND elh.[Year] = @Year
		   AND elh.IsActive = 1
	
		   IF(@EmployeeLeaveHistoryId IS NULL)
	       BEGIN
		        INSERT INTO EmployeeLeaveHistory 
					(EmployeeId,
					 EmployeeCardId,
					 [Year],
					 LeaveTypeId,
					 NoOfAllowableLeaveDays,
					 NoOfConsumedLeaveDays,
					 NoOfRemainingLeaveDays,
					 CreatedDate,
					 IsActive)
				SELECT @EmployeeId,
					   @EmployeeCardId,
					   @Year,
					   LeaveTypeId,
					   NoOfDays,
					   0,
					   0,
					   CURRENT_TIMESTAMP,
					   1
					   FROM LeaveSetting
					   WHERE EmployeeTypeId = @EmployeeTypeId
					   AND BranchUnitId = @BranchUnitId
					   AND IsActive = 1	
			END

			UPDATE EmployeeLeaveHistory
			SET NoOfConsumedLeaveDays = 
				(SELECT ISNULL(COUNT(ConsumedDate),0)
					FROM EmployeeLeaveDetail
					WHERE EmployeeLeaveHistory.EmployeeId = EmployeeLeaveDetail.EmployeeId
					AND EmployeeLeaveHistory.LeaveTypeId = EmployeeLeaveDetail.LeaveTypeId
					AND Year(ConsumedDate) = @Year
					AND EmployeeLeaveDetail.IsActive = 1
					AND EmployeeLeaveHistory.IsActive = 1
					GROUP BY EmployeeLeaveDetail.LeaveTypeId
				)
			WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
			AND EmployeeLeaveHistory.[Year] = @Year
			AND EmployeeLeaveHistory.IsActive = 1

			UPDATE EmployeeLeaveHistory
			SET NoOfRemainingLeaveDays = (ISNULL(NoOfAllowableLeaveDays,0) - ISNULL(NoOfConsumedLeaveDays,0))
			WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
			AND EmployeeLeaveHistory.[Year] = @Year	
			AND EmployeeLeaveHistory.IsActive = 1
 
		   SET @i = @i + 1 
	END	

	DELETE FROM @EmployeeLeaveInfo;

	COMMIT TRAN

END



-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- =============================================

-- EXEC HRMSPGetEmployeeAvailableLeaveOfSpecificLeaveType 'D8E6017A-B474-43E8-B4D7-94E16D71BFC6', '7051', 2020,1, 1, 5

CREATE PROCEDURE [dbo].[HRMSPGetEmployeeAvailableLeaveOfSpecificLeaveType]
	@EmployeeId UNIQUEIDENTIFIER,
	@EmployeeCardId NVARCHAR(100),
	@Year INT,
	@BranchUnitId INT,
	@EmployeeTypeId INT,
	@LeaveTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

	DECLARE @EmployeeLeaveHistoryId INT;

	SELECT @EmployeeLeaveHistoryId = elh.EmployeeLeaveHistoryId
	FROM EmployeeLeaveHistory elh
	WHERE elh.EmployeeId = @EmployeeId
	AND elh.[Year] = @Year
	AND elh.LeaveTypeId = @LeaveTypeId
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
			   ISNULL(NoOfDays, 0),
			   0,
			   0,
			   CURRENT_TIMESTAMP,
			   1
			   FROM LeaveSetting
			   WHERE EmployeeTypeId = @EmployeeTypeId
			   AND BranchUnitId = @BranchUnitId
			   AND LeaveTypeId = @LeaveTypeId
			   AND IsActive = 1

	
		UPDATE EmployeeLeaveHistory
		SET NoOfConsumedLeaveDays = 
		(SELECT ISNULL(COUNT(ConsumedDate),0)
		 FROM EmployeeLeaveDetail
		 WHERE EmployeeLeaveHistory.EmployeeId = EmployeeLeaveDetail.EmployeeId
		 AND EmployeeLeaveHistory.LeaveTypeId = EmployeeLeaveDetail.LeaveTypeId
		 AND EmployeeLeaveHistory.LeaveTypeId = @LeaveTypeId
		 AND Year(EmployeeLeaveDetail.ConsumedDate) = @Year
		 AND EmployeeLeaveDetail.IsActive = 1
		 AND EmployeeLeaveHistory.IsActive = 1
		 GROUP BY EmployeeLeaveDetail.LeaveTypeId
		)
		WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
		AND EmployeeLeaveHistory.[Year] = @Year
		AND EmployeeLeaveHistory.LeaveTypeId = @LeaveTypeId
		AND EmployeeLeaveHistory.IsActive = 1

		UPDATE EmployeeLeaveHistory
		SET NoOfRemainingLeaveDays = (ISNULL(NoOfAllowableLeaveDays,0) - ISNULL(NoOfConsumedLeaveDays,0))
		WHERE EmployeeLeaveHistory.EmployeeId = @EmployeeId
		AND EmployeeLeaveHistory.[Year] = @Year
		AND EmployeeLeaveHistory.LeaveTypeId = @LeaveTypeId
		AND EmployeeLeaveHistory.IsActive = 1
					   		   
	END

	SELECT  lt.Title AS Title,
			--ISNULL(elh.NoOfRemainingLeaveDays,0) AS Available
			ISNULL(NoOfAllowableLeaveDays, 0) - ISNULL(NoOfConsumedLeaveDays, 0)
			FROM EmployeeLeaveHistory elh
			INNER JOIN LeaveType lt 
			ON elh.LeaveTypeID = lt.Id
			WHERE elh.EmployeeId = @EmployeeId
			AND elh.[Year] = @Year
			AND elh.LeaveTypeId = @LeaveTypeId
			AND elh.IsActive = 1
			AND lt.IsActive = 1
	COMMIT TRAN

END



-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2015.12.15
-- =============================================

-- EXEC HRMSPSaveIndividualLeaveHistoryForSpecificYear '13ed233a-e722-40d7-850e-c9e003bb2a83', '0760', 2015, 1, 4

CREATE PROCEDURE [dbo].[HRMSPSaveIndividualLeaveHistoryForSpecificYear]
	@EmployeeId UNIQUEIDENTIFIER,
	@EmployeeCardId NVARCHAR(100),
	@Year INT,
	@BranchUnitId INT,
	@EmployeeTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

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
				NoOfDays,
				CURRENT_TIMESTAMP,
				1
				FROM LeaveSetting
				WHERE EmployeeTypeId = @EmployeeTypeId
				AND BranchUnitId = @BranchUnitId
				AND IsActive = 1		
	
		IF(@@ERROR <> 0)
			SELECT 0;
		ELSE
			SELECT 1;	   		   
	END
	ELSE
	BEGIN
		SELECT 0;
	END

	COMMIT TRAN

END


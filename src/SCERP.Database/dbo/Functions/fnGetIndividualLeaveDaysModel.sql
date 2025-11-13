

CREATE FUNCTION [dbo].[fnGetIndividualLeaveDaysModel] (  

	@EmployeeId uniqueidentifier,
	@FromDate DateTime,
	@ToDate DateTime,
	@LeaveTypeId INT
)

RETURNS INT
AS BEGIN
   
        DECLARE @JoiningDate DATETIME;
		SELECT @JoiningDate = JoiningDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1
	
		IF(CAST(@JoiningDate AS DATE) > CAST(@FromDate AS DATE))
					SELECT @FromDate = CAST(@JoiningDate AS DATE) 
	
		DECLARE @QuitDate DATETIME;
		SELECT @QuitDate = QuitDate FROM Employee WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND [Status] = 2
	
		IF(@QuitDate IS NOT NULL)
		BEGIN
			IF((CAST(@QuitDate AS DATE) >= CAST(@FromDate AS DATE)) AND (CAST(@QuitDate AS DATE) < CAST(@ToDate AS DATE)))
					SELECT @ToDate = CAST(@QuitDate AS DATE) 
		END

		IF(@ToDate > CAST(CURRENT_TIMESTAMP AS DATE))
			SELECT @ToDate = CAST(CURRENT_TIMESTAMP AS DATE)

		DECLARE 
		@result int = 0
		
		SELECT @result = COUNT(EmployeeLeaveDetail.Id) FROM EmployeeLeaveDetail
		WHERE CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) >= CAST(@FromDate AS DATE)
		AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) <= CAST(@ToDate AS DATE) 
		AND EmployeeLeaveDetail.EmployeeId = @EmployeeId
		AND EmployeeLeaveDetail.LeaveTypeId = @LeaveTypeId
		AND EmployeeLeaveDetail.IsActive = 1 
		
		RETURN @result;
END






-- SELECT [dbo].[fnGetEmployeeLateTime]('13A58B03-E1F2-4386-BEEF-81FC1EE2BC61','2015-11-30','125630')

CREATE FUNCTION [dbo].[fnGetEmployeeLateTime] 
(  
	@EmployeeId uniqueidentifier,
	@Date Date,
	@EmployeeWorkShiftId INT,
	@JoiningDate DATETIME,
	@QuitDate DATETIME = NULL
)

RETURNS INT

AS BEGIN
	
	IF(@EmployeeWorkShiftId IS NULL)
			RETURN NULL;
		
	IF(CAST(@JoiningDate AS DATE) > @Date)
	BEGIN
		RETURN  0;
	END

	IF(@QuitDate IS NOT NULL AND (CAST(@QuitDate AS DATE) < @Date))
	BEGIN
		RETURN  0;
	END

	IF(@Date > CAST(CURRENT_TIMESTAMP AS DATE))
	BEGIN
		RETURN  0;
	END		

	IF EXISTS (SELECT 1 FROM EmployeeDailyAttendance 
			  WHERE CAST(EmployeeDailyAttendance.TransactionDateTime AS date) = @Date
			  AND EmployeeDailyAttendance.EmployeeId = @EmployeeId 
			  AND EmployeeDailyAttendance.IsActive=1)
	BEGIN		
	
		DECLARE @InTime TIME;
		SELECT @InTime = InTime FROM EmployeeInOut
		WHERE EmployeeId = @EmployeeId
		AND TransactionDate = @Date
		AND EmployeeWorkShiftId = @EmployeeWorkShiftId
		AND IsActive = 1

		DECLARE @DelayTime INT;
		SELECT @DelayTime =
			CASE
				WHEN  DATEDIFF(minute,  DATEADD(minute, ws.InBufferTime , ws.StartTime), @InTime) <= 0 
				THEN 0
				ELSE  DATEDIFF(minute,  DATEADD(minute, ws.InBufferTime , ws.StartTime), @InTime)
			END		
			FROM WorkShift AS ws							
			INNER JOIN BranchUnitWorkShift AS buws ON buws.WorkShiftId = ws.WorkShiftId
			INNER JOIN EmployeeWorkShift AS ews ON ews.BranchUnitWorkShiftId = buws.BranchUnitWorkShiftId			
			INNER JOIN Employee AS emp ON emp.EmployeeId = ews.EmployeeId
			WHERE ((CAST(ews.ShiftDate AS DATE) =  CAST(@Date AS DATE)) 
				AND (ews.EmployeeId = @EmployeeId)
				AND ews.EmployeeWorkShiftId = @EmployeeWorkShiftId
				AND ws.IsActive = 1	
				AND buws.IsActive = 1
				AND buws.[Status] = 1
				AND ews.IsActive = 1
				AND ews.[Status] = 1
				AND emp.IsActive = 1) 
	END

    RETURN @DelayTime
END





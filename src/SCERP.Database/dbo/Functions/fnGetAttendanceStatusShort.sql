
--- Select [dbo].[fnGetAttendanceStatusShort] ('D68B04EC-C478-413B-95A4-D3DCBB92F455','2016-02-16','185554')

CREATE FUNCTION [dbo].[fnGetAttendanceStatusShort] 
(  
		@EmployeeId uniqueidentifier,
		@Date Date,
		@EmployeeWorkShiftId INT
)

RETURNS VARCHAR(100)

AS BEGIN

    DECLARE @Result VARCHAR(250) = 'Absent'
		
				
	IF EXISTS(SELECT 1 FROM EmployeeLeaveDetail 
			  WHERE EmployeeLeaveDetail.EmployeeId = @EmployeeId 
			  AND EmployeeLeaveDetail.IsActive = 1 
			  AND CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) = CAST(@Date AS DATE))
	RETURN 'Leave'


	IF EXISTS(SELECT 1 FROM OutStationDuty WHERE OutStationDuty.EmployeeId = @EmployeeId 
			  AND CAST(OutStationDuty.DutyDate AS date) = CAST(@Date AS date) 
			  AND OutStationDuty.IsActive =1)
	RETURN 'OSD'


	IF NOT EXISTS(SELECT 1 FROM EmployeeDailyAttendance2 WHERE EmployeeId = @EmployeeId AND CAST(TransactionDateTime AS DATE) BETWEEN CAST(DATEADD(DAY, -1, @Date) AS DATE) AND @Date AND IsActive = 1 )
	BEGIN
		RETURN 'Absent'
	END

	ELSE

	BEGIN
		DECLARE @InTime TIME
		DECLARE @BranchUnitWorkShiftId INT
		DECLARE @OfficialInTime TIME

		SELECT @BranchUnitWorkShiftId = BranchUnitWorkShiftId FROM EmployeeWorkShift WHERE EmployeeWorkShiftId = @EmployeeWorkShiftId AND IsActive = 1

		IF(@BranchUnitWorkShiftId = 46)
			SET @OfficialInTime = '09:00:00.000'

		IF(@BranchUnitWorkShiftId = 24)
			SET @OfficialInTime = '07:00:00.000'

		IF(@BranchUnitWorkShiftId = 25)
			SET @OfficialInTime = '08:00:00.000'

		IF(@BranchUnitWorkShiftId = 57)
			SET @OfficialInTime = '08:30:00.000'

									-- Knitting Workshift
		IF(@BranchUnitWorkShiftId = 36)
			SET @OfficialInTime = '21:00:00.000'
		
		IF(@BranchUnitWorkShiftId = 43)
			SET @OfficialInTime = '09:00:00.000'

									-- Dyeing Workshift
		IF(@BranchUnitWorkShiftId = 11)
			SET @OfficialInTime = '21:00:00.000'

		IF(@BranchUnitWorkShiftId = 47)
			SET @OfficialInTime = '09:00:00.000'

		IF(@BranchUnitWorkShiftId = 48)
			SET @OfficialInTime = '08:00:00.000'

		IF(@BranchUnitWorkShiftId = 49)
			SET @OfficialInTime = '20:00:00.000'

		IF(@BranchUnitWorkShiftId = 50)
			SET @OfficialInTime = '14:00:00.000'


		IF(@BranchUnitWorkShiftId IN(24,25,46,43,47,48,57))
		BEGIN
			SELECT TOP(1) @InTime = CAST(TransactionDateTime AS TIME) 
			FROM EmployeeDailyAttendance2 
			WHERE EmployeeId = @EmployeeId 
			AND CAST(TransactionDateTime AS DATE) =  @Date
			AND CAST(TransactionDateTime AS TIME) BETWEEN '06:00:00.000' AND '10:00:00.000'
			AND IsActive = 1
			ORDER BY TransactionDateTime
		END

		IF(@BranchUnitWorkShiftId IN(11,36,49))
		BEGIN
			SELECT TOP(1) @InTime = CAST(TransactionDateTime AS TIME) 
			FROM EmployeeDailyAttendance2 
			WHERE EmployeeId = @EmployeeId 
			AND CAST(TransactionDateTime AS DATE) =  DATEADD(DAY, -1, @Date)
			AND CAST(TransactionDateTime AS TIME) BETWEEN '19:00:00.000' AND '23:00:00.000'
			AND IsActive = 1
			ORDER BY TransactionDateTime
		END

		IF(@BranchUnitWorkShiftId IN(50))
		BEGIN
			SELECT TOP(1) @InTime = CAST(TransactionDateTime AS TIME) 
			FROM EmployeeDailyAttendance2 
			WHERE EmployeeId = @EmployeeId 
			AND CAST(TransactionDateTime AS DATE) =  DATEADD(DAY, -1, @Date)
			AND CAST(TransactionDateTime AS TIME) BETWEEN '14:00:00.000' AND '16:00:00.000'
			AND IsActive = 1
			ORDER BY TransactionDateTime
		END


		IF( @InTime BETWEEN DATEADD(MINUTE, -120, @OfficialInTime) AND DATEADD(MINUTE, 6, @OfficialInTime))
			RETURN 'Present'

		ELSE IF(@InTime > DATEADD(MINUTE, 6, @OfficialInTime))
			RETURN 'Late'
			
	  END

    RETURN @Result
END
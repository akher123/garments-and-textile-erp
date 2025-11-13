
-- =================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-01-11>
-- Description:	<> EXEC [HRMSPAssignEmployeeLine] '2016-01-01'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[HRMSPAssignEmployeeLine]
				@EmployeeList AS dbo.EmployeeList READONLY,
				@TargetDepartmentLineId INT,
				@EffectiveFromDate DATETIME = NULL,
				@UserName NVARCHAR(100)

AS
BEGIN
	
		    SET NOCOUNT ON;
			BEGIN TRAN
			
			DECLARE @UserID UNIQUEIDENTIFIER;
			SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;
	
			DECLARE  @EmployeeDepartmentInfo  TABLE(
										 RowID       INT    IDENTITY (1,1),
										 EmployeeId UNIQUEIDENTIFIER
										)

			INSERT INTO @EmployeeDepartmentInfo
			SELECT EmployeeId
			FROM @EmployeeList		
			
			
			DECLARE  @imax INT, 
					 @i INT, 
					 @EmployeeIdTemp UNIQUEIDENTIFIER, 
					

					 @EmployeeCompanyInfoId INT,
					 @EmployeeId UNIQUEIDENTIFIER,
					 @BranchUnitDepartmentId INT,
					 @DesignationId INT,
					 @DepartmentSectionId INT,
					 @DepartmentLineId INT,
					 @PunchCardNo NVARCHAR(100),
					 @IsEligibleForOvertime BIT,
					 @FromDate DATETIME,
					 @ToDate DATETIME,
					 @JobTypeId INT

			SELECT @imax = COUNT(EmployeeId) FROM @EmployeeDepartmentInfo
			SET @i = 1
			
			WHILE (@i <= @imax)
			BEGIN			
				SELECT @EmployeeIdTemp = EmployeeId				  
				FROM   @EmployeeDepartmentInfo
				WHERE  RowID = @i


				SELECT TOP(1) @EmployeeCompanyInfoId = EmployeeCompanyInfoId,
							  @EmployeeId = EmployeeId,
							  @BranchUnitDepartmentId = BranchUnitDepartmentId,
							  @DesignationId = DesignationId,
							  @DepartmentSectionId = DepartmentSectionId,
							  @DepartmentLineId = DepartmentLineId,
							  @PunchCardNo = PunchCardNo,
							  @IsEligibleForOvertime = IsEligibleForOvertime,	
							  @JobTypeId = JobTypeId				   
				FROM  EmployeeCompanyInfo
				WHERE EmployeeId = @EmployeeIdTemp
				AND IsActive = 1
				ORDER BY FromDate DESC

				UPDATE EmployeeCompanyInfo
				SET ToDate =  DATEADD(DAY, -1, @EffectiveFromDate)
				WHERE EmployeeCompanyInfoId = @EmployeeCompanyInfoId

				INSERT INTO EmployeeCompanyInfo
							( EmployeeId,
							  BranchUnitDepartmentId,
							  DesignationId,
							  DepartmentSectionId,
							  DepartmentLineId,
							  PunchCardNo,
							  IsEligibleForOvertime,
							  JobTypeId,
							  FromDate,
							  CreatedDate,
							  CreatedBy,
							  IsActive)
				VALUES(@EmployeeId,
					   @BranchUnitDepartmentId,
					   @DesignationId,
					   @DepartmentSectionId,
					   @TargetDepartmentLineId, --- New Line
					   @PunchCardNo,
					   @IsEligibleForOvertime,
					   @JobTypeId,
					   @EffectiveFromDate, --- New Date
					   CURRENT_TIMESTAMP,
					   @UserID,
					   1)


				SET @i = @i + 1;
			
			END	
						 					
			COMMIT TRAN	
			
			
		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;		 	
						 	 
END



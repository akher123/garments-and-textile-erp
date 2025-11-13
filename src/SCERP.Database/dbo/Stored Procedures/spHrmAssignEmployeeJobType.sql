
-- =================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2016-05-30>
-- Description:	<> EXEC [spHrmAssignEmployeeJobType] 
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[spHrmAssignEmployeeJobType]
				@EmployeeList AS dbo.EmployeeList READONLY,
				@TargetJobTypeId INT,
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
					 @JobTypeId INT,
					 @PunchCardNo NVARCHAR(100),
					 @IsEligibleForOvertime BIT,
					 @FromDate DATETIME,
					 @ToDate DATETIME

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
							  @JobTypeId = JobTypeId,
							  @PunchCardNo = PunchCardNo,
							  @IsEligibleForOvertime = IsEligibleForOvertime				   
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
							  JobTypeId,
							  PunchCardNo,
							  IsEligibleForOvertime,
							  FromDate,
							  CreatedDate,
							  CreatedBy,
							  IsActive)
				VALUES(@EmployeeId,
					   @BranchUnitDepartmentId,
					   @DesignationId,
					   @DepartmentSectionId,
					   @DepartmentLineId, 
					   @TargetJobTypeId, --- New Job Type
					   @PunchCardNo,
					   @IsEligibleForOvertime,
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



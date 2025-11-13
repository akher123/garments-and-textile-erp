
-- =========================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-12>
-- Description:	<> EXEC [SPProcessEmployeeContinuousAbsentDays] 1, 1, 2, NULL, NULL, NULL, '2016-03-16','2016-03-16',2, '','superadmin'
-- =========================================================================================================================================

CREATE PROCEDURE [dbo].[SPProcessEmployeeContinuousAbsentDays]
	   @CompanyId INT = NULL,
	   @BranchId INT = NULL,
	   @BranchUnitId INT = NULL,
	   @BranchUnitDepartmentId INT = NULL, 
	   @DepartmentSectionId INT = NULL, 
	   @DepartmentLineId INT = NULL, 
	   @FromDate DATETIME = NULL,
	   @ToDate DATETIME = NULL,
	   @EmployeeTypeId INT = NULL,
	   @EmployeeCardId NVARCHAR(100) = '',
	   @UserName NVARCHAR(100)
AS
BEGIN
	
		
		SET XACT_ABORT ON;
		SET NOCOUNT ON;
		 	
		BEGIN TRAN

		
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

		IF (@EmployeeCardId  = '')
		BEGIN
			SET @EmployeeCardId = NULL
		END

		
		DECLARE @UserID UNIQUEIDENTIFIER;
		SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;

		DECLARE	@Days INT;

		
		
	
		SET FMTONLY OFF;

		DECLARE  @EmployeeInOutInfo  TABLE(
									 RowID	INT    IDENTITY ( 1 , 1 )							
									,[EmployeeId] UNIQUEIDENTIFIER					
									,[EmployeeWorkShiftId] INT
									,[TransactionDate] DATETIME
									);

		
		INSERT INTO @EmployeeInOutInfo
					(
					 [EmployeeId]
					,[EmployeeWorkShiftId]
					,[TransactionDate]
					)
		SELECT		     	 
			  employeeInOut.EmployeeId	
			 ,employeeInOut.EmployeeWorkShiftId
			 ,employeeInOut.TransactionDate		
		FROM					
		EmployeeInOut AS  employeeInOut					
		WHERE (employeeInOut.IsActive = 1
		AND (employeeInOut.CompanyId =  @CompanyId OR @CompanyId IS NULL)
		AND (employeeInOut.BranchId = @BranchId OR @BranchId IS NULL)
		AND (employeeInOut.BranchUnitId = @BranchUnitId OR @BranchUnitId IS NULL)
		AND (employeeInOut.BranchUnitDepartmentId = @BranchUnitDepartmentId OR @BranchUnitDepartmentId IS NULL)
		AND (employeeInOut.DepartmentSectionId = @DepartmentSectionId OR @DepartmentSectionId IS NULL)
		AND (employeeInOut.DepartmentLineId = @DepartmentLineId OR @DepartmentLineId IS NULL)			 							
		AND (employeeInOut.EmployeeTypeId = @EmployeeTypeId OR @EmployeeTypeId IS NULL)
		AND (employeeInOut.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)
		AND (((employeeInOut.TransactionDate) >= CAST(@FromDate AS DATE)) OR (@FromDate IS NULL))
		AND (((employeeInOut.TransactionDate) <= CAST(@ToDate AS DATE)) OR (@ToDate IS NULL)))
		
		ORDER BY TransactionDate, EmployeeCardId ASC	

		DECLARE  @imax INT, @i INT;	
		SELECT @imax = COUNT(EmployeeId) FROM @EmployeeInOutInfo

		SET @i = 1
		
		WHILE (@i <= @imax)
		BEGIN	
			 DECLARE
					 @EmployeeIdTemp UNIQUEIDENTIFIER
					,@EmployeeWorkShiftIdTemp INT
					,@TransactionDateTemp DATETIME
						
			SELECT   @EmployeeIdTemp = EmployeeId
					,@EmployeeWorkShiftIdTemp = EmployeeWorkShiftId				
					,@TransactionDateTemp = TransactionDate
			FROM @EmployeeInOutInfo
			WHERE RowID = @i;

			IF(@EmployeeWorkShiftIdTemp IS NOT NULL)
			BEGIN
				DECLARE @EmployeeInOutId INT
				SELECT @EmployeeInOutId =  [Id] FROM EmployeeInOut
				WHERE EmployeeId = @EmployeeIdTemp
				AND TransactionDate = @TransactionDateTemp
				AND EmployeeWorkShiftId = @EmployeeWorkShiftIdTemp 

				UPDATE [EmployeeInOut]
				SET  [TotalContinuousAbsentDays] = dbo.fnGetTotalContinuousAbsentDays(@EmployeeIdTemp, @TransactionDateTemp, 0, @EmployeeWorkShiftIdTemp)	
				WHERE [Id] = @EmployeeInOutId

			END

			SET @i = @i + 1	
		END

		COMMIT TRAN

		DECLARE @Result INT = 1;

		IF (@@ERROR <> 0)
			SET @Result = 0;
		
		SELECT @Result;
		
END







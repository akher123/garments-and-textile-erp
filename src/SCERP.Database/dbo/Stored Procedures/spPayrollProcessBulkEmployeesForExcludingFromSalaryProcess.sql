
-- =====================================================================================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <2015-10-04>
-- Description:	<> EXEC [spPayrollProcessBulkEmployeesForExcludingFromSalaryProcess] -1, -1, -1, -1, -1, -1, -1,'', 2015, 10, '2015-10-01', '2015-10-31', 'superadmin'
-- =====================================================================================================================================================================

CREATE PROCEDURE [dbo].[spPayrollProcessBulkEmployeesForExcludingFromSalaryProcess]
	   @EmployeeList AS dbo.EmployeeList READONLY,
	   @Year INT,
	   @Month INT,
	   @FromDate DATETIME,
	   @ToDate DATETIME,	
	   @Remarks NVARCHAR(MAX) = '', 
	   @UserName NVARCHAR(100)
AS
BEGIN
	
			SET XACT_ABORT ON;
			SET NOCOUNT ON;

			BEGIN TRAN

			DECLARE @UserID UNIQUEIDENTIFIER;
			SELECT @UserID = EmployeeID FROM [User] WHERE UserName = @UserName;	
			
			DECLARE  @imax INT, @i INT;		
			DECLARE  @EmployeeInfo  TABLE(
									 RowID       INT    IDENTITY ( 1 , 1 ),
									 EmployeeId UNIQUEIDENTIFIER
									)	

			INSERT INTO @EmployeeInfo
			SELECT EmployeeId FROM @EmployeeList				
		
			DECLARE  @EmployeeId UNIQUEIDENTIFIER;
			DECLARE  @EmployeeCardId NVARCHAR(100);

			SELECT @imax = COUNT(EmployeeId) FROM @EmployeeInfo
			SET @i = 1

			WHILE (@i <= @imax)
			BEGIN
				SELECT @EmployeeId = EmployeeId
				FROM   @EmployeeInfo
				WHERE  RowID = @i

				SELECT @EmployeeCardId = EmployeeCardId FROM Employee
				WHERE EmployeeId = @EmployeeId
				
				DELETE FROM [dbo].[PayrollExcludedEmployeeFromSalaryProcess]
				WHERE           
				(
				(EmployeeId = @EmployeeId OR @EmployeeId IS NULL)
				AND (EmployeeCardId = @EmployeeCardId OR @EmployeeCardId IS NULL)		
				AND ([Year] = @Year)
				AND ([Month] = @Month)
				AND ([FromDate] = @FromDate)
				AND ([ToDate] = @ToDate)
				AND (IsActive=1)
				)

				BEGIN
					INSERT INTO [dbo].[PayrollExcludedEmployeeFromSalaryProcess]
							   ([EmployeeId]
							   ,[EmployeeCardId]
							   ,[Year]
							   ,[Month]
							   ,[FromDate]
							   ,[ToDate]
							   ,[Remarks]
							   ,[CreatedDate]
							   ,[CreatedBy]
							   ,[EditedDate]
							   ,[EditedBy]
							   ,[IsActive])
				    VALUES(
							@EmployeeId,
							@EmployeeCardId,
							@Year,
							@Month,
							@FromDate,
							@ToDate,
							@Remarks,
							CURRENT_TIMESTAMP,
							@UserID,
							NULL,
							NULL,
							1
					      )					
				END

				SET @i = @i + 1 
			END

			DELETE FROM @EmployeeInfo;

			COMMIT TRAN

			IF (@@ERROR <> 0)
				SELECT 0;
			ELSE
				SELECT 1;
END







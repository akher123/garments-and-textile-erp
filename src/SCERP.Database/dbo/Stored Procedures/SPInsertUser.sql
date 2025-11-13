
---- EXEC [dbo].[SPInsertUser] 'kamrul','','43CB0E34-9226-451C-A9B1-25C78DA35477','kamrul9119','2015-12-06','5146FD70-8CEE-4022-A606-7CFAFEB7874C',1

CREATE PROCEDURE [dbo].[SPInsertUser]
    @UserName NVARCHAR(100) , 
	@EmailAddress VARCHAR(100),
    @EmployeeId UNIQUEIDENTIFIER,
    @Password NVARCHAR(50),
    @CDT DATETIME=NULL,
	@CreatedBy UNIQUEIDENTIFIER,
	@IsActive BIT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @salt UNIQUEIDENTIFIER = NEWID(),@responseCode INT = 0
    BEGIN TRY

        INSERT INTO [User] (UserName,EmailAddress,EmployeeId,PasswordHash,Salt,CDT,CreatedBy,IsActive)
        VALUES(@UserName,@EmailAddress,@EmployeeId, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt,@CDT,@CreatedBy,@IsActive)
		
        SET @responseCode=1;

    END TRY
    BEGIN CATCH
        SET @responseCode=0;
    END CATCH
	SELECT @responseCode;
END

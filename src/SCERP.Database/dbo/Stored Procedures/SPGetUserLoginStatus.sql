
CREATE PROCEDURE [dbo].[SPGetUserLoginStatus]
    @LoginName NVARCHAR(254),
    @Password NVARCHAR(50)
AS
BEGIN

    SET NOCOUNT ON

    DECLARE @id INT, @responseCode INT = 0

    IF EXISTS (SELECT TOP 1 Id FROM [dbo].[User] WHERE UserName = @LoginName)
    BEGIN
		
        SET @id = (SELECT Id FROM [dbo].[User] WHERE UserName= @LoginName AND PasswordHash = HASHBYTES('SHA2_512', @Password + CAST(Salt AS NVARCHAR(36))))

       IF(@id IS NOT NULL)
           SET @responseCode = 1;
		   ELSE 
           SET @responseCode=0;
    END


	SELECT @responseCode;

END

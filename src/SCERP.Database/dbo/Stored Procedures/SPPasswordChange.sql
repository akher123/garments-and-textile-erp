
CREATE PROCEDURE [dbo].[SPPasswordChange] 
(
    @Id INT,
    @Password NVARCHAR(50)
)
AS
BEGIN

   SET NOCOUNT ON

   DECLARE @salt UNIQUEIDENTIFIER = NEWID(),@responseCode INT = 0

   BEGIN TRY
        Update [User] Set PasswordHash = HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))),Salt=@salt
        Where Id=@Id

        SET @responseCode=1;

    END TRY
    BEGIN CATCH
        SET @responseCode=0;
    END CATCH

	SELECT @responseCode;

END
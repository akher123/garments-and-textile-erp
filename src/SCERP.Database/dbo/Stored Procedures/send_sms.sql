CREATE procedure [dbo].[send_sms](
@receiver varchar(50),
@msg varchar(200)
)
as

begin

DECLARE @status int
DECLARE @responseText as table(responseText nvarchar(max))
DECLARE @res as Int;
DECLARE @url as nvarchar(1000) = 'http://66.45.237.70/maskingapi.php?username=SoftCode&password=GTexERP&number=' + @receiver + '&senderid=GTex%20ERP&message=' + @msg
EXEC sp_OACreate 'MSXML2.ServerXMLHTTP', @res OUT
EXEC sp_OAMethod @res, 'open', NULL, 'GET',@url,'false'
EXEC sp_OAMethod @res, 'send'
EXEC sp_OAGetProperty @res, 'status', @status OUT
INSERT INTO @ResponseText (ResponseText) EXEC sp_OAGetProperty @res, 'responseText'
EXEC sp_OADestroy @res
SELECT @status, responseText FROM @responseText

end
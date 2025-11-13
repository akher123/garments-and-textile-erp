CREATE VIEW VwMailSend
AS
Select MS.*,M.ModuleName from MailSend AS MS
INNER JOIN Module AS M
ON MS.ModuleId=M.Id

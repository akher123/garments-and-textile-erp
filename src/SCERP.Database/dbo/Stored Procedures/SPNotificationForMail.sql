-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/12/2014>
-- Description:	<> exec SPNotificationForMail 4
-- =============================================
CREATE PROCEDURE [dbo].[SPNotificationForMail]
	
     @notificationId INT = NULL 
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT N.NotificationRecipientId, K.KeyProcessName,E.Name, N.SendCopyToEmail,N.SendCopyToPhone,N.IsNotifyByEmail,N.IsNotifyByPhone,EP.EmailAddress,EP.MobilePhone
    FROM NotificationRecipient N 
	LEFT OUTER JOIN Mrc_TimeAndActionCalendar TNA ON N.TimeAndActionId = TNA.TimeAndActionId
	LEFT OUTER JOIN Employee E ON TNA.ResponsiblePersonId = E.EmployeeId
	LEFT OUTER JOIN Mrc_KeyProcess K ON TNA.KeyProcessId = K.KeyProcessId
	LEFT OUTER JOIN Mrc_SpecificationSheet S ON TNA.SpecSheetId = s.SpecificationSheetId
	LEFT OUTER JOIN EmployeePresentAddress EP ON TNA.ResponsiblePersonId = EP.EmployeeId

	WHERE N.NotificationRecipientId = @notificationId
	AND E.[Status] = 1 AND E.IsActive=1
	ORDER BY N.NotificationRecipientId DESC
	   
END






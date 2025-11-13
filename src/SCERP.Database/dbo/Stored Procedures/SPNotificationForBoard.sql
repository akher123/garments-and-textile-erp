-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <27/12/2014>
-- Description:	<> exec [SPNotificationForBoard]
-- =============================================
CREATE PROCEDURE [dbo].[SPNotificationForBoard]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT B.BuyerName,S.StyleNo,S.JobNo,O.OrderNo,K.KeyProcessName,TNA.PlannedStartDate,TNA.PlannedEndDate, D.Name AS Title, E.Name, TNA.ActualStartDate,TNA.ActualEndDate	
    FROM NotificationRecipient N 
	LEFT OUTER JOIN Mrc_TimeAndActionCalendar TNA ON N.TimeAndActionId = TNA.TimeAndActionId
	LEFT OUTER JOIN Employee E ON TNA.ResponsiblePersonId = E.EmployeeId
	LEFT OUTER JOIN Mrc_KeyProcess K ON TNA.KeyProcessId = K.KeyProcessId
	LEFT OUTER JOIN Mrc_SpecificationSheet S ON TNA.SpecSheetId = s.SpecificationSheetId
	LEFT OUTER JOIN EmployeePresentAddress EP ON TNA.ResponsiblePersonId = EP.EmployeeId
	LEFT OUTER JOIN Department D ON TNA.DepartmentId = D.Id
	LEFT OUTER JOIN Mrc_Buyer B ON S.BuyerId = B.Id
	LEFT OUTER JOIN Mrc_OrderInformation O ON S.SpecificationSheetId = O.SpecSheetId
	ORDER BY N.NotificationRecipientId DESC
	   
END





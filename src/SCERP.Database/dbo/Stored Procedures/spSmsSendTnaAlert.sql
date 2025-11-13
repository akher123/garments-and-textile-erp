CREATE procedure [dbo].[spSmsSendTnaAlert]

as
Truncate table OM_TnAAlert

INSERT INTO OM_TnAAlert
                         (PSDate, DDiff, BeforeDays, Receiver, Msg)
SELECT        tna.PSDate, DATEDIFF(day, GETDATE(), CONVERT(datetime, tna.PSDate, 103)) AS DDiff, nt.BeforeDays, nt.Receiver, LEFT(RTRIM(LTRIM(tna.ActivityName)) + ' Pending for ' + RTRIM(LTRIM(st.BuyerName)) 
                         + ' Order No-' + RTRIM(LTRIM(st.RefNo)) + ' Style No -' + RTRIM(LTRIM(st.StyleName)), 160) AS Msg
FROM            OM_TNA AS tna INNER JOIN
                         VOM_BuyOrdStyle AS st ON tna.OrderStyleRefId = st.OrderStyleRefId AND tna.CompId = st.CompId INNER JOIN
                         OM_NotificationTemplate AS nt ON tna.SerialId = nt.ActivityId AND tna.CompId = nt.CompId and st.BuyerRefId=nt.BuyerRefId
WHERE        (st.ActiveStatus = 1) AND (LEN(tna.PSDate) > 0) and left(nt.Receiver,2) ='01'

INSERT INTO OM_TnAAlert
                         (PSDate, DDiff, BeforeDays, Receiver, Msg)
SELECT        tna.PSDate, DATEDIFF(day, GETDATE(), CONVERT(datetime, tna.PSDate, 103)) AS DDiff, nt.BeforeDays, isnull((SELECT TOP (1) Phone
FROM            OM_Merchandiser WHERE CompId = '001' AND EmpId = st.MerchandiserId),'01776042772')  as Receiver, LEFT(RTRIM(LTRIM(tna.ActivityName)) + ' Pending for ' + RTRIM(LTRIM(st.BuyerName)) 
                         + ' Order No-' + RTRIM(LTRIM(st.RefNo)) + ' Style No -' + RTRIM(LTRIM(st.StyleName)), 160) AS Msg
FROM            OM_TNA AS tna INNER JOIN
                         VOM_BuyOrdStyle AS st ON tna.OrderStyleRefId = st.OrderStyleRefId AND tna.CompId = st.CompId INNER JOIN
                         OM_NotificationTemplate AS nt ON tna.SerialId = nt.ActivityId AND tna.CompId = nt.CompId and st.BuyerRefId=nt.BuyerRefId
WHERE        (st.ActiveStatus = 1) AND (LEN(tna.PSDate) > 0) and left(nt.Receiver,2) ='@M'

INSERT INTO OM_TnAAlert
                         (PSDate, DDiff, BeforeDays, Receiver, Msg)
SELECT        tna.PSDate, DATEDIFF(day, GETDATE(), CONVERT(datetime, tna.PSDate, 103)) AS DDiff, nt.BeforeDays, ar.RespobsiblePhone  as Receiver, LEFT(RTRIM(LTRIM(tna.ActivityName)) + ' Pending for ' + RTRIM(LTRIM(st.BuyerName)) 
                         + ' Order No-' + RTRIM(LTRIM(st.RefNo)) + ' Style No -' + RTRIM(LTRIM(st.StyleName)), 160) AS Msg
FROM            OM_TNA AS tna INNER JOIN
                         VOM_BuyOrdStyle AS st ON tna.OrderStyleRefId = st.OrderStyleRefId AND tna.CompId = st.CompId INNER JOIN
                         OM_NotificationTemplate AS nt ON tna.SerialId = nt.ActivityId AND tna.CompId = nt.CompId and st.BuyerRefId=nt.BuyerRefId
						 inner join OM_ActivityResponsible as ar on nt.ActivityId=ar.ActivityId and tna.CompId=ar.CompId
WHERE        (st.ActiveStatus = 1) AND (LEN(tna.PSDate) > 0) and left(nt.Receiver,2) ='@R'


DECLARE @i INT = 1;
DECLARE @trec int =0;

select @trec = (select count(*) from OM_TnAAlert where DDiff = beforedays and DDiff>0)

WHILE @i <= @trec
BEGIN

   Insert into SmsLog
	( SendDateTime, Receiver, Message, SendingStatus )
	Select SendDateTime, Receiver, Msg, SendingStatus From (select row_number() Over (Order by Receiver) as SL,  getdate() as SendDateTime, Receiver, Msg, 0 as SendingStatus from OM_TnAAlert where DDiff <= beforedays and DDiff>0) as TT where SL=@i


   SET @i = @i + 1;
END;



/*
exec [dbo].[spSmsSendTnaAlert]


select * from OM_TnAAlert where DDiff>0

select * from SmsLog
*/




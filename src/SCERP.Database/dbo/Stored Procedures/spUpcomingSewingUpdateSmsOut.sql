CREATE procedure [dbo].[spUpcomingSewingUpdateSmsOut]

as

insert into SmsOut (Receiver, Message, XStatus)

SELECT ISNULL(M.Phone ,'01912297684') as Receiver, 'Upcomming Sewing as Plan for Buyer: ' +  VOM_BuyOrdStyle.BuyerName  + ', Order: ' + VOM_BuyOrdStyle.RefNo + ', Style: ' + VOM_BuyOrdStyle.StyleName + ', Plan Date: ' + convert(varchar(10),SDate,103) + ' Remains '+ CAST( DATEDIFF(d , GETDATE(),SDate) AS VARCHAR(MAX))+' days' as SweingStatus, 'W' as XStatus
FROM         OM_TNA
inner join VOM_BuyOrdStyle  on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
inner join OM_Merchandiser as M on VOM_BuyOrdStyle.MerchandiserId=M.EmpId
inner join OM_BuyOrdStyle as ST on  OM_TNA.OrderStyleRefId=ST.OrderStyleRefId
WHERE     (FlagValue = 'SSD') AND (ST.ActiveStatus = 1) AND (SDate >= CAST(GETDATE() as DATE)) AND (SDate <= GETDATE()+7) AND (ASDate IS NULL)
order by VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName ,SDate





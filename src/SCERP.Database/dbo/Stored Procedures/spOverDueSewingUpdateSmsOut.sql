create procedure [dbo].[spOverDueSewingUpdateSmsOut]

as

insert into SmsOut (Receiver, Message, XStatus)

SELECT ISNULL(M.Phone ,'01912297684') as Receiver, 'Over due Sewing as Plan for Buyer: ' +  VOM_BuyOrdStyle.BuyerName  + ', Order: ' + VOM_BuyOrdStyle.RefNo + ', Style: ' + VOM_BuyOrdStyle.StyleName + ', Plan Date: ' + convert(varchar(10),SDate,103) + ' Due '+ CAST( DATEDIFF(d ,SDate,GETDATE()) AS VARCHAR(MAX))+' days' as SweingStatus, 'W' as XStatus
FROM         OM_TNA
inner join VOM_BuyOrdStyle  on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
inner join OM_Merchandiser as M on VOM_BuyOrdStyle.MerchandiserId=M.EmpId
WHERE     (FlagValue = 'SSD') AND (VOM_BuyOrdStyle.ActiveStatus = 1) AND (SDate < CAST(GETDATE() as DATE)) AND (ASDate IS NULL)
order by VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName ,SDate







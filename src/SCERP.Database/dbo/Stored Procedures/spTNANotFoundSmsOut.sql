create procedure spTNANotFoundSmsOut
 as
 
insert into SmsOut (Receiver, [Message], XStatus)

SELECT ISNULL(M.Phone ,'01912297684') as Receiver, 'TNA NOT FOUND FOR :' +  VOM_BuyOrdStyle.BuyerName  + ', Order: ' + VOM_BuyOrdStyle.RefNo + ', Style: ' + VOM_BuyOrdStyle.StyleName as [Message] , 'W' as XStatus
  from OM_BuyOrdStyle 
inner join VOM_BuyOrdStyle on OM_BuyOrdStyle.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
inner join OM_Merchandiser as M on VOM_BuyOrdStyle.MerchandiserId=M.EmpId
where  VOM_BuyOrdStyle.OrderStyleRefId  not in ( select OrderStyleRefId from OM_TNA )and VOM_BuyOrdStyle.ActiveStatus=1 and OM_BuyOrdStyle.OrderNo in (select OrderNo from OM_BuyerOrder where Closed='O' and OM_BuyerOrder.SCont ='N' )
 
 order by OM_BuyOrdStyle.EFD
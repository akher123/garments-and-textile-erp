CREATE procedure [dbo].[spTnAReport]
@CompId varchar(3),
@OrderStyleRefId varchar(7)
as 

select 
BO.RefNo as OrderNo,
ST.StyleName,
ST.ItemName,
BO.OrderDate,
(select top(1) SeasonName from OM_Season where SeasonRefId=OST.SeasonRefId and CompId=OST.CompId) as Season,
B.BuyerName as Buyer,
Mrc.EmpName as Merchandiser,
(select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=OST.OrderNo and  BSP.OrderStyleRefId=OST.OrderStyleRefId) as ShipDate,
OST.Quantity as StyleQty,
TNA.ActivityName,
TNA.PSDate,
TNA.PEDate,
dbo.fnDateConvert(TNA.ASDate)  as ASDate,
dbo.fnDateConvert(TNA.AEDate)  as AEDate,

TNA.Responsible,
TNA.Rmks,
TNA.UpdateRemarks,
TNA.SerialId,
TNA.XWhen,
TNA.XWho

from 
OM_BuyerOrder AS BO
inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
inner join OM_TNA AS TNA ON OST.OrderStyleRefId=TNA.OrderStyleRefId and OST.CompId=TNA.CompId
inner join VStyle as ST on OST.StyleRefId =ST.StylerefId
inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
where BO.CompId=@CompId and TNA.OrderStyleRefId =@OrderStyleRefId
order by TNA.SerialId
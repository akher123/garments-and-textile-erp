
CREATE proc [dbo].[SPOmConfirmedOderDetail] 
@CompId varchar(3)
as
select 
BO.RefNo as OrderNo,
ST.StyleName,
B.BuyerName as Buyer,
Mrc.EmpName as Merchandiser,
(select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=OST.OrderNo and  BSP.OrderStyleRefId=OST.OrderStyleRefId) as ShipDate,
OST.Quantity as StyleQty,
0 as FinishQty,
ISNULL(OST.despatchQty ,0) as ShipQty,
(OST.Quantity-ISNULL(OST.despatchQty ,0)) as BalanceQty
from OM_BuyerOrder AS BO
inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId =ST.StylerefId
inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
where BO.CompId=@CompId
order by BO.BuyerOrderId




CREATE proc [dbo].[SPOmConfirmedOderStatus] 

@CompId varchar(3)
as
select 
BO.RefNo as OrderNo,
B.BuyerName as Buyer,
Mrc.EmpName as Merchandiser
,BO.Quantity,ISNULL(BO.OAmount,0) as Value,
(select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo) as ShipDate,
ISNULL((select SUM(DespatchQty) from OM_BuyOrdStyle as BOS where BO.CompId=BOS.CompId and BO.OrderNo=BOS.OrderNo  ),0) as ShipQty,0 as FinishQty ,(BO.Quantity- ISNULL((select SUM(DespatchQty) from OM_BuyOrdStyle as BOS where BO.CompId=BOS.CompId and BO.OrderNo=BOS.OrderNo  ),0)) as Balance
from OM_BuyerOrder AS BO
inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
where BO.CompId=@CompId
order by BO.BuyerOrderId



select DespatchQty from OM_BuyOrdStyle 






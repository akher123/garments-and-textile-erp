CREATE proc [dbo].[SPOmMerchandiserWiseOrderSummary] 
@FromDate datetime = NULL,
@ToDate datetime=null,
@CompId varchar(3)
as
select BO.RefNo as OrderNo,BO.OrderNo as OrderRefId,
B.BuyerName,
Mrc.EmpName as Merchandiser
,SUM(ISNULL(BOST.Quantity,0)- ISNULL(BOST.despatchQty,0)) as Quantity,ISNULL(SUM(ISNULL(BOST.Quantity,0)- ISNULL(BOST.despatchQty,0))*ISNULL(BOST.Rate,0) ,0) as Value,
(select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo) as ShipDate
from OM_BuyerOrder AS BO
inner join OM_BuyOrdStyle as BOST on BO.OrderNo=BOST.OrderNo and BO.CompId=BOST.CompId
inner join OM_Buyer AS B ON BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as Mrc on BO.MerchandiserId=Mrc.EmpId and BO.CompId=Mrc.CompId 
where BO.CompId=@CompId and  (select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo) between @FromDate and @ToDate and BOST.ActiveStatus=1
group by BO.RefNo ,BO.OrderNo,BOST.Rate,
B.BuyerName,
Mrc.EmpName 








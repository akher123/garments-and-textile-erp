CREATE procedure SpOmRunningOrderSumamry
@CompId varchar(3)
as
select OM_Merchandiser.EmpName,
OM_Buyer.BuyerName,
SUM(ISNULL(OM_BuyOrdStyle.Quantity,0)-ISNULL(OM_BuyOrdStyle.despatchQty,0)) as TotalQuantity,
SUM((ISNULL(OM_BuyOrdStyle.Quantity,0)-ISNULL(OM_BuyOrdStyle.despatchQty,0))*ISNULL(OM_BuyOrdStyle.Rate,0)) as TotalAmount
from OM_BuyerOrder 
inner join OM_BuyOrdStyle ON OM_BuyerOrder.OrderNo=OM_BuyOrdStyle.OrderNo and OM_BuyerOrder.CompId=OM_BuyerOrder.CompId
inner join OM_Merchandiser on OM_BuyerOrder.MerchandiserId=OM_Merchandiser.EmpId and OM_Merchandiser.CompId=OM_Merchandiser.CompId 
inner join OM_Buyer on OM_BuyerOrder.BuyerRefId=OM_Buyer.BuyerRefId and OM_BuyerOrder.CompId=OM_Buyer.CompId
where OM_BuyOrdStyle.ActiveStatus=1 and OM_BuyOrdStyle.CompId=@CompId
group by OM_Merchandiser.EmpName,OM_Buyer.BuyerName
order by OM_Merchandiser.EmpName

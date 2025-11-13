CREATE proc [dbo].[SPOmBuyerWiseOrderSummary] 
@FromDate datetime = NULL,
@ToDate datetime=null,
@CompId varchar(3),
@BuyerRefId varchar(3)
as
select M.EmpName as Merchandiser, B.BuyerName as Buyer, BO.OrderNo as OrderRefId,BO.RefNo as OrderNo,BO.Fab as Fabrication,ISNULL(S.SeasonName,'----') as Season,BO.Quantity ,BO.OAmount as Value,
BO.OrderDate,
(select MAX(ShipDate)  from OM_BuyOrdShip as BS where BS.CompId=BO.CompId and BS.OrderNo=BO.OrderNo) as ShipDate 
from OM_BuyerOrder as BO
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and Bo.CompId=M.CompId
left join OM_Season as S on BO.SeasonRefId=S.SeasonRefId and BO.CompId=S.CompId
where BO.CompId=@CompId and (Bo.BuyerRefId=@BuyerRefId or @BuyerRefId='-1') and (select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo) between @FromDate and @ToDate
order by BO.BuyerOrderId








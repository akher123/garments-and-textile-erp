CREATE procedure [dbo].[SpRunningOrderSummary]
@RMonth varchar(10),
@RYear varchar(5),
@RBuyer varchar(max)
as



select OM_Buyer.BuyerName as BUYER,
OM_BuyerOrder.RefNo as [ORDER],
OM_Style.StyleName as [STYLE],
I.ItemName as [ITEM],
Format(SH.ShipDate,'dd/MM/yyyy') as [EXF DATE],
Convert(int ,SUM(SH.Quantity)) as [ORDER QTY],
Convert(float,(SUM(SH.Quantity* ISNULL(OM_BuyOrdStyle.Rate,0))/SUM(SH.Quantity))) as [RATE($)],
Convert(float,SUM(SH.Quantity* ISNULL(OM_BuyOrdStyle.Rate,0))) as [VALUE($)],

SUM(ISNULL(SH.DespatchQty,0))as [SHIP QTY],
SUM(SH.Quantity-ISNULL(SH.DespatchQty,0)) as [BALANCE QTY],
OM_BuyerOrder.OrderStatus as [STATUS],
IIF((select COUNT(OrderStyleRefId) from OM_TNA where OrderStyleRefId=OM_BuyOrdStyle.OrderStyleRefId)>=10,'YES','NO') AS [TNA],
IIF((select COUNT(OrderStyleRefId) from OM_CostOrdStyle where OrderStyleRefId=OM_BuyOrdStyle.OrderStyleRefId)>=1,'YES','NO') AS [COST SHEET],
IIF((select COUNT(OrderStyleRefId) from OM_FabricOrderDetail where OrderStyleRefId=OM_BuyOrdStyle.OrderStyleRefId)>0,'YES','NO') AS [FAB_BOM],
IIF((select COUNT(OrderStyleRefId) from CommPurchaseOrder where OrderStyleRefId=OM_BuyOrdStyle.OrderStyleRefId)>0,'YES','NO') AS [ACC_BOM],
OM_BuyerOrder.Remarks as [REMARKS],
OM_BuyOrdStyle.OrderStyleRefId
from OM_BuyerOrder
inner join OM_BuyOrdStyle on OM_BuyerOrder.OrderNo=OM_BuyOrdStyle.OrderNo and  OM_BuyerOrder.CompId=OM_BuyOrdStyle.CompId
inner join OM_BuyOrdShip as SH on SH.OrderStyleRefId=OM_BuyOrdStyle.OrderStyleRefId and SH.CompId=OM_BuyOrdStyle.CompId 
inner join OM_Buyer on OM_BuyerOrder.BuyerRefId=OM_Buyer.BuyerRefId and OM_BuyerOrder.CompId=OM_Buyer.CompId
inner join OM_Style on OM_BuyOrdStyle.StyleRefId=OM_Style.StylerefId and OM_BuyOrdStyle.CompId=OM_Style.CompID
inner join Inventory_Item as I on OM_Style.ItemId=I.ItemId
where OM_Buyer.BuyerName=@RBuyer and RIGHT(YEAR(SH.ShipDate), 2) =@RYear and FORMAT(SH.ShipDate,'MMM')=@RMonth  and OM_BuyOrdStyle.ActiveStatus=1 and OM_BuyOrdStyle.CompId='001'
group by OM_Buyer.BuyerName,OM_BuyerOrder.RefNo,OM_Style.StyleName,I.ItemName,SH.ShipDate,OM_BuyerOrder.OrderStatus,OM_BuyerOrder.Remarks,OM_BuyOrdStyle.OrderStyleRefId








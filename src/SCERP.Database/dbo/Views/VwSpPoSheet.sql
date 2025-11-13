

CREATE view [dbo].[VwSpPoSheet]
as
select
PO.PurchaseOrderRefId,
PO.PurchaseOrderId,
POD.PurchaseOrderDetailId,
I.ItemId,
I.ItemName,
(select UnitName from MeasurementUnit where UnitId=I.MeasurementUinitId) as UnitName ,
C.ColorName,
S.SizeName,
ISNULL(POD.Quantity,0) as Quantity,
CCD.GSizeName,
CCD.GColorName,
CCD.GColorRefId,
CCD.GSizeRefId,
PO.OrderStyleRefId,
ISNULL(POD.xRate,0) as xRate ,
POD.ColorRefId,
POD.SizeRefId,
(select sum(ReceivedQty)-SUM(ISNULL(RejectedQty,0)) from Inventory_MaterialReceiveAgainstPoDetail where PurchaseOrderDetailId=POD.PurchaseOrderDetailId) as TotalRcvQty
from CommPurchaseOrder as PO
inner join CommPurchaseOrderDetail as POD on PO.PurchaseOrderId=POD.PurchaseOrderId
inner join Inventory_Item as I on POD.ItemCode=I.ItemCode 
left join VConsumption as CD on I.ItemCode=CD.ItemCode and PO.OrderStyleRefId=CD.OrderStyleRefId and PO.CompId=CD.CompId
left join VConsumptionDetail as CCD on CD.ConsRefId=CCD.ConsRefId and CD.CompId=CCD.CompId and ISNULL(POD.SizeRefId,'0000')=ISNULL(CCD.PSizeRefId,'0000')  and ISNULL(POD.ColorRefId,'0000')=ISNULL(CCD.PColorRefId,'0000') and ISNULL(POD.GSizeRefId,'0000')=ISNULL(CCD.GSizeRefId,'0000')  and ISNULL(POD.GColorRefId,'0000')=ISNULL(CCD.GColorRefId,'0000')
left join OM_Color as C on POD.ColorRefId=ISNULL(C.ColorRefId,'0000') and POD.CompId=C.CompId
left join OM_Size as S on POD.SizeRefId=S.SizeRefId  and  POD.CompId=S.CompId

where PO.PType='A' and POD.Quantity >0











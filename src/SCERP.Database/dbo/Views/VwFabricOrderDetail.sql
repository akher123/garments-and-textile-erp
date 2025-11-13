
CREATE view [dbo].[VwFabricOrderDetail]
as
select 
FO.FabricOrderRefId,
FO.FabricOrderId,
FO.[Status] as ActiveStatus, 
OST.OrderStyleRefId,
FO.BuyerRefId,
OST.OrderNo,
FO.ExpDate,
FO.OrderDate,
FO.CompId,
FOD.YLocked,
M.EmpName as Merchandiser,
OST.EFD as ShipDate,
(select BuyerName from OM_Buyer where BuyerRefId=FO.BuyerRefId and CompId=FO.CompId) as BuyerName,
BO.RefNo as OrderName,
ST.StyleName,
(select ItemName from  Inventory_Item where ItemId=ST.ItemId )as ItemName 
,CAST(ISNULL((select SUM([TQty]) from OM_CompConsumptionDetail where OrderStyleRefId=FOD.OrderStyleRefId AND CompID=FOD.CompId),0.0) as decimal(19,3))  AS FinishQty,
CAST(ISNULL((select SUM(ISNULL((TQty/(1-ProcessLoss*0.01)),0.0) ) from OM_CompConsumptionDetail where OrderStyleRefId=FOD.OrderStyleRefId AND CompID=FOD.CompId),0) as decimal(19,3)) AS GreyQty,
CAST(ISNULL((select SUM(POD.Quantity) from CommPurchaseOrder AS PO
INNER JOIN CommPurchaseOrderDetail AS POD ON PO.PurchaseOrderRefId=POD.PurchaseOrderRefId AND PO.CompId=POD.CompId
WHERE PO.OrderStyleRefId=FOD.OrderStyleRefId AND PO.CompId=FOD.CompId AND PO.PType='Y'),0.0) as decimal(19,3)) AS BookingQty
from OM_FabricOrder as FO
inner join OM_FabricOrderDetail  as FOD on FO.FabricOrderId=FOD.FabricOrderId
inner join OM_BuyerOrder as BO on FO.OrderNo=BO.OrderNo and FO.CompId=BO.CompId
inner join OM_BuyOrdStyle as OST on FOD.OrderStyleRefId=OST.OrderStyleRefId and FOD.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId

where FO.[Status]='A' AND OST.ActiveStatus=1

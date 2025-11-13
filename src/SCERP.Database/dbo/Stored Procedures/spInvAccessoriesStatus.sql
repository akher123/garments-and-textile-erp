create procedure spInvAccessoriesStatus
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as

SELECT  
MR.PoNo,
MR.Supplier,
BO.BuyerName,
Bo.RefNo as OrderNo,
BO.StyleName,
MRD.ItemCode, 
MRD.ItemName,
MRD.ColorName,
MRD.SizeName,
MRD.FColorName ,
MRD.GSizeName,     
MRD.ReceivedRate as Rate, 
(select SUM(Quantity) from CommPurchaseOrderDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId) as BookingQty,
 SUM(MRD.ReceivedQty - ISNULL(MRD.RejectedQty, 0)) AS TotalRcvQty, ISNULL((select sum(IssueQty) from Inventory_AdvanceMaterialIssueDetail where PurchaseOrderDetailId=MRD.PurchaseOrderDetailId),0) AS ToalIssueQty
FROM            VwMaterialReceiveAgainstPoDetail AS MRD INNER JOIN
                         VwMaterialReceiveAgainstPo AS MR ON MRD.MaterialReceiveAgstPoId = MR.MaterialReceiveAgstPoId
						 inner join VOM_BuyOrdStyle  as BO ON MR.OrderStyleRefId=BO.OrderStyleRefId and MR.CompId=BO.CompId
WHERE        (MRD.RType = 'P') and MR.OrderStyleRefId=@OrderStyleRefId and MRD.CompId=@CompId
GROUP  BY MR.PoNo, MR.Supplier, BO.BuyerName,
Bo.RefNo ,
BO.StyleName, MRD.ItemCode, MRD.ItemName,MRD.ItemId, MRD.ColorRefId, MRD.SizeRefId,MRD.FColorRefId,  MRD.FColorName, MRD.GSizeRefId, MRD.ColorName, MRD.SizeName, MRD.FColorName, MRD.GSizeName, MRD.PurchaseOrderDetailId, 
                         MRD.ReceivedRate







		
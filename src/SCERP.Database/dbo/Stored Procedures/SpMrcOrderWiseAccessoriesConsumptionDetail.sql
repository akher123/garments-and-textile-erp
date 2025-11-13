CREATE procedure [dbo].[SpMrcOrderWiseAccessoriesConsumptionDetail] 
 
@CompId varchar(3),
@OrderNo  varchar(12)

as 
SELECT   
 M.ItemCode, 
replace(I.ItemName,' ','') as ItemName,
GC.ColorName as GColorName,
GS.SizeName as GSizeName,
C.ColorName,
S.SizeName, 
U.UnitName,
sum(round(D.QuantityP,3)) as Quantity,
sum(round((D.PPQty * 12),3)) AS [Cons/DZ],
sum(round(D.PAllow,2)) as Allowance, 
sum(round(D.TotalQty,3)) as Total,
D.Remarks as Code,

ISNULL((select SUM(Quantity) from CommPurchaseOrderDetail 
inner join CommPurchaseOrder on CommPurchaseOrderDetail.PurchaseOrderId=CommPurchaseOrder.PurchaseOrderId
where  ItemCode=M.ItemCode and ColorRefId=ISNULL( D.PColorRefId,'0000') and SizeRefId=ISNULL(  D.PSizeRefId,'0000') and GColorRefId=ISNULL(D.GColorRefId,'0000') and GSizeRefId=ISNULL(D.GSizeRefId,'0000') and CommPurchaseOrder.OrderNo=OST.OrderNo),0) as BookingQty,

ISNULL((select  SUM(MRD.ReceivedQty - ISNULL(MRD.RejectedQty, 0))  from Inventory_MaterialReceiveAgainstPoDetail as MRD
inner join Inventory_MaterialReceiveAgainstPo as MR on MRD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
inner join Inventory_Item as I on MRD.ItemId=I.ItemId
where MR.RType='P' and MR.StoreId=2 and  MR.OrderNo=OST.OrderNo and I.ItemCode=M.ItemCode and MRD.ColorRefId=ISNULL(D.PColorRefId,'0000') and MRD.SizeRefId=ISNULL(D.PSizeRefId,'0000') and MRD.GSizeRefId=ISNULL(D.GSizeRefId,'0000') and MRD.FColorRefId=ISNULL(D.GColorRefId,'0000')),0) AS TotalRcvQty,

ISNULL((select sum(IssueQty) from Inventory_AdvanceMaterialIssueDetail  as MID
inner join Inventory_Item as I on MID.ItemId=I.ItemId
inner join Inventory_AdvanceMaterialIssue as MI  on MID.AdvanceMaterialIssueId=MI.AdvanceMaterialIssueId
where MI.StoreId=2 and MID.ColorRefId=ISNULL(D.PColorRefId,'0000') and MID.SizeRefId=ISNULL(D.PSizeRefId,'0000')  and MID.GSizeRefId=ISNULL(D.GSizeRefId,'0000')  and MID.FColorRefId=ISNULL(D.GColorRefId,'0000') and I.ItemCode=M.ItemCode and MI.OrderNo=OST.OrderNo),0) as ToalIssueQty


FROM  
 OM_ConsumptionDetail AS D
 inner JOIN OM_Consumption AS M ON D.CompId = M.CompId AND D.ConsRefId = M.ConsRefId
 inner join OM_BuyOrdStyle as OST on M.OrderStyleRefId=OST.OrderStyleRefId and M.CompId=OST.CompId
 inner join Inventory_Item as I on M.ItemCode=I.ItemCode and D.CompId=I.CompId
 left join OM_Color as  GC on D.GColorRefId=GC.ColorRefId and D.CompId=GC.CompId
  left join OM_Size as GS on D.GSizeRefId=GS.SizeRefId and D.CompId=GS.CompId
 left join MeasurementUnit as U on I.MeasurementUinitId=U.UnitId
 left join OM_Color as C on D.PColorRefId=C.ColorRefId and D.CompId=C.CompId
 left join OM_Size as S on D.PSizeRefId=S.SizeRefId and D.CompId=S.CompId
 left join  OM_BuyOrdStyleSize as BSS on M.OrderStyleRefId=BSS.OrderStyleRefId and M.CompId=BSS.CompId and D.GSizeRefId=BSS.SizeRefId

WHERE (D.CompId =@CompId) AND (OST.OrderNo=@OrderNo) and M.ConsGroup='B' and left(I.ItemCode,2) in('03','04') 

 group by   M.ItemCode, I.ItemName,GC.ColorName,GS.SizeName, C.ColorName,S.SizeName,U.UnitName,D.Remarks,BSS.SizeRow, D.PColorRefId,D.PSizeRefId,D.GColorRefId,D.GSizeRefId,OST.OrderNo

order by BSS.SizeRow


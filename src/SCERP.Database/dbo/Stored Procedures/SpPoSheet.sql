CREATE procedure [dbo].[SpPoSheet]
@CompId varchar(3),
@PurchaseOrderId bigint
as 
select
CM.Name as CompanyName,
CM.FullAddress as FullAddress, 
BOST.BuyerName as BuyerName,
BOST.RefNo as OrderName,
BOST.StyleName as StyleName,

PO.PurchaseOrderRefId,
SC.CompanyName as Supplier,
SC.[Address] as SupplierAddress ,
SC.Email,
SC.Phone,
SC.ContactName,
PO.PurchaseOrderDate as PoDate, 
PO.ExpDate,PO.Rmks,
isnull(( Select top 1 ItemName From Inventory_Item as I where ItemCode=POD.ItemCode),'-') as ItemName,
isnull(( Select top 1 ColorName From  OM_Color as PC where  CompId= POD.CompId AND ColorRefId= POD.ColorRefId),'-') as ColorName,
isnull(( Select top 1 SizeName From  OM_Size as PS where  CompId= POD.CompId AND SizeRefId= POD.SizeRefId),'-')  as SizeName,
isnull(( SELECT        TOP (1) MeasurementUnit.UnitName
FROM  Inventory_Item INNER JOIN MeasurementUnit ON Inventory_Item.MeasurementUinitId = MeasurementUnit.UnitId AND Inventory_Item.ItemCode=POD.ItemCode),'-') as UnitName,
ISNULL(POD.Quantity,0) as Quantity,
(SELECT TOP (1) CND.QuantityP
FROM  OM_ConsumptionDetail AS CND 
INNER JOIN OM_Consumption AS CN ON CND.CompId = CN.CompId AND CND.ConsRefId = CN.ConsRefId
WHERE  (CN.OrderStyleRefId = PO.OrderStyleRefId) AND (CN.ItemCode = POD.ItemCode) AND (CN.CompId = PO.CompId) AND (COALESCE(CND.GColorRefId,'0000') =COALESCE( POD.GColorRefId,'0000')) AND (COALESCE(CND.GSizeRefId,'0000') =COALESCE( POD.GSizeRefId,'0000') )  AND (COALESCE(CND.PColorRefId,'0000') =COALESCE( POD.ColorRefId ,'0000'))) as QuantityP,

(SELECT TOP (1) CND.Remarks
FROM  OM_ConsumptionDetail AS CND 
INNER JOIN OM_Consumption AS CN ON CND.CompId = CN.CompId AND CND.ConsRefId = CN.ConsRefId
WHERE  (CN.OrderStyleRefId = PO.OrderStyleRefId) AND (CN.ItemCode = POD.ItemCode) AND (CN.CompId = PO.CompId) AND (COALESCE(CND.GColorRefId,'0000') =COALESCE( POD.GColorRefId,'0000')) AND (COALESCE(CND.GSizeRefId,'0000') =COALESCE( POD.GSizeRefId,'0000') )  AND (COALESCE(CND.PColorRefId,'0000') =COALESCE( POD.ColorRefId ,'0000'))) as Code,

isnull(( Select top 1 SizeName From  OM_Size as PSG where  CompId= POD.CompId AND SizeRefId= POD.GSizeRefId),'-') as GSizeName,
isnull(( Select top 1 ColorName From  OM_Color as PCG where  CompId= POD.CompId AND ColorRefId= POD.GColorRefId),'-')  as GColorName,

E.Name as Creator,
ISNULL(POD.xRate,0) as xRate ,

IIF((select COUNT(1)  from CommPurchaseOrder as  spo
inner join CommPurchaseOrderDetail as spod on spo.PurchaseOrderId=spod.PurchaseOrderId
where spo.OrderStyleRefId=PO.OrderStyleRefId and ItemCode=POD.ItemCode  and ColorRefId=POD.ColorRefId and SizeRefId=POD.SizeRefId and GColorRefId=POD.GColorRefId and GSizeRefId=POD.GSizeRefId)>1,1,0) as RptStatus

from CommPurchaseOrder as PO 
inner join CommPurchaseOrderDetail as POD on PO.PurchaseOrderId=POD.PurchaseOrderId
inner join VOM_BuyOrdStyle as BOST on PO.OrderNo=BOST.OrderNo and PO.OrderStyleRefId=BOST.OrderStyleRefId and Po.CompId=BOST.CompId						
inner join Mrc_SupplierCompany as SC on PO.SupplierId=SC.SupplierCompanyId
inner join Employee as E on PO.UserId=E.EmployeeId
inner join Company as CM on PO.CompId=CM.CompanyRefId
where PO.PType='A' and PO.PurchaseOrderId=@PurchaseOrderId and POD.Quantity >0








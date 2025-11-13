create  procedure [dbo].[SpPoSheet1]
@CompId varchar(3),
@PurchaseOrderId bigint
as 
select
distinct CM.Name as CompanyName,
CM.FullAddress, 
BOST.BuyerName,
BOST.RefNo as OrderName,
BOST.StyleName,
PO.PurchaseOrderRefId,
SC.CompanyName as Supplier,
SC.[Address] as SupplierAddress ,
SC.Email,
SC.Phone,
SC.ContactName,
PO.PurchaseOrderDate as PoDate, 
PO.ExpDate,PO.Rmks,
REPLACE (I.ItemName,' ','') as ItemName,
C.ColorName,
S.SizeName,
M.UnitName,
ISNULL(POD.Quantity,0) as Quantity,
--0 as QuantityP,
--'' as  GSizeName,
--'' as GColorName,
(select sum(CCD.QuantityP) from VConsumptionDetail as CCD where  CD.ConsRefId=CCD.ConsRefId ) as QuantityP,
(select top(1) CCD.GSizeName from VConsumptionDetail as CCD where  CD.ConsRefId=CCD.ConsRefId  and CD.CompId=CCD.CompId and ISNULL(POD.SizeRefId,'0000')=ISNULL(CCD.PSizeRefId,'0000')  and ISNULL(POD.ColorRefId,'0000')=ISNULL(CCD.PColorRefId,'0000')) as GSizeName,
(select top(1) CCD.GColorName from VConsumptionDetail as CCD where  CD.ConsRefId=CCD.ConsRefId  and CD.CompId=CCD.CompId and ISNULL(POD.SizeRefId,'0000')=ISNULL(CCD.PSizeRefId,'0000')  and ISNULL(POD.ColorRefId,'0000')=ISNULL(CCD.PColorRefId,'0000')) as GColorName,

E.Name as Creator,
ISNULL(POD.xRate,0) as xRate from CommPurchaseOrder as PO
inner join CommPurchaseOrderDetail as POD on PO.PurchaseOrderId=POD.PurchaseOrderId
inner join VOM_BuyOrdStyle as BOST on PO.OrderNo=BOST.OrderNo and PO.OrderStyleRefId=BOST.OrderStyleRefId and Po.CompId=BOST.CompId
inner join Inventory_Item as I on POD.ItemCode=I.ItemCode 
inner join Mrc_SupplierCompany as SC on PO.SupplierId=SC.SupplierCompanyId
inner join Employee as E on PO.UserId=E.EmployeeId
left join VConsumption as CD on I.ItemCode=CD.ItemCode and PO.OrderStyleRefId=CD.OrderStyleRefId and PO.CompId=CD.CompId
--left join VConsumptionDetail as CCD on CD.ConsRefId=CCD.ConsRefId  and CD.CompId=CCD.CompId and ISNULL(POD.SizeRefId,'0000')=ISNULL(CCD.PSizeRefId,'0000')  and ISNULL(POD.ColorRefId,'0000')=ISNULL(CCD.PColorRefId,'0000')
left join MeasurementUnit as M on I.MeasurementUinitId=M.UnitId
left join OM_Color as C on POD.ColorRefId=ISNULL(C.ColorRefId,'0000') and POD.CompId=C.CompId
left join OM_Size as S on POD.SizeRefId=S.SizeRefId  and  POD.CompId=S.CompId
inner join Company as CM on PO.CompId=CM.CompanyRefId
where PO.PType='A' and PO.PurchaseOrderId=@PurchaseOrderId and POD.Quantity >0


--exec [SpPoSheet] '001',48


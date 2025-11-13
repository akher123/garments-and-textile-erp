CREATE procedure [dbo].[SpOmYarnWorkOrderReport]
@PurchaseOrderId bigint, 
@CompId varchar(3)
as 
select BOST.BuyerName,
BOST.RefNo as OrderName,
BOST.Merchandiser,
BOST.StyleName, 
ISNULL((select top(1) PiNo from ProFormaInvoice where CompId=PO.CompId and PiRefId=PO.PurchaseOrderNo AND SupplierId=PO.SupplierId),PO.PurchaseOrderNo) AS   PurchaseOrderRefId,
PO.PurchaseOrderDate,
PO.ExpDate,SC.[Address] as SupplierAddress,
SC.CompanyName as Supplier,
SC.Phone,
SC.ContactName,
PO.Rmks,
I.ItemCode ,
I.ItemName,
Cnt.SizeName as CountName,
C.ColorName,
MU.UnitName,
SUM(POD.Quantity) AS Quantity ,
POD.xRate ,
CP.Name as CompanyName,
CP.FullAddress,
E.Name as Employee
 from CommPurchaseOrder as PO 
inner join CommPurchaseOrderDetail as POD on PO.PurchaseOrderId=POD.PurchaseOrderId
inner join OM_Size as Cnt on POD.SizeRefId=Cnt.SizeRefId
inner join OM_Color as C on POD.ColorRefId=C.ColorRefId and POD.CompId=C.CompId
inner join Mrc_SupplierCompany as SC on PO.SupplierId=SC.SupplierCompanyId

inner join VOM_BuyOrdStyle as BOST on PO.OrderStyleRefId=BOST.OrderStyleRefId and PO.CompId=BOST.CompId
inner join Inventory_Item as I on POD.ItemCode=I.ItemCode and POD.CompId=I.CompId
inner join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
inner join Company as CP on PO.CompId=CP.CompanyRefId
inner join Employee as E on PO.UserId=E.EmployeeId
where PO.PType='Y' and PO.PurchaseOrderId=@PurchaseOrderId and PO.CompId=@CompId

GROUP BY BOST.BuyerName,
BOST.RefNo ,
BOST.Merchandiser,
BOST.StyleName, 
PO.PurchaseOrderRefId,
PO.PurchaseOrderDate,
PO.ExpDate,SC.[Address] ,
SC.CompanyName ,
SC.Phone,
SC.ContactName,
PO.Rmks,
I.ItemCode ,
I.ItemName,
Cnt.SizeName ,
C.ColorName,
MU.UnitName,
POD.xRate ,
CP.Name,
CP.FullAddress,
E.Name ,
PO.SupplierId,
PO.PurchaseOrderNo,
PO.CompId



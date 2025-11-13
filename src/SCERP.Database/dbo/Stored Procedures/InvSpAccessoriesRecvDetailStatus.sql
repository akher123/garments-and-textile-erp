CREATE procedure [dbo].[InvSpAccessoriesRecvDetailStatus]
@FromDate dateTime,
@ToDate dateTime,
@CompId varchar(3)
as
select 
(select BuyerName from OM_Buyer where BuyerId=MR.BuyerId ) as BuyerName,
(select RefNo from VOM_BuyOrdStyle where OrderStyleRefId=MR.OrderStyleRefId and CompId=MR.CompId) as OrderNo,
(select StyleName from VOM_BuyOrdStyle where OrderStyleRefId=MR.OrderStyleRefId and CompId=MR.CompId) as StyleName,
MR.PoNo,
(select CompanyName from Mrc_SupplierCompany where SupplierCompanyId=MR.SupplierId ) as Supplier,
MR.RefNo as  MRRNo , 
MR.MRRDate,
MR.InvoiceNo,
MR.InvoiceDate,
(select ColorName from OM_Color where ColorRefId=MRD.ColorRefId and CompId=MR.CompId ) as PColorName,
(select SizeName from OM_Size where SizeRefId=MRD.SizeRefId and CompId=MR.CompId ) as PSizeName,
(select ColorName from OM_Color where ColorRefId=MRD.FColorRefId and CompId=MR.CompId ) as GColorName,
(select SizeName from OM_Size where SizeRefId=MRD.GSizeRefId and CompId=MR.CompId ) as GSizeName,
MRD.Location,
MRD.ReceivedQty,
MRD.ReceivedRate,
MRD.RejectedQty,
(select ItemName from Inventory_Item where ItemId=MRD.ItemId ) as ItemName,
(select UnitName from VInvItem where ItemId=MRD.ItemId ) as UnitName
from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_MaterialReceiveAgainstPoDetail as MRD on MR.MaterialReceiveAgstPoId=MRD.MaterialReceiveAgstPoId

where MR.RType='P' and MR.MRRDate between @FromDate and @ToDate





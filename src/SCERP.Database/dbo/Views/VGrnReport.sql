




CREATE  view [dbo].[VGrnReport]

as 
select ITM.ItemCode,
ITM.ItemName,
SL.Quantity,
SL.UnitPrice,
SL.Amount ,
ITS.InvoiceDate,
ITS.ReceivedRegisterNo,
ITS.ReceivedDate,
SPR.RequisitionDate,
ITS.GateEntry,
ITS.GateEntryDate,
(select top(1) ISD.Specification from dbo.Inventory_ItemStoreDetail as ISD
inner join dbo.Inventory_ItemStore as TS on ISD.ItemStoreId=TS.ItemStoreId
 where ITS.ItemStoreId= TS.ItemStoreId and  TS.InvoiceNo=ITS.InvoiceNo and (ISD.ItemId=SL.ItemId or ISD.SizeId=SL.SizeId or ISD.BrandId=SL.BrandId or SL.OriginId=ISD.OriginId) order by ISD.ItemStoreId) as Spicification,
ISNULL(SZ.Title,'--') as Size ,
ISNULL(BR.Name,'--') as Brand,
ISNULL(CT.CountryName,'--') as Origin,
ITS.InvoiceNo,ISNULL(ITS.RequisitionNo,'--') as SPRNO,
GRN.GRNNumber,GRN.GRNDate,
RefP.Name as RefPersonName,
SUPPLR.CompanyName AS SupplierCompanyName, SUPPLR.Address AS SupplierAddress,
ISNULL(COMPANY.FullAddress,'North Norshingpur, Fatullah, Narayanganj') as FullAddress,ISNULL( COMPANY.Name,'Plummy Fashions Liminted') AS CompanyName
from dbo.Inventory_StoreLedger as SL
inner join dbo.Inventory_GoodsReceivingNote as GRN on SL.GoodsReceivingNoteId=GRN.GoodsReceivingNotesId
inner join dbo.Inventory_QualityCertificate as QC on GRN.QualityCertificateId=QC.QualityCertificateId
inner join dbo.Inventory_ItemStore as ITS on QC.ItemStoreId=ITS.ItemStoreId
LEFT JOIN Mrc_SupplierCompany AS SUPPLR ON ITS.SupplierId = SUPPLR.SupplierCompanyId
LEFT JOIN Employee as RefP on ITS.PurchaseReferencePerson=RefP.EmployeeId 
inner join dbo.Inventory_Item as ITM on SL.ItemId=ITM.ItemId
left join dbo.Inventory_StorePurchaseRequisition as SPR on ITS.RequisitionNo=SPR.RequisitionNo
left join   Inventory_MaterialRequisition AS MATREQ ON SPR.MaterialRequisitionId = MATREQ.MaterialRequisitionId
LEFT JOIN BranchUnitDepartment AS BRANCHUNITDEPT ON BRANCHUNITDEPT.BranchUnitDepartmentId = MATREQ.BranchUnitDepartmentId
LEFT JOIN BranchUnit AS BRANCHUNIT ON BRANCHUNITDEPT.BranchUnitId = BRANCHUNIT.BranchUnitId 
LEFT JOIN   Branch AS BRANCH ON BRANCHUNIT.BranchId = BRANCH.Id 
LEFT JOIN Company AS COMPANY ON BRANCH.CompanyId = COMPANY.Id  
left join dbo.Inventory_Size as SZ on SL.SizeId=SZ.SizeId
left join dbo.Inventory_Brand as BR on SL.BrandId=BR.BrandId
left join  dbo.Country as CT on SL.OriginId=CT.Id
where GRN.IsActive = 1 and ITS.IsActive=1 and QC.IsActive=1 and  SL.TransactionType=1 








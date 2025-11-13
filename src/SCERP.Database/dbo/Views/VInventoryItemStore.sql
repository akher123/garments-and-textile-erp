



CREATE view [dbo].[VInventoryItemStore]
AS
SELECT 
INVSTOR.ItemStoreId,
INVSTOR.ReceiveType,
INVSTOR.GateEntry,
INVSTOR.GateEntryDate,
INVSTOR.RequisitionNo, 
INVSTOR.LCNo, 
INVSTOR.InvoiceNo, 
INVSTOR.InvoiceDate,
INVSTOR.SupplierId,
INVSTOR.ReceivedDate, 
INVSTOR.ReceivedRegisterNo, 
INVSTOR.QCStatus,
SUPPLIER.CompanyName AS SupplierCompanyName, 
SUPPLIER.SupplierCode, 
EMP.Name AS PurchaseReferencePersonName, 
EMP.EmployeeId, 
INVSTOR.IsActive
FROM           
 dbo.Inventory_ItemStore AS INVSTOR INNER JOIN
 dbo.Employee  EMP ON INVSTOR.PurchaseReferencePerson = EMP.EmployeeId INNER JOIN
 dbo.Mrc_SupplierCompany AS SUPPLIER ON INVSTOR.SupplierId = SUPPLIER.SupplierCompanyId
--where INVSTOR.IsActive=1 and EMP.[Status]=1

where INVSTOR.IsActive=1






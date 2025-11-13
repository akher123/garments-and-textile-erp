

CREATE view [dbo].[VwCommPurchaseOrder]
as 
select PO.*,
BOST.BuyerName,
BOST.BuyerRefId,
BOST.RefNo as OrderName,
BOST.StyleName,
BOST.MerchandiserId,
SUPL.CompanyName as SupplierName
from CommPurchaseOrder as PO
inner join Mrc_SupplierCompany as SUPL on PO.SupplierId=SUPL.SupplierCompanyId
inner join VOM_BuyOrdStyle as BOST on PO.OrderStyleRefId=BOST.OrderStyleRefId and BOST.CompId=PO.CompId



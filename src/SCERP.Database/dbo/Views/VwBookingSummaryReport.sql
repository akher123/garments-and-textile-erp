
CREATE view [dbo].[VwBookingSummaryReport]
as
select 
BK.BookingRefId,
BK.BookingId,
BK.BuyerId,
BK.SupplierId,
BK.MarchandiserId,
BK.CompId,
BK.OrderNo,
BK.StyleNo,
BK.BookingDate,
BK.OrderQty,
sum(BKD.Rate*BKD.Quantity) as Amount,
BK.PiNo,
B.BuyerName ,
M.EmpName as Merchandiser,
SP.CompanyName as Supplier,
CM.Name as CompanyName,
CM.FullAddress,
BK.StoreId
from Inventory_Booking as BK 
inner join Company as CM on BK.CompId=CM.CompanyRefId
inner join Inventory_BookingDetail as BKD on BK.BookingId=BKD.BookingId and BK.CompId=BKD.CompId
inner join OM_Buyer as B on BK.BuyerId=B.BuyerId and BK.CompId=B.CompId
inner join OM_Merchandiser as M on BK.MarchandiserId=M.MerchandiserId and BK.CompId=M.CompId
inner join Mrc_SupplierCompany as SP on BK.SupplierId=SP.SupplierCompanyId 
group by BK.BookingRefId,
BK.BookingId,
BK.BuyerId,
BK.SupplierId,
BK.MarchandiserId,
BK.CompId,
BK.OrderNo,
BK.StyleNo,
BK.BookingDate,
BK.OrderQty,
BK.PiNo,
B.BuyerName ,
M.EmpName ,
SP.CompanyName,
CM.Name,
CM.FullAddress,
BK.StoreId

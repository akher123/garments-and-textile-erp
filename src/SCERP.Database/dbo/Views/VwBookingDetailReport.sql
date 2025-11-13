CREATE view [dbo].[VwBookingDetailReport]
as 
select 
BK.BookingRefId,
BK.BookingId,
BK.BuyerId,
BK.SupplierId,
BK.MarchandiserId,
BKD.ColorRefId,
BKD.SizeRefId,
BK.CompId,
BK.OrderNo,
BK.StyleNo,
BK.BookingDate,
BK.OrderQty,
BK.PiNo,
B.BuyerName ,
M.EmpName as Merchandiser,
SP.CompanyName as Supplier,
C.ColorName,
S.SizeName,
I.ItemName,
BKD.ItemId,
U.UnitName,
I.ItemCode, 
BKD.Quantity,
CM.Name as CompanyName,
CM.FullAddress,
BKD.Rate,
BK.StoreId,
FC.ColorName as FColorName,
BKD.FColorRefId,
ISNULL((select  sum(RD.ReceivedQty) from VwMaterialReceiveAgainstPoDetail as RD

where RD.ItemId=BKD.ItemId and RD.PoNo=BK.BookingRefId and RD.CompId=BKD.CompId),0) as ReceivedQty from Inventory_Booking as BK 
inner join Company as CM on BK.CompId=CM.CompanyRefId
inner join Inventory_BookingDetail as BKD on BK.BookingId=BKD.BookingId and BK.CompId=BKD.CompId
inner join OM_Buyer as B on BK.BuyerId=B.BuyerId and BK.CompId=B.CompId
inner join OM_Merchandiser as M on BK.MarchandiserId=M.MerchandiserId and BK.CompId=M.CompId
inner join Mrc_SupplierCompany as SP on BK.SupplierId=SP.SupplierCompanyId 
inner join Inventory_Item as I on BKD.ItemId=I.ItemId
left join MeasurementUnit as U on I.MeasurementUinitId=U.UnitId
left join OM_Color as C on BKD.ColorRefId=C.ColorRefId and BKD.CompId=C.CompId
left join OM_Color as FC on BKD.FColorRefId=FC.ColorRefId and BKD.CompId=FC.CompId
left join OM_Size as S on BKD.SizeRefId=S.SizeRefId and BKD.CompId=S.CompId




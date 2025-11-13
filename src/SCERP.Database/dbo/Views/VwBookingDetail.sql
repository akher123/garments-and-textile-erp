CREATE view [dbo].[VwBookingDetail]
as 
select BKD.*,FC.ColorName as FColorName, I.ItemName, C.ColorName,S.SizeName  from Inventory_BookingDetail as BKD

inner join Inventory_Item as I on BKD.ItemId=I.ItemId

left join OM_Color as C on BKD.ColorRefId=C.ColorRefId and BKD.CompId=C.CompId
left join OM_Color as FC on BKD.FColorRefId=FC.ColorRefId and BKD.CompId=FC.CompId
left join OM_Size as S on BKD.SizeRefId=S.SizeRefId and BKD.CompId=S.CompId

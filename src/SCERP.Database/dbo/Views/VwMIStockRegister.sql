create view VwMIStockRegister
as
select lot.ColorName,sz.SizeName,ST.* from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_StockRegister as  ST on MR.MaterialReceiveAgstPoId=ST.SourceId and MR.StoreId=ST.StoreId 
inner join OM_Color as lot on ST.ColorRefId=Lot.ColorRefId and ST.CompId=ST.CompId
inner join OM_Size as sz on ST.SizeRefId=sz.SizeRefId and ST.CompId=ST.CompId
inner join Inventory_Brand as B on lot.ColorCode=b.BrandId 
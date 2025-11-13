create view VwMRStockRegister
as
select lot.ColorName,sz.SizeName,ST.* from Inventory_AdvanceMaterialIssue as MI
inner join Inventory_StockRegister as  ST on MI.AdvanceMaterialIssueId=ST.SourceId and MI.StoreId=ST.StoreId 
inner join OM_Color as lot on ST.ColorRefId=Lot.ColorRefId and ST.CompId=ST.CompId
inner join OM_Size as sz on ST.SizeRefId=sz.SizeRefId and ST.CompId=ST.CompId
inner join Inventory_Brand as B on lot.ColorCode=b.BrandId 
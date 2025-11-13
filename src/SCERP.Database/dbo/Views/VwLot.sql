create view VwLot

as 
select C.*,B.Name as BrandName from OM_Color as C left join Inventory_Brand as B on C.ColorCode=CAST(B.BrandId as varchar(50)) 
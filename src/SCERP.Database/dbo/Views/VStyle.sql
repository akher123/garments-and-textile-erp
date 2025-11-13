create view VStyle
as
select 
ISNULL((select ItemName from Inventory_Item where OM_Style.ItemId=Inventory_Item.ItemId),'---') as ItemName,
 * from OM_Style







CREATE view [dbo].[VItemReceiveDetail]
AS
SELECT 
ItemStoreDrtail.ItemStoreDetailId,
ItemStoreDrtail.ItemStoreId,
ItemStoreDrtail.ItemId,
ItemStoreDrtail.Specification,
ItemStoreDrtail.SuppliedQuantity,
ItemStoreDrtail.ReceivedQuantity,
ItemStoreDrtail.UnitPrice,
ItemStoreDrtail.ManufacturingDate,
ItemStoreDrtail.ExpirationDate,
CURRENCY.Name AS CurrencyName,
MEASUNIT.UnitName AS UnitName,
Item.ItemName,
ItemStoreDrtail.BrandId,
BRAND.Name AS BrandName,
COUNTRY.CountryName AS OriginName,
SIZE.Title AS SizeName,
ItemStoreDrtail.OriginId,
ItemStoreDrtail.SizeId,
Item.MeasurementUinitId AS MeasurementUnitId,
ItemStoreDrtail.CurrencyId
from Inventory_ItemStoreDetail AS ItemStoreDrtail
INNER JOIN Inventory_ItemStore AS ItemStore ON ItemStoreDrtail.ItemStoreId=ItemStore.ItemStoreId
INNER JOIN Inventory_Item AS Item ON ItemStoreDrtail.ItemId=Item.ItemId
INNER JOIN MeasurementUnit AS MEASUNIT ON Item.MeasurementUinitId=MEASUNIT.UnitId
INNER JOIN Currency AS CURRENCY ON ItemStoreDrtail.CurrencyId=CURRENCY.CurrencyId
LEFT JOIN Inventory_Size AS SIZE ON ItemStoreDrtail.SizeId=SIZE.SizeId
LEFT JOIN Inventory_Brand AS BRAND ON ItemStoreDrtail.BrandId=BRAND.BrandId
LEFT JOIN Country AS COUNTRY ON ItemStoreDrtail.OriginId=COUNTRY.Id
where ItemStoreDrtail.IsActive=1 





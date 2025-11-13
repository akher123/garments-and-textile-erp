
CREATE view [dbo].[VMaterialIssueDetail]
AS 
SELECT 
MATISSDET.MaterialIssueDetailId,
MATISSDET.MaterialIssueId,
MATISSDET.TransactionDate,
MATISSDET.RequiredQuantity,
MATISSDET.IssuedQuantity,
MATISSDET.IssuedItemRate,
MATISSDET.StockInHand,
MATISSDET.Remarks ,
MATISSDET.ItemId,
MACHINE.MachineId,
MACHINE.Name AS MachineName,
MEASUNIT.UnitId As MeasurementUnitId,
MEASUNIT.UnitName As MeasurementUnitName,
CURRENCY.CurrencyId AS CurrencyId,
CURRENCY.Name AS CurrencyName,

ITEM.ItemName,
SISE.Title AS SizeName,
SISE.SizeId ,
BRAND.Name AS BrandName,
BRAND.BrandId ,
COUNTRY.CountryName AS OriginName,
COUNTRY.Id AS OriginId
from Inventory_MaterialIssueDetail AS MATISSDET
Inner join Inventory_Item AS ITEM ON MATISSDET.ItemId=ITEM.ItemId
LEFT join Production_Machine AS MACHINE ON MACHINE.MachineId=MATISSDET.MachineId
Inner join MeasurementUnit AS MEASUNIT ON ITEM.MeasurementUinitId=MEASUNIT.UnitId
Inner join Currency AS CURRENCY ON MATISSDET.CurrencyId=CURRENCY.CurrencyId
LEFT JOIN Inventory_Size AS SISE ON MATISSDET.SizeId=SISE.SizeId
LEFT JOIN Inventory_Brand AS BRAND ON MATISSDET.BrandId=BRAND.BrandId
LEFT JOIN Country AS COUNTRY ON MATISSDET.OriginId=COUNTRY.Id

where MATISSDET.IsActive=1 









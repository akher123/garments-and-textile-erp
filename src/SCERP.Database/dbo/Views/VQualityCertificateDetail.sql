






CREATE view [dbo].[VQualityCertificateDetail]
AS
select
ISD.ItemStoreId,
IQCD.QualityCertificateId,
IQCD.QualityCertificateDetailId,
Item.ItemName,
Item.ItemId,
ISD.ItemStoreDetailId,
IQCD.Remarks,
Item.ItemCode,
ISD.Specification ,
ISD.SuppliedQuantity,
ISD.ReceivedQuantity,
IQCD.CorrectQuantity,
IQCD.RejectedQuantity,
ISD.UnitPrice,
ISD.OriginId,
ISD.SizeId,
SIZE.Title AS SizeName,
BRAND.Name AS BrandName,
BRAND.BrandId,
COUNTRY.CountryName,
ISD.ManufacturingDate,
ISD.ExpirationDate,
CUR.CurrencyId,CUR.Name AS CureencyName,
MUNT.UnitName,
MUNT.UnitId AS MeasurementUnitId
from  Inventory_QualityCertificateDetail AS IQCD INNER JOIN
                         Inventory_QualityCertificate AS IQC ON IQCD.QualityCertificateId = IQC.QualityCertificateId INNER JOIN
                         Inventory_ItemStoreDetail AS ISD ON IQCD.ItemId = ISD.ItemId INNER JOIN
                         Inventory_ItemStore AS ITS ON ISD.ItemStoreId = ITS.ItemStoreId INNER JOIN
                         Inventory_Item AS Item ON ISD.ItemId = Item.ItemId LEFT OUTER JOIN
                         Inventory_Size AS SIZE ON ISD.SizeId = SIZE.SizeId LEFT OUTER JOIN
                         Inventory_Brand AS BRAND ON ISD.BrandId = BRAND.BrandId LEFT OUTER JOIN
                         Country AS COUNTRY ON ISD.OriginId = COUNTRY.Id LEFT OUTER JOIN
                         Mrc_SupplierCompany AS Supplier ON ITS.SupplierId = Supplier.SupplierCompanyId INNER JOIN
                         MeasurementUnit AS MUNT ON Item.MeasurementUinitId = MUNT.UnitId INNER JOIN
                         Currency AS CUR ON ISD.CurrencyId = CUR.CurrencyId
where IQCD.IsActive=1









CREATE View [dbo].[VBuyOrdStyleSize]
as
SELECT    OM_BuyOrdStyleSize.*, ISNULL
                          ((SELECT     TOP (1) SizeName
                              FROM         OM_Size
                              WHERE     (CompID = OM_BuyOrdStyleSize.CompID) AND (SizeRefId = OM_BuyOrdStyleSize.SizeRefId)), '-') AS SizeName
FROM         OM_BuyOrdStyleSize 


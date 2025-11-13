CREATE View VBuyOrdStyleColor 
as
SELECT    OrderStyleColorId, CompID, OrderStyleRefId, ColorRefId, ColorRow, ISNULL
                          ((SELECT     TOP (1) ColorName
                              FROM         OM_Color
                              WHERE     (CompID = OM_BuyOrdStyleColor.CompID) AND (ColorRefId = OM_BuyOrdStyleColor.ColorRefId)), '-') AS ColorName
FROM         OM_BuyOrdStyleColor 

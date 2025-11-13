
CREATE view [dbo].[VOrderStyleDetail]
as 
SELECT       SD.CompId, S.OrderNo, S.OrderStyleRefId, ST.StyleRefId, STL.ItemId, SD.ColorRefId, SD.SizeRefId, ISNULL(SUM(SD.QuantityP),0) AS Quantity,ISNULL(I.ItemName,'--') as  ItemName,ISNULL(C.ColorName,'--') as  ColorName,ISNULL(Sz.SizeName,'--') as  SizeName ,ISNULL(M.UnitName,'--') as UnitName,I.ItemCode
FROM            dbo.OM_BuyOrdShip AS S INNER JOIN
                         dbo.OM_BuyOrdShipDetail AS SD ON S.CompId = SD.CompId AND S.OrderShipRefId = SD.OrderShipRefId INNER JOIN
                         dbo.OM_BuyOrdStyle AS ST ON S.OrderStyleRefId = ST.OrderStyleRefId AND S.CompId = ST.CompId INNER JOIN
                         dbo.OM_Style AS STL ON ST.CompId = STL.CompID AND ST.StyleRefId = STL.StylerefId  left join 
                         dbo.Inventory_Item as I on  STL.ItemId=I.ItemId left join 
						 dbo.OM_Color as C on SD.ColorRefId=C.ColorRefId and SD.CompId=C.CompId 
						 left join dbo.OM_Size as Sz on SD.SizeRefId=Sz.SizeRefId and SD.CompId=Sz.CompId 
						 left join dbo.MeasurementUnit as M on I.MeasurementUinitId=M.UnitId
GROUP BY SD.CompId, S.OrderNo, S.OrderStyleRefId, ST.StyleRefId, STL.ItemId, SD.ColorRefId, SD.SizeRefId,I.ItemName,C.ColorName,Sz.SizeName,M.UnitName,I.ItemCode


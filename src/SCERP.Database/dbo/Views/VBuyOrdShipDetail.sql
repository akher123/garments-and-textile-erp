
CREATE view [dbo].[VBuyOrdShipDetail]
as
SELECT     dbo.OM_BuyOrdShipDetail.CompId,
                    BuyOrdShip_1.OrderStyleRefId,
						  dbo.OM_BuyOrdShipDetail.OrderShipRefId,
                    dbo.OM_BuyOrdShipDetail.OrderShipDetailId,
                    dbo.OM_BuyOrdShipDetail.ColorRefId,
                     dbo.OM_BuyOrdShipDetail.SizeRefId, 
                     dbo.OM_BuyOrdShipDetail.Quantity,
                     dbo.OM_BuyOrdShipDetail.PAllow,
                     dbo.OM_BuyOrdShipDetail.QuantityP,
                     dbo.OM_BuyOrdShipDetail.ShQty,
                          (SELECT     TOP (1) dbo.OM_BuyOrdStyleColor.ColorRow
                            FROM          dbo.OM_BuyOrdStyleColor INNER JOIN
dbo.OM_BuyOrdShip ON dbo.OM_BuyOrdStyleColor.CompId = dbo.OM_BuyOrdShip.CompID AND dbo.OM_BuyOrdStyleColor.OrderStyleRefId = dbo.OM_BuyOrdShip.OrderStyleRefId
                            WHERE      (dbo.OM_BuyOrdStyleColor.CompId= dbo.OM_BuyOrdShipDetail.CompId) AND (dbo.OM_BuyOrdShip.OrderShipRefId = dbo.OM_BuyOrdShipDetail.OrderShipRefId) AND 
                                                   (dbo.OM_BuyOrdStyleColor.ColorRefId = dbo.OM_BuyOrdShipDetail.ColorRefId)) AS ColorRow,
                          (SELECT     TOP (1) dbo.OM_BuyOrdStyleSize.SizeRow
                            FROM          dbo.OM_BuyOrdStyleSize INNER JOIN
dbo.OM_BuyOrdShip AS BuyOrdShip_2 ON dbo.OM_BuyOrdStyleSize.CompId = BuyOrdShip_2.CompId AND 
dbo.OM_BuyOrdStyleSize.OrderStyleRefId = BuyOrdShip_2.OrderStyleRefId
                            WHERE      (dbo.OM_BuyOrdStyleSize.CompID = dbo.OM_BuyOrdShipDetail.CompID) AND (BuyOrdShip_2.OrderShipRefId = dbo.OM_BuyOrdShipDetail.OrderShipRefId) AND 
                                                   (dbo.OM_BuyOrdStyleSize.SizeRefId = dbo.OM_BuyOrdShipDetail.SizeRefId)) AS SizeRow
FROM         dbo.OM_BuyOrdShipDetail INNER JOIN
dbo.OM_BuyOrdShip AS BuyOrdShip_1 ON dbo.OM_BuyOrdShipDetail.CompID = BuyOrdShip_1.CompID AND dbo.OM_BuyOrdShipDetail.OrderShipRefId = BuyOrdShip_1.OrderShipRefId

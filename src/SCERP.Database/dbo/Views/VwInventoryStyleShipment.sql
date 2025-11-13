

CREATE VIEW [dbo].[VwInventoryStyleShipment]
AS
SELECT V.BuyerName,V.RefNo,V.StyleName,SS.InvoiceNo,SS.InvoiceDate,SS.DepoName,SS.Through,SS.ThroughCellNo,SS.DriverName,SS.DriverCellNo,SS.ShipmentMode,SS.CompId,SS.BuyerRefId,SS.IsApproved,
SS.StyleShipmentId,
SS.Remarks,
SS.StyleShipmentRefId,
SS.ShipDate
 from Inventory_StyleShipment AS SS
INNER JOIN VOM_BuyOrdStyle AS V ON V.OrderStyleRefId=SS.OrderStyleRefId AND V.CompId=V.CompId






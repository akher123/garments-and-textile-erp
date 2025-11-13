
CREATE procedure spShipRatio
@OrderStyleRefId varchar(7)
AS
;WITH RatioCTE AS
(

select 
C.ColorName AS [Color Name] ,
Convert(int,SUM(BOS.QuantityP)) as OrderQty, 
(select SUM(Quantity) 
from PROD_BundleCutting AS BC
where BC.OrderStyleRefId=BOS.OrderStyleRefId and BC.ColorRefId=BOS.ColorRefId  and BC.ComponentRefId='001') AS CuttingQty
,(select SUM(ShipmentQty) from Inventory_StyleShipmentDetail where  OrderStyleRefId=BOS.OrderStyleRefId and ColorRefId=BOS.ColorRefId ) as ShipmentQty
  from  VBuyOrdShipDetail as BOS
inner join OM_Color as C on BOS.ColorRefId=C.ColorRefId and BOS.CompId=C.CompId
where BOS.OrderStyleRefId=@OrderStyleRefId
group by
BOS.OrderStyleRefId,
BOS.CompId,
BOS.ColorRefId,
C.ColorName

)
SELECT T.*, T.ShipmentQty *100/T.OrderQty AS 'ORDER TO SHIP RATIO(%)', T.ShipmentQty *100/T.CuttingQty AS 'CUT TO SHIP RATIO(%)',(100-T.ShipmentQty *100/T.CuttingQty) AS 'REJECTION(%)'  
FROM RatioCTE AS T

UNION ALL
SELECT 'TOTAL'  AS [Color Name],SUM(T.OrderQty) AS [Order Qty] ,SUM(T.CuttingQty) AS [Cutting Qty],SUM(T.ShipmentQty) AS [Shipment Qty], SUM(T.ShipmentQty) *100/SUM(T.OrderQty) AS 'ORDER TO SHIP RATIO(%)', SUM(T.ShipmentQty) *100/SUM(T.CuttingQty) AS 'CUT TO SHIP RATIO(%)',(100-SUM(T.ShipmentQty) *100/SUM(T.CuttingQty)) AS 'REJECTION(%)'  
FROM RatioCTE AS T







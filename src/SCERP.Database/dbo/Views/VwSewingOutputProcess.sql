
CREATE VIEW [dbo].[VwSewingOutputProcess]
AS
Select  
 SO.[SewingOutPutProcessId]
      ,SO.[SewingOutPutProcessRefId]
      ,SO.[LineId]
      ,SO.[BuyerRefId]
      ,SO.[OrderNo]
      ,SO.[OrderStyleRefId]
      ,SO.[ColorRefId]
      ,SO.[PreparedBy]
      ,SO.[CompId]
      ,SO.[HourId]
      ,SO.[Remarks]
      ,CAST( SO.[OutputDate] AS DATE) AS [OutputDate]
      ,SO.[ManPower]
      ,SO.[BatchNo]
      ,SO.[JobNo]
      ,SO.[OrderShipRefId]

,BOS.BuyerName,BOS.RefNo AS OrderName,BOS.StyleName,C.ColorName,M.Name AS MachineName,H.HourName,
(Select SUM(Quantity) OutputQuantity from PROD_SewingOutPutProcessDetail where SewingOutPutProcessId=SO.SewingOutPutProcessId) AS OutputQuantity,
IsNull((Select Top 1 StMv From PROD_StanderdMinValue where OrderStyleRefId=SO.OrderStyleRefId AND CompId=SO.CompId),0)  AS SMV,
CASE WHEN SO.ManPower>0 THEN  
(((Select SUM(Quantity) OutputQuantity from PROD_SewingOutPutProcessDetail where SewingOutPutProcessId=SO.SewingOutPutProcessId) * (Select Top 1 StMv From PROD_StanderdMinValue where OrderStyleRefId=SO.OrderStyleRefId AND CompId=SO.CompId)* 100)/((SO.ManPower)*60)) 
ELSE 0 
END AS Eff
From PROD_SewingOutPutProcess AS SO
INNER JOIN VOM_BuyOrdStyle AS BOS
ON SO.OrderStyleRefId=BOS.OrderStyleRefId AND SO.CompId=BOS.CompId
INNER JOIN OM_Color AS C
ON SO.ColorRefId=C.ColorRefId AND SO.ColorRefId=C.ColorRefId
INNER JOIN Production_Machine AS M
ON SO.LineId=M.MachineId AND SO.CompId=M.CompId
INNER JOIN PROD_Hour AS H
ON SO.HourId=H.HourId AND SO.CompId=H.CompId








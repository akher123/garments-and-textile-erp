



CREATE VIEW [dbo].[VwSewingInputProcess]
AS
Select cast(SI.InputDate as date) as OnlyDate, SI.*,B.BuyerName,BO.RefNo AS OrderName,BOS.StyleName,C.ColorName,M.Name AS MachineName,H.HourName,
(Select SUM(InputQuantity) InputQuantity from PROD_SewingInputProcessDetail where SewingInputProcessId=SI.SewingInputProcessId) AS InputQuantity
From PROD_SewingInputProcess AS SI
INNER JOIN OM_Buyer AS B
ON SI.BuyerRefId=B.BuyerRefId AND SI.CompId=B.CompId
INNER JOIN OM_BuyerOrder AS BO
ON SI.OrderNo=BO.OrderNo AND SI.CompId=BO.CompId
INNER JOIN VOM_BuyOrdStyle AS BOS
ON SI.OrderStyleRefId=BOS.OrderStyleRefId AND SI.CompId=BOS.CompId
INNER JOIN OM_Color AS C
ON SI.ColorRefId=C.ColorRefId AND SI.ColorRefId=C.ColorRefId
INNER JOIN Production_Machine AS M
ON SI.LineId=M.MachineId AND SI.CompId=M.CompId
LEFT JOIN PROD_Hour AS H
ON SI.HourId=H.HourId AND SI.CompId=H.CompId



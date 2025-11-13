
CREATE view [dbo].[VwFinishingProcess]
as
Select 
FP.*,BOS.BuyerName,
BOS.RefNo AS OrderName,
BOS.StyleName,
C.ColorName,
H.HourName,
(Select SUM(InputQuantity)as  InputQuantity from PROD_FinishingProcessDetail where FinishingProcessId=FP.FinishingProcessId) AS InputQuantity
From PROD_FinishingProcess AS FP
INNER JOIN VOM_BuyOrdStyle AS BOS
ON FP.OrderStyleRefId=BOS.OrderStyleRefId AND FP.CompId=BOS.CompId
INNER JOIN OM_Color AS C
ON FP.ColorRefId=C.ColorRefId AND FP.ColorRefId=C.ColorRefId
inner JOIN PROD_Hour AS H
ON FP.HourId=H.HourId AND FP.CompId=H.CompId





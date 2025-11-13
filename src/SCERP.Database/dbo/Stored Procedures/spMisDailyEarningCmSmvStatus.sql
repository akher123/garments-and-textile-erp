CREATE procedure spMisDailyEarningCmSmvStatus
@EarningDate date,
@ToEarningDate date
as

SELECT     M.Name as Line,   VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser,  VOM_BuyOrdStyle.Rate,VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo AS OrderName, VOM_BuyOrdStyle.StyleName, 
                         SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity, ISNULL
                             ((SELECT        TOP (1) StMv
                                 FROM            PROD_StanderdMinValue
                                 WHERE        (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS Smv, ISNULL
                             ((SELECT        TOP (1) CostRate
                                 FROM            OM_CostOrdStyle
                                 WHERE        (CostRefId = '22') AND (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS CmCost
FROM            VOM_BuyOrdStyle INNER JOIN
                         PROD_SewingOutPutProcess ON VOM_BuyOrdStyle.OrderStyleRefId = PROD_SewingOutPutProcess.OrderStyleRefId INNER JOIN
                         PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
						  inner join Production_Machine as M on PROD_SewingOutPutProcess.LineId=M.MachineId
WHERE      Convert(date, PROD_SewingOutPutProcess.OutputDate)>=Convert(date,@EarningDate) AND Convert(date,PROD_SewingOutPutProcess.OutputDate) <= Convert(date,@ToEarningDate)
GROUP BY M.Name, VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo, VOM_BuyOrdStyle.StyleName, VOM_BuyOrdStyle.CompId, VOM_BuyOrdStyle.Rate




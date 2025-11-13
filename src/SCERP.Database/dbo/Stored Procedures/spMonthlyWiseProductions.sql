 CREATE procedure [dbo].[spMonthlyWiseProductions]
 @YearId int ,
 @MonthId int
 as
 
 SELECT        VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo AS OrderName, VOM_BuyOrdStyle.StyleName,   VOM_BuyOrdStyle.Rate,
                         SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity, ISNULL
                             ((SELECT        TOP (1) StMv
                                 FROM            PROD_StanderdMinValue
                                 WHERE        (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS Smv, ISNULL
                             ((SELECT        TOP (1) CostRate
                                 FROM            OM_CostOrdStyle
                                 WHERE        (CostRefId = '22') AND (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS CmCost
FROM            VOM_BuyOrdStyle INNER JOIN
                         PROD_SewingOutPutProcess ON VOM_BuyOrdStyle.OrderStyleRefId = PROD_SewingOutPutProcess.OrderStyleRefId AND VOM_BuyOrdStyle.CompId = PROD_SewingOutPutProcess.CompId INNER JOIN
                         PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
WHERE        (MONTH(PROD_SewingOutPutProcess.OutputDate) =@MonthId) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = @YearId)  
GROUP BY VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo, VOM_BuyOrdStyle.StyleName, VOM_BuyOrdStyle.CompId,VOM_BuyOrdStyle.Rate


--exec [spMonthlyWiseProductions] 2018,1
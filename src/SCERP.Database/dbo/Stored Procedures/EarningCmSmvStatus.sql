
CREATE procedure EarningCmSmvStatus
@EarningDate date
as

--SELECT        VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser,  VOM_BuyOrdStyle.Rate,VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo AS OrderName, VOM_BuyOrdStyle.StyleName, 
--                         SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity, ISNULL
--                             ((SELECT        TOP (1) StMv
--                                 FROM            PROD_StanderdMinValue
--                                 WHERE        (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS Smv, ISNULL
--                             ((SELECT        TOP (1) CostRate
--                                 FROM            OM_CostOrdStyle
--                                 WHERE        (CostRefId = '22') AND (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS CmCost
--FROM            VOM_BuyOrdStyle INNER JOIN
--                         PROD_SewingOutPutProcess ON VOM_BuyOrdStyle.OrderStyleRefId = PROD_SewingOutPutProcess.OrderStyleRefId INNER JOIN
--                         PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
--WHERE        (MONTH(PROD_SewingOutPutProcess.OutputDate) =MONTH(@EarningDate)) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = YEAR(@EarningDate))
--GROUP BY VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo, VOM_BuyOrdStyle.StyleName, VOM_BuyOrdStyle.CompId, VOM_BuyOrdStyle.Rate

SELECT        VOM_BuyOrdStyle.OrderStyleRefId, VOM_BuyOrdStyle.Merchandiser,  VOM_BuyOrdStyle.Rate,VOM_BuyOrdStyle.BuyerName, VOM_BuyOrdStyle.RefNo AS OrderName, VOM_BuyOrdStyle.StyleName, 
                     (
select  SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity 
from    PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId

WHERE  (MONTH(PROD_SewingOutPutProcess.OutputDate) =MONTH(@EarningDate)) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = YEAR(@EarningDate)) and  PROD_SewingOutPutProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId) AS Quantity,
						  ISNULL
                             ((SELECT        TOP (1) StMv
                                 FROM            PROD_StanderdMinValue
                                 WHERE        (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS Smv, ISNULL
                             ((SELECT        TOP (1) CostRate
                                 FROM            OM_CostOrdStyle
                                 WHERE        (CostRefId = '22') AND (OrderStyleRefId = VOM_BuyOrdStyle.OrderStyleRefId) AND (CompId = VOM_BuyOrdStyle.CompId)), 0) AS CmCost
FROM  VOM_BuyOrdStyle 
where OrderStyleRefId in ( select OrderStyleRefId from PROD_SewingOutPutProcess

 WHERE  (MONTH(PROD_SewingOutPutProcess.OutputDate) =MONTH(@EarningDate)) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = YEAR(@EarningDate)) )


 --exec EarningCmSmvStatus '2019-02-10'

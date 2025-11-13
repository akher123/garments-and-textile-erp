
CREATE procedure SpMisDailyEarningBuyerWiseCost
@EarningDate date,
@ToEarningDate date
as

select BuyerName, sum(Quantity) as Qty, sum(Quantity*CmCost/12) as TE, COALESCE( 100*sum(Quantity*CmCost/12)/NULLIF(sum(Quantity*smv),0),0) as  TEM, sum(Quantity*smv) as TM from (select 
 VOM_BuyOrdStyle.BuyerName,
 (
select  SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity 
from    PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId

WHERE      Convert(date, PROD_SewingOutPutProcess.OutputDate)>=Convert(date,@EarningDate) AND Convert(date,PROD_SewingOutPutProcess.OutputDate) <= Convert(date,@ToEarningDate) and  PROD_SewingOutPutProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId) AS Quantity,

ISNULL((select top(1) StMv from PROD_StanderdMinValue
where OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as Smv,
ISNULL((select top(1) CostRate from OM_CostOrdStyle where CostRefId='22' and OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as CmCost
from VOM_BuyOrdStyle 
where OrderStyleRefId in ( select OrderStyleRefId from PROD_SewingOutPutProcess

 WHERE   Convert(date, PROD_SewingOutPutProcess.OutputDate)>=Convert(date,@EarningDate) AND Convert(date,PROD_SewingOutPutProcess.OutputDate) <= Convert(date,@ToEarningDate) )


 ) as A group by BuyerName



CREATE procedure [dbo].[EarningBuyerWise]
@EarningDate date
as 

--select BuyerName, sum(Quantity) as Qty, sum(Quantity*CmCost/12) as TE, COALESCE( 100*sum(Quantity*CmCost/12)/NULLIF(sum(Quantity*smv),0),0) as  TEM, sum(Quantity*smv) as TM from (select 
-- VOM_BuyOrdStyle.BuyerName,
--SUM(PROD_SewingOutPutProcessDetail.Quantity) as Quantity,
--ISNULL((select top(1) StMv from PROD_StanderdMinValue
--where OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as Smv,
--ISNULL((select top(1) CostRate from OM_CostOrdStyle where CostRefId='22' and OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as CmCost
--from VOM_BuyOrdStyle 
--inner join PROD_SewingOutPutProcess on VOM_BuyOrdStyle.OrderStyleRefId=PROD_SewingOutPutProcess.OrderStyleRefId
--inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
--where MONTH(PROD_SewingOutPutProcess.OutputDate)=MONTH(@EarningDate) and Year(PROD_SewingOutPutProcess.OutputDate)=YEAR(@EarningDate)
--group by VOM_BuyOrdStyle.OrderStyleRefId, 
--VOM_BuyOrdStyle.BuyerName,
--VOM_BuyOrdStyle.CompId ) as A group by BuyerName

select BuyerName, sum(Quantity) as Qty, sum(Quantity*CmCost/12) as TE, COALESCE( 100*sum(Quantity*CmCost/12)/NULLIF(sum(Quantity*smv),0),0) as  TEM, sum(Quantity*smv) as TM from 

(select 
 VOM_BuyOrdStyle.BuyerName,
                     (
select  SUM(PROD_SewingOutPutProcessDetail.Quantity) AS Quantity 
from    PROD_SewingOutPutProcess 
inner join PROD_SewingOutPutProcessDetail ON PROD_SewingOutPutProcess.SewingOutPutProcessId = PROD_SewingOutPutProcessDetail.SewingOutPutProcessId

WHERE  (MONTH(PROD_SewingOutPutProcess.OutputDate) =MONTH(@EarningDate)) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = YEAR(@EarningDate)) and  PROD_SewingOutPutProcess.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId) AS Quantity,
ISNULL((select top(1) StMv from PROD_StanderdMinValue
where OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as Smv,
ISNULL((select top(1) CostRate from OM_CostOrdStyle where CostRefId='22' and OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and CompId=VOM_BuyOrdStyle.CompId),0) as CmCost
from VOM_BuyOrdStyle 
where OrderStyleRefId in ( select OrderStyleRefId from PROD_SewingOutPutProcess

 WHERE  (MONTH(PROD_SewingOutPutProcess.OutputDate) =MONTH(@EarningDate)) AND (YEAR(PROD_SewingOutPutProcess.OutputDate) = YEAR(@EarningDate)) )


) as A group by BuyerName

--exec [dbo].[EarningBuyerWise] '2019-02-06'
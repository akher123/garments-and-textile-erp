CREATE procedure spDailyPructivityAnalysis

@filterDate datetime

as
select SUM(NoMachine*600) as Capacity,

(select SUM(TTD.TargetQty*s.StMv)  from PLAN_TargetProduction as TD
inner join PLAN_TargetProductionDetail as TTD on TD.TargetProductionId=TTD.TargetProductionId
inner join PROD_StanderdMinValue as s on TD.OrderStyleRefId=S.OrderStyleRefId
where YEAR(TTD.TargetDate)=YEAR(@filterDate) and MONTH(TTD.TargetDate)=MONTH(@filterDate) and  DAY(TTD.TargetDate)=DAY(@filterDate)) as TargetMin,

(select SUM(SOD.Quantity*S.StMv) from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
inner join PROD_StanderdMinValue as s on SO.OrderStyleRefId=S.OrderStyleRefId
where YEAR(SO.OutputDate)=YEAR(@filterDate) and MONTH(SO.OutputDate)=MONTH(@filterDate) and  DAY(SO.OutputDate)=DAY(@filterDate)) as Production,
0 as HistoricalEff,
0 as ShortExcess
from Production_Machine as M where  M.ProcessorRefId in (select ProcessorRefId from PROD_Processor where ProcessRefId='007')







 


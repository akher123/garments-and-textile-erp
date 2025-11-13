CREATE procedure SpProdSpLine
@MonthId int ,
@YearId int
as
select TGD.TargetDate, SUM(ISNULL(TGD.TargetQty,0)) as TQty,ISNULL((select SUM(SPD.Quantity) from PROD_SewingOutPutProcess  as SP
inner join PROD_SewingOutPutProcessDetail as SPD on SP.SewingOutPutProcessId=SPD.SewingOutPutProcessId
where   SP.OutputDate=TGD.TargetDate    ),0) as PQty
from VwTargetProduction as TG
inner join PLAN_TargetProductionDetail as TGD on TG.TargetProductionId=TGD.TargetProductionId
where MONTH(TGD.TargetDate)=@MonthId and YEAR(TGD.TargetDate)=@YearId
group by TGD.TargetDate




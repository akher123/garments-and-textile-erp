CREATE procedure [dbo].[spMisSewingProductionBoard]
@CurrentDate dateTime,
@CompId varchar(3)
as
--select ISNULL(SUM(TargetQty),0) as TargetQty,ISNULL((select SUM(SPD.Quantity) from [dbo].[PROD_SewingOutPutProcess] as SP
--inner join [dbo].[PROD_SewingOutPutProcessDetail] as SPD on SP.SewingOutPutProcessId=SPD.SewingOutPutProcessId
--where SP.OutputDate=TargetDate),0) as ActualQty  from PLAN_TargetProductionDetail
--where YEAR(TargetDate)=YEAR(@CurrentDate) and  MONTH(TargetDate)=MONTH(@CurrentDate)and  DAY(TargetDate)=DAY(@CurrentDate)
--group by TargetDate

select ISNULL(SUM(PLAN_DailyLineLayout.PlanQty),0) as TargetQty,ISNULL((select SUM(SPD.Quantity) from [dbo].[PROD_SewingOutPutProcess] as SP
inner join [dbo].[PROD_SewingOutPutProcessDetail] as SPD on SP.SewingOutPutProcessId=SPD.SewingOutPutProcessId
where SP.OutputDate= PLAN_DailyLineLayout.OutputDate),0) as ActualQty  from PLAN_DailyLineLayout
where YEAR(PLAN_DailyLineLayout.OutputDate)=YEAR(@CurrentDate) and  MONTH( PLAN_DailyLineLayout.OutputDate)=MONTH(@CurrentDate)and  DAY( PLAN_DailyLineLayout.OutputDate)=DAY(@CurrentDate)
group by  PLAN_DailyLineLayout.OutputDate


--exec spMisSewingProductionBoard '2018-03-28','001'


--select SUM(PlanQty) from PLAN_DailyLineLayout where PLAN_DailyLineLayout.OutputDate='2018-03-28'


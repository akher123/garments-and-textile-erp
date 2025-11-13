
CREATE procedure [dbo].[spMisMontlyBuyerWiseProductionPlan]
as
select BuyerName as [Buyer],

isnull((piv.[202006]),0) as [Jun-20],
isnull((piv.[202007]),0) as [Jul-20],
isnull((piv.[202008]),0) as [Aug-20],
isnull((piv.[202009]),0) as [Sep-20],
isnull((piv.[202010]),0) as [Oct-20],
isnull((piv.[202011]),0) as [Nov-20],
isnull((piv.[202012]),0) as [Dec-20],
isnull((piv.[202101]),0) as [Jan-21], 
isnull((piv.[202102]),0) as [Feb-21],
isnull((piv.[202103]),0) as [Mar-21],
isnull((piv.[202104]),0) as [Apr-21],
isnull((piv.[202105]),0) as [May-21]
from 
(

select B.BuyerName,(YEAR( TP.StartDate) * 100 + MONTH( TP.StartDate)) as xM,SUM(TP.TotalTargetQty) as Qty 
from PLAN_TargetProduction as TP
inner join OM_Buyer as B on TP.BuyerRefId=B.BuyerRefId

group    BY (YEAR(TP.StartDate) * 100 + MONTH(TP.StartDate)), B.BuyerName, B.BuyerName
having SUM(TP.TotalTargetQty)>0
) src 
pivot
(
  SUM(src.Qty)
  for  xM in (   [202006], [202007],[202008],[202009],[202010],[202011],[202012],[202101],[202102],[202103],[202104],[202105])  
) piv

 where  isnull((piv.[202006]),0)+isnull((piv.[202007]),0)+isnull((piv.[202008]),0)+isnull((piv.[202009]),0)+isnull((piv.[202010]),0)+isnull((piv.[202011]),0)+isnull((piv.[202012]),0)+isnull((piv.[202101]),0)+isnull((piv.[202102]),0)+isnull((piv.[202103]),0)+isnull((piv.[202104]),0)+isnull((piv.[202105]),0)>0

 --exec [dbo].[spMisMontlyBuyerWiseProductionPlan]
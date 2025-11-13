
CREATE procedure [dbo].[SpYearlyMerchandiserWiseOrderStatus]
as

select EmpName as [Merchandiser],  

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
  SELECT   B.EmpId, B.EmpName, (YEAR(SH.ShipDate) * 100 + MONTH(SH.ShipDate)) AS xM, Convert(int, SUM(SH.Quantity-ISNULL(SH.DespatchQty,0)))  AS Qty
FROM         OM_BuyOrdStyle AS OS 
inner join OM_BuyOrdShip as SH on OS.OrderStyleRefId=SH.OrderStyleRefId and OS.CompId=SH.CompId
INNER JOIN OM_BuyerOrder AS O ON OS.CompId = O.CompId AND OS.OrderNo = O.OrderNo 

INNER JOIN OM_Merchandiser AS B ON O.CompId = B.CompId AND O.MerchandiserId = B.EmpId
WHERE      (YEAR(SH.ShipDate) >= 2020) and (YEAR(SH.ShipDate) <= 2021) AND (OS.CompId = '001' and O.SCont='N' and OS.ActiveStatus=1)
GROUP BY (YEAR(SH.ShipDate) * 100 + MONTH(SH.ShipDate)), B.EmpName, B.EmpId
) src
pivot
(
  SUM(Qty)
  for  xM in ( [202006], [202007],[202008],[202009],[202010],[202011],[202012],[202101],[202102],[202103],[202104],[202105])  
) piv


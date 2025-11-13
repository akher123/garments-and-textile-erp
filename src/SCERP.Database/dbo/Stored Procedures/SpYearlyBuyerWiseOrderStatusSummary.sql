CREATE procedure [dbo].[SpYearlyBuyerWiseOrderStatusSummary]
as
declare @av integer;

truncate table MIS_ROrderSum;
truncate table MIS_tmpCut;

INSERT INTO MIS_tmpCut ( xMonth, AQty )
SELECT        YEAR(SO.OutputDate) * 100 + MONTH(SO.OutputDate) AS xMonth, SUM(SOD.Quantity) AS AQty
FROM            PROD_SewingOutPutProcess AS SO INNER JOIN
                         PROD_SewingOutPutProcessDetail AS SOD ON SO.SewingOutPutProcessId = SOD.SewingOutPutProcessId
GROUP BY YEAR(SO.OutputDate) * 100 + MONTH(SO.OutputDate);


set @av=40000;


INSERT INTO MIS_ROrderSum (Particular,M1,M2,M3,M4,M5,M6,M7,M8,M9,M10,M11,M12)

select '2:ABILITY' as [Particular] ,isnull((piv.[7]),0) as [Jul], isnull((piv.[8]),0) as [Aug], isnull((piv.[9]),0) as [Sep], isnull((piv.[10]),0) as [Oct], isnull((piv.[11]),0) as [Nov], isnull((piv.[12]),0) as [October] , isnull((piv.[1]),0) as [Jan], isnull((piv.[2]),0) as [Feb], isnull((piv.[3]),0) as [Mar], isnull((piv.[4]),0) as [Apr], isnull((piv.[5]),0) as [May], isnull((piv.[6]),0) as [Jun]
from 
(

SELECT     MONTH(WorkingDate) AS xM, COUNT(*)*  @av AS Qty
FROM         PLAN_WorkingDay
WHERE    (YEAR(WorkingDate)*100+month(WorkingDate) >= 201707) AND (YEAR(WorkingDate)*100+month(WorkingDate) <= 201807) AND (CompId = '001') AND (DayStatus = 1) AND (IsActive = 1)
GROUP BY MONTH(WorkingDate), Year(WorkingDate)
) src
pivot
(
  SUM(Qty)
  for  xM in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]) 
) piv ;


INSERT INTO MIS_ROrderSum (Particular,M1,M2,M3,M4,M5,M6,M7,M8,M9,M10,M11,M12)

select '1:TOTAL' as ['Particular'],

isnull((piv.[7]),0) as [July], isnull((piv.[8]),0) as [August], isnull((piv.[9]),0) as [September], isnull((piv.[10]),0) as [October], isnull((piv.[11]),0) as [November], isnull((piv.[12]),0) as [December], isnull((piv.[1]),0) as [January], isnull((piv.[2]),0) as [February], isnull((piv.[3]),0) as [March], isnull((piv.[4]),0) as [April], isnull((piv.[5]),0) as [May], isnull((piv.[6]),0) as [June]
from 
(
  SELECT     MONTH(SH.ShipDate) AS xM, SUM(SH.Quantity-ISNULL(SH.DespatchQty,0)) AS Qty
FROM         OM_BuyOrdStyle AS OS 
inner join OM_BuyOrdShip as SH on OS.OrderStyleRefId=SH.OrderStyleRefId and OS.CompId=SH.CompId
INNER JOIN OM_BuyerOrder AS O ON OS.CompId = O.CompId AND OS.OrderNo = O.OrderNo
INNER JOIN OM_Buyer AS B ON O.CompId = B.CompId AND O.BuyerRefId = B.BuyerRefId
WHERE     (YEAR(SH.ShipDate)*100+month(SH.ShipDate) >= 201707) AND (YEAR(SH.ShipDate)*100+month(SH.ShipDate) <= 201807) AND (OS.CompId = '001' and   OS.ActiveStatus=1) and O.SCont='N'
GROUP BY MONTH(SH.ShipDate)
) src
pivot
(
  SUM(Qty)
  for  xM in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]) 
) piv;


update MIS_ROrderSum set M1=0-M1, M2=0-M2, M3=0-M3, M4=0-M4, M5=0-M5, M6=0-M6, M7=0-M7, M8=0-M8, M9=0-M9, M10=0-M10, M11=0-M11, M12=0-M12 where Particular like '2%';

insert into MIS_ROrderSum (Particular,M1,M2,M3,M4,M5,M6,M7,M8,M9,M10,M11,M12)
Select '3:Ex/(Short)' as Particular, Sum(M1) as January, sum(M2) as February, sum(M3) as March, sum(M4) as April, sum(M5) as May, sum(M6) as June, sum(M7) as July, sum(M8) AS August, sum(M9) as September, sum(M10) as October, sum(M11) as November, sum(M12) as December  From MIS_ROrderSum as AA ;

update MIS_ROrderSum set  M1=0-M1, M2=0-M2, M3=0-M3, M4=0-M4, M5=0-M5, M6=0-M6, M7=0-M7, M8=0-M8, M9=0-M9, M10=0-M10, M11=0-M11, M12=0-M12 where Particular like '2%';


insert into MIS_ROrderSum (Particular,M1,M2,M3,M4,M5,M6,M7,M8,M9,M10,M11,M12) Values ('4:Production', 0,0,0,0,0,0,0,0,0,0,0,0);

update MIS_ROrderSum set M1=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201707),0) where Particular like '4%';
update MIS_ROrderSum set M2=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201708),0) where Particular like '4%';
update MIS_ROrderSum set M3=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201709),0) where Particular like '4%';
update MIS_ROrderSum set M4=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201710),0) where Particular like '4%';
update MIS_ROrderSum set M5=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201711),0) where Particular like '4%';
update MIS_ROrderSum set M6=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201712),0) where Particular like '4%';
update MIS_ROrderSum set M7=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201801),0) where Particular like '4%';
update MIS_ROrderSum set M8=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201802),0) where Particular like '4%';
update MIS_ROrderSum set M9=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201803),0) where Particular like '4%';
update MIS_ROrderSum set M10=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201804),0) where Particular like '4%';
update MIS_ROrderSum set M11=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201805),0) where Particular like '4%';
update MIS_ROrderSum set M12=isnull(( Select top 1 AQty From MIS_tmpCut where xMonth=201806),0) where Particular like '4%';


select Particular,

M1 as 'Jul-17',M2 as 'Aug-17',M3 as 'Sep-17',M4 as 'Oct-17',M5 as 'Nov-17',M6 as 'Dec-17',M7 as'Jan-18' ,M8 as 'Feb-18', M9 as 'Mar-18', M10 as 'Apr-18' ,M11 as 'May-18',M11 as 'Jun-18' From MIS_ROrderSum  order by particular;



create procedure spMisDailyEarningCost
@EarningDate date,
@ToEarningDate date
as

SELECT  GroupCode, DepartmentName, SUM(NetAmount + ExtraOTAmount + WeekendOTAmount + HolidayOTAmount) AS TTLCOST, ISNULL(Percentage,0) as Percentage,
(SELECT  SUM((NetAmount + ExtraOTAmount + WeekendOTAmount + HolidayOTAmount)*ISNULL(Percentage,0)*.01) AS TTLCOST
FROM            MIS_SalarySummaryDaily
WHERE   GroupCode='D' and   Convert(date, MIS_SalarySummaryDaily.TransactionDate)>=Convert(date,@EarningDate) AND Convert(date,MIS_SalarySummaryDaily.TransactionDate) <= Convert(date,@ToEarningDate)) as DCost
FROM            MIS_SalarySummaryDaily
WHERE      Convert(date, MIS_SalarySummaryDaily.TransactionDate)>=Convert(date,@EarningDate) AND Convert(date,MIS_SalarySummaryDaily.TransactionDate) <= Convert(date,@ToEarningDate)
group by Percentage, GroupCode, DepartmentName
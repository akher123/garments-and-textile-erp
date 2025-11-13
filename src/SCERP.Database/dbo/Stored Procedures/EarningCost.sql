CREATE procedure EarningCost
@EarningDate date
as

SELECT        GroupCode, DepartmentName, NetAmount + ExtraOTAmount + WeekendOTAmount + HolidayOTAmount AS TTLCOST, Percentage
FROM            MIS_SalarySummary
WHERE        (YearCode = YEAR(@EarningDate)) AND (MonthCode =MONTH(@EarningDate))


--exec EarningCost '2019-02-10'
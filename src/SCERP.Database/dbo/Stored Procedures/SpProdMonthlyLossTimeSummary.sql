CREATE procedure [dbo].[SpProdMonthlyLossTimeSummary]
@YearId int ,
@MonthId int 
as

select dtc.CategoryName, dtc.Code,0 as Percentage, isnull((select SUM(DATEDIFF(MINUTE, npt.StartTime,npt.EndTime )*npt.Manpower ) from PROD_NonProductiveTime as npt where Year(npt.EntryDate)=@YearId and MONTH(npt.EntryDate)=@MonthId and  npt.DownTimeCategoryId=dtc.DownTimeCategoryId)/60,0) as LossHr
from PROD_DownTimeCategory as dtc
order by dtc.Code
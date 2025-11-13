
CREATE procedure [dbo].[spMisHourlyAchievement]
@CompId varchar(3),
@OutPutDate datetime
as

BEGIN
Update MIS_HourlyAchievement set QTQty=0

Update MIS_HourlyAchievement set XQty= isnull(( SELECT        ISNULL(SUM(PlanQty), 0) / 10 AS HQty
FROM            PLAN_DailyLineLayout
WHERE        (CompId = @CompId) AND (Convert(date,OutputDate) =CONVERT(date, @OutPutDate)) ),0) where HourId <= 15

Update MIS_HourlyAchievement set QTQty= isnull(( Select Sum(XQty) From MIS_HourlyAchievement as A Where HourId <= MIS_HourlyAchievement.HourId ),0)


Update MIS_HourlyAchievement set XQty=0
Update MIS_HourlyAchievement set XQty= isnull(( SELECT  ISNULL(SUM(SD.Quantity), 0) 
FROM            PROD_SewingOutPutProcess AS S INNER JOIN
                         PROD_SewingOutPutProcessDetail AS SD ON S.SewingOutPutProcessId = SD.SewingOutPutProcessId
WHERE        (S.CompId =@CompId) AND (Convert(date,S.OutputDate) =Convert(date, @OutPutDate)) AND (S.HourId = MIS_HourlyAchievement.HourId) ),0)

Update MIS_HourlyAchievement set QAQty=0

Update MIS_HourlyAchievement set QAQty= isnull(( Select Sum(XQty) From MIS_HourlyAchievement as A Where HourId <= MIS_HourlyAchievement.HourId ),0)


--select HourName,QAQty,QTQty from MIS_HourlyAchievement where CompId=@CompId and HourId <= 15

END
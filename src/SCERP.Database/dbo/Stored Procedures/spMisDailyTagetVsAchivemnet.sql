

CREATE procedure [dbo].[spMisDailyTagetVsAchivemnet]
@CompId varchar(3),
@OutPutDate datetime 
as


select Production_Machine.Name ,
(select ISNULL(SUM(LO.PlanQty),0) from PLAN_DailyLineLayout as LO
where CONVERT(date,LO.OutputDate)=CONVERT(date,@OutPutDate) and LO.LineId=Production_Machine.MachineId AND lo.CompId=Production_Machine.CompId) as TQty,
(

select ISNULL(SUM(SOD.Quantity),0) from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where CONVERT(date,SO.OutputDate) =CONVERT(date,@OutPutDate) and SO.LineId=Production_Machine.MachineId AND SO.CompId=Production_Machine.CompId
) as AQty
from Production_Machine 
where  Production_Machine.IsActive=1 and Production_Machine.ProcessRefId='007' AND Production_Machine.CompId=@CompId
order by Production_Machine.MachineId

--EXEC spMisDailyTagetVsAchivemnet '001','2018-04-04'




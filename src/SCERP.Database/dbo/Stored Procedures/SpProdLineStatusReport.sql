CREATE procedure [dbo].[SpProdLineStatusReport]
@CompId varchar(3),
@OutputDate datetime 

as
select 
(select SUM(Eff)/Count(DISTINCT HourId)  from VwSewingOutputProcess
where CAST(OutputDate AS DATE)= CAST(OP.OutputDate AS DATE) and  LineId=Line.MachineId and CompId=@CompId) as AvgEff,
CAST(OP.OutputDate AS DATE) AS OutputDate,
Line.Name as LineName,
( Select sum(manpower)/count(*) as MP From (
select OP.OutputDate,OP.LineId, Max(OP.ManPower)  as ManPower
from PROD_SewingOutPutProcess as OP where OP.CompId=@CompId
group by OP.OutputDate,OP.LineId ) AA  Where Convert(date,AA.OutputDate)=Convert(Date,OP.OutputDate) and AA.LineId= Line.MachineId ) AS ManPower,

(select  SUM(Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
Where Convert(Date,PROD_SewingOutPutProcess.OutputDate)=Convert(date,OP.OutputDate) and PROD_SewingOutPutProcess.LineId= Line.MachineId and PROD_SewingOutPutProcess.CompId=@CompId) as Quantity,

(select Count(distinct HourId) from PROD_SewingOutPutProcess

where Convert(date,OutputDate)=Convert(date,OP.OutputDate) and LineId=Line.MachineId and CompId=@CompId) as WorkingHour ,
SUM(SMV.StMv)/((select Count(distinct HourId) from PROD_SewingOutPutProcess

where CAST(OutputDate AS DATE)=CAST(OP.OutputDate AS DATE) and LineId=Line.MachineId and CompId=@CompId)) as Smv 
from  PROD_SewingOutPutProcess as OP
inner join Production_Machine as Line on OP.LineId=Line.MachineId and OP.CompId=Line.CompId
left join PROD_StanderdMinValue as SMV on OP.OrderStyleRefId=SMV.OrderStyleRefId and OP.CompId=SMV.CompId
where Convert(date,OP.OutputDate)=convert(date,@OutputDate) and OP.CompId=@CompId
group by Line.Name , Line.MachineId,CAST(OP.OutputDate AS DATE)
order by Line.MachineId


-- exec SpProdLineStatusReport '001','2016-09-19'




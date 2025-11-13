CREATE Procedure [dbo].[spSewingUnitProductionForecasting]
@FilterDate datetime,
@CompId varchar(3)
as
select P.ProcessorName,

(  select Count(*) from PLAN_WorkingDay
where Day(WorkingDate)<=Day(@FilterDate)  and MONTH(WorkingDate)=MONTH(@FilterDate) and  YEAR(WorkingDate)=YEAR(@FilterDate)  and IsActive=1 and DayStatus=1 ) as UTDWD,
(  select Count(*) from PLAN_WorkingDay
where  MONTH(WorkingDate)=MONTH(@FilterDate) and  YEAR(WorkingDate)=YEAR(@FilterDate)  and IsActive=1 and DayStatus=1 ) as TTWD,

(select SUM(SOPD.Quantity) from PROD_SewingOutPutProcessDetail as SOPD 
where (select CAST( OutputDate AS DATE) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)=CAST(@FilterDate AS DATE)) as TDQty ,

(select SUM(SOPD.Quantity) from PROD_SewingOutPutProcessDetail as SOPD 
where (select MONTH(OutputDate) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)=MONTH(@FilterDate)
 and (select day(OutputDate) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)<=day(@FilterDate)
and (select YEAR(OutputDate) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)=YEAR(@FilterDate)
 )as TMQty ,

(select SUM(SOPD.Quantity) from PROD_SewingOutPutProcessDetail as SOPD 
where (select MONTH(OutputDate) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)=(MONTH(@FilterDate)-1)
and (select YEAR(OutputDate) from PROD_SewingOutPutProcess as SO
inner join Production_Machine as M on SO.LineId=M.MachineId
where  SO.SewingOutPutProcessId=SOPD.SewingOutPutProcessId and SO.LineId=M.MachineId and M.ProcessorRefId=P.ProcessorRefId)=(YEAR(@FilterDate))

) as TLMQty 

from PROD_Processor as P
where  P.ProcessRefId='007' and P.CompId=@CompId


--exec spSewingUnitProductionForecasting '2018-05-21','001'






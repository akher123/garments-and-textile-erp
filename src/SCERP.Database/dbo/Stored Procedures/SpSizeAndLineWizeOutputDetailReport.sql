create procedure SpSizeAndLineWizeOutputDetailReport
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CompId varchar(3)
as 
select
(M.MachineRefId+'--'+ M.Name) as Line,

 (select SUM(ManPower) from PROD_SewingOutPutProcess

where OrderStyleRefId=OP.OrderStyleRefId and ColorRefId=OP.ColorRefId and LineId=OP.LineId and OutputDate=OP.OutputDate)/( select count(ManPower) from PROD_SewingOutPutProcess

where OrderStyleRefId=OP.OrderStyleRefId and ColorRefId=OP.ColorRefId and LineId=OP.LineId and OutputDate=OP.OutputDate) as ManPower,
(select count(HourId) from PROD_SewingOutPutProcess

where OrderStyleRefId=OP.OrderStyleRefId and ColorRefId=OP.ColorRefId and LineId=OP.LineId and OutputDate=OP.OutputDate) as RunningHoure,
S.SizeName,
SUM(SOD.Quantity) as Quantity,
OP.OutputDate,
ISNULL((select top(1) StMv from PROD_StanderdMinValue where OrderStyleRefId=OP.OrderStyleRefId and CompId=OP.CompId),0) as Smv
from PROD_SewingOutPutProcess as OP
inner join PROD_SewingOutPutProcessDetail as SOD on OP.SewingOutPutProcessId=SOD.SewingOutPutProcessId
inner join Production_Machine as M on OP.LineId=M.MachineId
inner join PROD_Hour as H on OP.HourId=H.HourId
inner join OM_Size as S on SOD.SizeRefId=S.SizeRefId and OP.CompId=S.CompId
inner join VOM_BuyOrdStyle as BOST on   OP.OrderStyleRefId=BOST.OrderStyleRefId and OP.CompId=BOST.CompId
where OP.OrderStyleRefId=@OrderStyleRefId and OP.ColorRefId=@ColorRefId and OP.CompId=@CompId
group by M.MachineRefId,M.Name,OP.OrderStyleRefId,OP.ColorRefId,OP.LineId,OP.OutputDate,S.SizeName,OP.CompId
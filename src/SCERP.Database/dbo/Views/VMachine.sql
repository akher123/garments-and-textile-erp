
CREATE View [dbo].[VMachine]
as
select M.MachineId,
M.MachineRefId,
M.CompId,M.[Description],
M.EfficiencyPer,
M.IdelPer,M.Name,
M.ProcessorRefId,
M.RatedCapacity,
M.IsActive,
PS.ProcessName,
PS.ProcessRefId,
P.ProcessorName from Production_Machine as M
inner join PROD_Processor as P on M.ProcessorRefId=P.ProcessorRefId and M.CompId=P.CompId
inner join PLAN_Process as PS on P.ProcessRefId=PS.ProcessRefId and P.CompId=PS.CompId

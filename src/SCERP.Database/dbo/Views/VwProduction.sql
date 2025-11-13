create view VwProduction
as 
select 
P.*,(select SUM(PD.Qty) as Qty from PROD_ProductionDetaill as PD
where PD.ProductionId=P.ProductionId and PD.CompId=P.CompId) as Qty,
PR.ProcessorName ,
M.Name as MachineName
from  PROD_Production as P
inner join PROD_Processor as PR on P.ProcessRefId=PR.ProcessorRefId and P.CompId=PR.CompId
inner join Production_Machine as M on P.MachineRefId=M.MachineRefId and P.CompId=M.CompId




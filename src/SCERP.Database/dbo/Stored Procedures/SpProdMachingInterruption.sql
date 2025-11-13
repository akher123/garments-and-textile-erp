
CREATE PROCEDURE [dbo].[SpProdMachingInterruption]
@ProcessRefId varchar(3),
@InterrupDate datetime,
@CompId varchar(3)
AS 
Select M.MachineId,M.Name, 
( SELECT Remarks FROM PROD_MachingInterruption
WHERE MachineId =M.MachineId and convert(date,InterrupDate)=convert(date,@InterrupDate) and ProcessRefId=@ProcessRefId) as Remarks
from Production_Machine AS M Where ProcessorRefId IN ( select PROD_Processor.ProcessorRefId from PROD_Processor
inner join PLAN_Process on PROD_Processor.ProcessRefId=PLAN_Process.ProcessRefId and  PROD_Processor.CompId=PLAN_Process.CompId 
where PLAN_Process.ProcessRefId=@ProcessRefId and PROD_Processor.CompId=@CompId) AND IsActive =1


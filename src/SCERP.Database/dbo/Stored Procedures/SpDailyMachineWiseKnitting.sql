CREATE procedure [dbo].[SpDailyMachineWiseKnitting]
@CompId varchar(3),
@kType char(2),
@RollDate datetime
as

select P.Name,
M.Name as Machine,
SUM(KR.Quantity) as TTRollQty
from PROD_KnittingRoll as KR
inner join PLAN_Program as PG on KR.ProgramId=PG.ProgramId
inner join Party as P on KR.PartyId=P.PartyId
inner join Production_Machine as M on KR.MachineId=M.MachineId
where PG.CID=@kType and convert(date,KR.RollDate)=Convert(date,@RollDate) and KR.CompId=@CompId
group by  P.Name,M.Name






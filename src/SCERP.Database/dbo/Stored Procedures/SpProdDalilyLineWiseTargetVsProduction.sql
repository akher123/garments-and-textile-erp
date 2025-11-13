
CREATE procedure [dbo].[SpProdDalilyLineWiseTargetVsProduction]
@FilterDate datetime,
@CompId varchar(3)
as 

select  CAST(SP.OutputDate AS DATE) AS OutputDate, M.Name as  Line, SUM(ISNULL(SPD.Quantity,0)) as PQty,(select SUM(TGD.TargetQty) from PLAN_TargetProduction as TG
inner join PLAN_TargetProductionDetail as TGD on TG.TargetProductionId=TGD.TargetProductionId
where TGD.TargetDate= SP.OutputDate and TG.LineId=M.MachineId and TG.CompId=@CompId) as TQty from PROD_SewingOutPutProcess  as SP
inner join PROD_SewingOutPutProcessDetail as SPD on SP.SewingOutPutProcessId=SPD.SewingOutPutProcessId
inner join Production_Machine as M on SP.LineId=M.MachineId
where Convert(date,SP.OutputDate)=Convert(date,@FilterDate) and SP.CompId=@CompId
group by M.MachineId, M.Name,OutputDate







CREATE procedure SpPlaningTargetProductionDetail

as
select OST.OrderStyleRefId, BO.RefNo as OrderNo,ST.StyleName, Tg.TotalTargetQty, Pr.ProcessName,M.MachineRefId,M.Name as Line, MIN(TgD.TargetDate) as FromDate , MAX(TgD.TargetDate) as ToDate from  PLAN_TargetProduction as Tg 
inner join PLAN_TargetProductionDetail as TgD on Tg.TargetProductionId=TgD.TargetProductionId and Tg.CompId=TgD.CompId
inner join Production_Machine as M on Tg.LineId=M.MachineId and Tg.CompId=M.CompId
inner join PLAN_Process as Pr on Pg.ProcessRefId=Pr.ProcessRefId and Pg.CompId=Pr.CompId
inner join OM_BuyOrdStyle as OST on Tg.OrderStyleRefId=OST.OrderStyleRefId and Tg.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
inner join VStyle as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
group by OST.OrderStyleRefId, Tg.TotalTargetQty, Pr.ProcessName,M.Name,BO.RefNo ,ST.StyleName,M.MachineRefId



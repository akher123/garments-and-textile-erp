

CREATE view [dbo].[VwAssignedProgram]
as
			select Pg.CompId,
		     Pg.xStatus,
		     OST.Quantity,
			 Pg.ProgramId,
			 Pg.ProgramRefId,
			 Pg.ProcessRefId,
			 Pr.ProcessName,
			 Pg.PrgDate,
			 Pg.ExpDate,
			 Pg.OrderStyleRefId,
			 ST.StyleName,
			 BO.OrderNo,
			 (select ISNULL(sum(TgD.TargetQty),0) from PLAN_TargetProductionDetail as TgD 
			 inner join PLAN_TargetProduction as Tg on TgD.TargetProductionId=Tg.TargetProductionId and TgD.CompId=Tg.CompId
			 where Tg.OrderStyleRefId=Pg.OrderStyleRefId and TgD.CompId=Tg.CompId
			 ) as BookedQty,
			 BO.RefNo from PLAN_Program as Pg
			inner join PLAN_Process as Pr on Pg.ProcessRefId=Pr.ProcessRefId and Pg.CompId=Pr.CompId
			inner join OM_BuyOrdStyle as OST on Pg.OrderStyleRefId=OST.OrderStyleRefId and Pg.CompId=OST.CompId
			inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
			inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
			



CREATE view [dbo].[VwTargetProduction]
as
select 
TP.CompId,
TP.TargetProductionId,
TP.TargetProductionRefId,
BO.BuyerName,
BO.RefNo as OrderName,
BO.StyleName,
TP.BuyerRefId,
TP.OrderNo,
TP.OrderStyleRefId,
TP.TotalTargetQty,
(
select SUM(OPD.Quantity- ISNULL(OPD.QcRejectQty,0)) from PROD_SewingOutPutProcess as OP 
INNER JOIN PROD_SewingOutPutProcessDetail AS OPD ON OP.SewingOutPutProcessId=OPD.SewingOutPutProcessId
where OP.OrderStyleRefId=TP.OrderStyleRefId and OP.LineId=TP.LineId) AS AcheivedQty,
TP.LineId,
Line.Name as Line, 
TP.StartDate,TP.EndDate,TP.Remarks from PLAN_TargetProduction as TP 
inner join Production_Machine as Line on TP.LineId=Line.MachineId
inner join VOM_BuyOrdStyle as BO on TP.OrderStyleRefId=BO.OrderStyleRefId and TP.CompId=BO.CompId

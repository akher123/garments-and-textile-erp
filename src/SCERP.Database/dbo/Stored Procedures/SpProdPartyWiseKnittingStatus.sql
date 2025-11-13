CREATE procedure [dbo].[SpProdPartyWiseKnittingStatus]
@CompId varchar(3),
@ProcessRefId varchar(3),
@PartyId bigint
as
--select   P.Name as Party,PS.ProcessName as Process, SUM(PD.Quantity) as ProgramQty,
--ISNULL((select SUM(PROD_KnittingRoll.Quantity) from PROD_KnittingRoll where PartyId=P.PartyId),0) as RcvQty,
--ISNULL((select SUM(MID.IssueQty) from Inventory_AdvanceMaterialIssue as MI
--inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
--where MI.PartyId=P.PartyId and MI.ProcessRefId=PG.ProcessRefId and MI.CompId=PG.CompId),0.0) as YarnDeliveryQty
--from PLAN_Program as PG
--inner join PLAN_ProgramDetail as PD on PG.ProgramId=PD.ProgramId
--inner join Party as P on PG.PartyId=P.PartyId
--inner join PLAN_Process as PS on PG.ProcessRefId=PS.ProcessRefId and PG.CompId=PS.CompId
--where PD.MType='I' and Pg.ProcessRefId=@ProcessRefId and PG.CompId=@CompId and  (PG.PartyId=@PartyId  or @PartyId='0')
--group by P.Name,PG.CompId,P.PartyId,Pg.ProcessRefId,PS.ProcessName


--select pg.PartyName as Party,'' as ProcessName, SUM(pg.ReqYarnQty) as ProgramQty,SUM(pg.RcvQty) as RcvQty ,SUM(pg.YarnDeliveryQty-pg.YarnReurnQty) as YarnDeliveryQty from VwProgram as pg
--where   Pg.ProcessRefId=@ProcessRefId and PG.CompId=@CompId and  (PG.PartyId=@PartyId  or @PartyId='0')
--group by pg.PartyName,PG.CompId,Pg.ProcessRefId



--select P.Name as Party,SUM(pgd.Quantity)  as ProgramQty,
--ISNULL((select SUM(PROD_KnittingRoll.Quantity) from PROD_KnittingRoll where PartyId=pg.PartyId),0) as RcvQty,
--ISNULL((select SUM(MID.IssueQty) from Inventory_AdvanceMaterialIssue as MI
--inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
--where MI.PartyId=Pg.PartyId and MI.ProcessRefId=@ProcessRefId),0.0)-(select SUM(Inventory_MaterialReceiveAgainstPoDetail.ReceivedQty) from Inventory_MaterialReceiveAgainstPo 
--inner join Inventory_MaterialReceiveAgainstPoDetail on Inventory_MaterialReceiveAgainstPo.MaterialReceiveAgstPoId=Inventory_MaterialReceiveAgainstPoDetail.MaterialReceiveAgstPoId
--where PoNo in (select ProgramRefId from PLAN_Program where PartyId=Pg.PartyId and ProcessRefId=@ProcessRefId)) as YarnDeliveryQty

--from PLAN_Program as pg
--inner join PLAN_ProgramDetail as pgd on pg.ProgramId=pgd.ProgramId 
--inner join Party as p on pg.PartyId=P.PartyId
--where pgd.MType='I' and Pg.ProcessRefId=@ProcessRefId and PG.CompId=@CompId and  (PG.PartyId=@PartyId  or @PartyId='0')
--group by p.Name,pg.PartyId
--order by p.Name,pg.PartyId


select
P.Name as Party,
SUM(pgd.Quantity) AS ProgramQty,
SUM(pgd.Quantity) as ReqYarnQty,
ISNULL((select SUM(MID.IssueQty) from Inventory_AdvanceMaterialIssue as MI
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
where MI.PartyId=PG.PartyId and MI.CompId=PG.CompId),0.0) as YarnDeliveryQty,
ISNULL((select SUM((MRD.ReceivedQty+MRD.RejectedQty)) from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_MaterialReceiveAgainstPoDetail as MRD on MR.MaterialReceiveAgstPoId=MRD.MaterialReceiveAgstPoId
inner join PLAN_Program AS pgs on PoNo=pgs.ProgramRefId
where pgs.PartyId=PG.PartyId),0) as Process,
ISNULL((select SUM(WstYarnQty) from Inventory_FabricReturn as fb
inner join PLAN_Program as pg1 on fb.ProgramId=pg1.ProgramId
where pg1.PartyId=PG.PartyId),0)  as ProcessLoss,
ISNULL((select SUM(Quantity) from PROD_KnittingRoll where PartyId=pg.PartyId ),0) as RcvQty,
ISNULL((select SUM(RollLength) from PROD_KnittingRoll where PartyId= PG.PartyId),0) as QtyInPcs
from PLAN_Program as PG
inner join PLAN_ProgramDetail as pgd on pg.ProgramId=pgd.ProgramId 
inner join Party as P on PG.PartyId=P.PartyId 
where pgd.MType='I' AND PG.ProcessRefId in ('002','009') and PG.CompId=@CompId and  (PG.PartyId=@PartyId  or @PartyId='0')
group by PG.PartyId,P.Name,PG.CompId






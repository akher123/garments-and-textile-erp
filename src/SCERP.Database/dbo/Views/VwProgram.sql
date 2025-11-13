

CREATE view [dbo].[VwProgram]
as
select PG.ProgramId,
B.BuyerName as Buyer,
ORD.BuyerRefId,
ORD.MerchandiserId,
M.EmpName as Merchandiser,
ORD.OrderNo,
ORD.RefNo,
ORD.Quantity,
PG.OrderStyleRefId, 
PG.ProgramRefId,
PG.PrgDate,
PG.ExpDate,
PR.ProcessName,
PG.ProcessRefId ,
PG.ProcessorRefId,
PC.ProcessorName,
P.Name as PartyName,
P.PartyId,
P.[Address] as PartyArddress,
PG.xStatus,
PG.Rmks,
PG.IsLock,
ISNULL((select SUM(Quantity) from PLAN_ProgramDetail where ProgramId=PG.ProgramId and MType='I'),0) as ReqYarnQty,
ISNULL((select SUM(Quantity) from PLAN_ProgramDetail where ProgramId=PG.ProgramId and MType='O'),0) as ReqFabQty,
ISNULL((select SUM(MID.IssueQty) from Inventory_AdvanceMaterialIssue as MI
inner join Inventory_AdvanceMaterialIssueDetail as MID on MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
where MI.ProgramRefId=PG.ProgramRefId and MI.ProcessRefId=PG.ProcessRefId and MI.CompId=PG.CompId),0.0) as YarnDeliveryQty,
(
select SUM((MRD.ReceivedQty+MRD.RejectedQty)) from Inventory_MaterialReceiveAgainstPo as MR

inner join Inventory_MaterialReceiveAgainstPoDetail as MRD on MR.MaterialReceiveAgstPoId=MRD.MaterialReceiveAgstPoId
where PoNo=PG.ProgramRefId) as YarnReurnQty,
--ISNULL((select SUM(FabQty+ReturnYarnQty+WstYarnQty) from Inventory_FabricReturn where ProgramId=PG.ProgramId),0) as RcvQty,
--ISNULL((select SUM(Quantity) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0) as RcvQty,
ISNULL(( select CONVERT(int, SUM(RollLength)) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0) as QtyInPcs,
(
case 
when (PG.ProcessRefId='009' or PG.ProcessRefId='010') then ISNULL(( select SUM(Quantity) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0)
when PG.ProcessRefId='002' then  ISNULL((select SUM(Quantity) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0)
when PG.ProcessRefId='001' then ISNULL((select SUM((MRD.ReceivedQty+MRD.RejectedQty)) from Inventory_MaterialReceiveAgainstPo as MR
inner join Inventory_MaterialReceiveAgainstPoDetail as MRD on MR.MaterialReceiveAgstPoId=MRD.MaterialReceiveAgstPoId
inner join Inventory_AdvanceMaterialIssue as MI on MR.PoNo=MI.IRefId
where MI.ProgramRefId=PG.ProgramRefId),0)
end) as   RcvQty,
PG.Attention,
PG.CGRID,
PG.CID,
PG.ProgramType,
PG.CompId ,
STL.StyleName,
PG.IsApproved,
ISNULL((select top(1)Name from Employee
 where EmployeeId=PG.ApprovedBy ),'Not Approved') as ApproverName ,
 (select top(1)Name from Employee
 where EmployeeId=PG.PrepairedBy ) as PrepareName 
from PLAN_Program as PG
inner join Party as P on PG.PartyId=P.PartyId and PG.CompId=P.CompId
inner  join OM_BuyOrdStyle as ST on PG.OrderStyleRefId=ST.OrderStyleRefId and PG.CompId=ST.CompId
inner join OM_BuyerOrder as ORD on ST.OrderNo=ORD.OrderNo and ST.CompId=ORD.CompId
inner join OM_Style as STL on ST.StyleRefId=STL.StyleRefId and ST.CompId=STL.CompID
inner join OM_Merchandiser as M on ORD.MerchandiserId=M.EmpId and ORD.CompId=M.CompId
inner join OM_Buyer as B on ORD.BuyerRefId=B.BuyerRefId and ORD.CompId=B.CompId
inner join PLAN_Process as PR on PG.ProcessRefId=PR.ProcessRefId and PG.CompId=PR.CompId
inner join PROD_Processor as PC on PG.ProcessorRefId=PC.ProcessorRefId and PG.CompId=PC.CompId







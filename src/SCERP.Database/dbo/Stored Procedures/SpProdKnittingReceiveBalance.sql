CREATE procedure SpProdKnittingReceiveBalance
@ProcessRefId varchar(3),
@OrderStyleRefId varchar(7)=null,
@compId varchar(3)
as
select PG.ProgramRefId,
P.Name as PartyName,
(select top(1) BuyerName from OM_Buyer where BuyerRefId=PG.BuyerRefId and CompId=PG.CompId) as BuyerName,
(select top(1)RefNo from OM_BuyerOrder where OrderNo=PG.OrderNo and CompId=PG.CompId) as OrderName,
(select StyleName from VOMBuyOrdStyle where OrderStyleRefId=PG.OrderStyleRefId and CompId=PG.CompId) as StyleName,
(select SUM(Quantity) from PLAN_ProgramDetail where MType='I' and ProgramId=PG.ProgramId) as ReqQty,
ISNULL((select SUM(IssueQty) from Inventory_AdvanceMaterialIssue
inner join Inventory_AdvanceMaterialIssueDetail on Inventory_AdvanceMaterialIssue.AdvanceMaterialIssueId=Inventory_AdvanceMaterialIssueDetail.AdvanceMaterialIssueId
where Inventory_AdvanceMaterialIssue.ProgramRefId=PG.ProgramRefId and Inventory_AdvanceMaterialIssue.CompId=PG.CompId
),0)as SentQty,
ISNULL((select SUM(Quantity) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0)as TTRollWt,
ISNULL((select count(*) from PROD_KnittingRoll where ProgramId=PG.ProgramId),0)as TTRollQty,
ISNULL((select SUM(FabQty) from Inventory_FabricReturn where ProgramId=PG.ProgramId ),0) as TTGREIGEQTY,
ISNULL((select SUM(ReturnYarnQty) from Inventory_FabricReturn where ProgramId=PG.ProgramId ),0) as TTYarnQtY
from PLAN_Program as PG
inner join   Party as P on PG.PartyId=P.PartyId
where  (PG.OrderStyleRefId=@OrderStyleRefId or @OrderStyleRefId=null) and PG.CompId=@compId

order by P.Name

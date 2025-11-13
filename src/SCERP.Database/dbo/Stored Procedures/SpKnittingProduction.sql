CREATE procedure [dbo].[SpKnittingProduction]
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as
select BST.BuyerName,
BST.RefNo as OrderNo,
BST.StyleName,
PD.ItemName as FabricType,
PD.GSM,
STUFF((SELECT ',' + ItemName
            FROM VProgramDetail where MType='I' and ProgramId=PR.ProgramId 
            FOR XML PATH('')) ,1,1,'') AS Yarn ,
			PD.ColorName,
			PD.SleeveLength as SLength,
			PD.SizeName as McDia,
			PD.FinishSizeName as FDia,
PR.ProgramRefId,
(select top(1) Name from Party 
where PartyId=PR.PartyId ) as Factory,
PR.PrgDate,
PR.ExpDate,
--(select SUM(Quantity) from PLAN_ProgramDetail where MType='I' and ProgramId=PR.ProgramId) as YarnQty,
PD.Quantity as  YarnQty,
ISNULL((select SUM(Quantity) from PROD_KnittingRoll where ProgramId=PR.ProgramId and ColorRefId= PD.ColorRefId),0) as RcvGreyQty,
PD.Quantity as RqGreyQty,
ISNULL((select count(*) from PROD_KnittingRoll where ProgramId=PR.ProgramId and ColorRefId= PD.ColorRefId),0) as RollQty,
ISNULL((select SUM(AMD.IssueQty) from Inventory_AdvanceMaterialIssue as AM 
inner join Inventory_AdvanceMaterialIssueDetail as AMD on AM.AdvanceMaterialIssueId=AMD.AdvanceMaterialIssueId
where AM.ProgramRefId=PR.ProgramRefId and AM.CompId=PR.CompId),0) as YSentQty

from PLAN_Program as PR
inner join VProgramDetail as PD on PR.ProgramId=PD.ProgramId
inner join VOM_BuyOrdStyle as BST on PR.OrderStyleRefId=BST.OrderStyleRefId and PR.CompId=BST.CompId
where  PD.MType='O'    and PR.OrderStyleRefId=@OrderStyleRefId and PR.CompId=@CompId and PR.ProcessRefId in('002','009')







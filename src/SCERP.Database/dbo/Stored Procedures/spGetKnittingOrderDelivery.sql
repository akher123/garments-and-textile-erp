
CREATE procedure [dbo].[spGetKnittingOrderDelivery]
@ProgramId int ,
@GreyIssueId int
as
if @GreyIssueId=0 
select PLAN_Program.ProgramRefId, 
(select top(1) ItemName from Inventory_Item where ItemCode=PROD_KnittingRoll.ItemCode) as ItemName,
PROD_KnittingRoll.ItemCode,
(select ColorName from OM_Color where ColorRefId=PROD_KnittingRoll.ColorRefId) as ColorName,
PROD_KnittingRoll.ColorRefId,
(select SizeName from OM_Size where SizeRefId=PROD_KnittingRoll.SizeRefId) as SizeName,
PROD_KnittingRoll.SizeRefId,
(select SizeName from OM_Size where SizeRefId=PROD_KnittingRoll.FinishSizeRefId) as FSizeName,
PROD_KnittingRoll.FinishSizeRefId,
PROD_KnittingRoll.GSM,
PROD_KnittingRoll.StLength,
ISNULL(SUM(PROD_KnittingRollIssueDetail.RollQty),0) as ProductionQty,
ISNULL(COUNT(PROD_KnittingRollIssueDetail.KnittingRollId),0) as TRollQty,
ISNULL((select SUM(RollQty) from Inventory_GreyIssueDetail where ProgramRegId=PLAN_Program.ProgramRefId and ColorRefId=PROD_KnittingRoll.ColorRefId and ItemCode=PROD_KnittingRoll.ItemCode),0) as IRollQty,
ISNULL((select SUM(Qty) from Inventory_GreyIssueDetail  where ProgramRegId=PLAN_Program.ProgramRefId and  ItemCode=PROD_KnittingRoll.ItemCode and ColorRefId=PROD_KnittingRoll.ColorRefId),0)as IQty,
ISNULL((select SUM(Quantity) from PLAN_ProgramDetail  where ProgramId=PROD_KnittingRoll.ProgramId and  ItemCode=PROD_KnittingRoll.ItemCode and ColorRefId=PROD_KnittingRoll.ColorRefId  and MType='O'),0) as OrdQty
from PROD_KnittingRoll 
inner join PROD_KnittingRollIssueDetail on PROD_KnittingRoll.KnittingRollId=PROD_KnittingRollIssueDetail.KnittingRollId
inner join PLAN_Program on PROD_KnittingRoll.ProgramId=PLAN_Program.ProgramId
where PROD_KnittingRoll.ProgramId=@ProgramId
group by PROD_KnittingRoll.ProgramId,PLAN_Program.ProgramRefId, PROD_KnittingRoll.ItemCode,PROD_KnittingRoll.ColorRefId,PROD_KnittingRoll.SizeRefId,
PROD_KnittingRoll.FinishSizeRefId,PROD_KnittingRoll.GSM,PROD_KnittingRoll.StLength
else 
select PLAN_Program.ProgramRefId, 
(select top(1) ItemName from Inventory_Item where ItemCode=Inventory_GreyIssueDetail.ItemCode) as ItemName,
Inventory_GreyIssueDetail.ItemCode,
(select ColorName from OM_Color where ColorRefId=Inventory_GreyIssueDetail.ColorRefId) as ColorName,
Inventory_GreyIssueDetail.ColorRefId,
(select SizeName from OM_Size where SizeRefId=Inventory_GreyIssueDetail.SizeRefId) as SizeName,
Inventory_GreyIssueDetail.SizeRefId,
(select SizeName from OM_Size where SizeRefId=Inventory_GreyIssueDetail.FinishSizeRefId) as FSizeName,
Inventory_GreyIssueDetail.FinishSizeRefId,
Inventory_GreyIssueDetail.GSM,
Inventory_GreyIssueDetail.StLength,
Inventory_GreyIssueDetail.RollQty,
ISNULL((
select SUM(PROD_KnittingRollIssueDetail.RollQty) from PROD_KnittingRollIssueDetail ,PROD_KnittingRoll 
where PROD_KnittingRollIssueDetail.KnittingRollId=PROD_KnittingRoll.KnittingRollId and PROD_KnittingRoll.ProgramId=PLAN_Program.ProgramId and  ItemCode=Inventory_GreyIssueDetail.ItemCode and ColorRefId=Inventory_GreyIssueDetail.ColorRefId),0) as ProductionQty,
ISNULL((
select COUNT(PROD_KnittingRollIssueDetail.KnittingRollId) from PROD_KnittingRollIssueDetail ,PROD_KnittingRoll 
where PROD_KnittingRollIssueDetail.KnittingRollId=PROD_KnittingRoll.KnittingRollId and PROD_KnittingRoll.ProgramId=PLAN_Program.ProgramId and  ItemCode=Inventory_GreyIssueDetail.ItemCode and ColorRefId=Inventory_GreyIssueDetail.ColorRefId),0) as TRollQty,
ISNULL((select SUM(RollQty) from Inventory_GreyIssueDetail as GD where GD.ProgramRegId=PLAN_Program.ProgramRefId and GD.ColorRefId=Inventory_GreyIssueDetail.ColorRefId and GD.ItemCode=Inventory_GreyIssueDetail.ItemCode),0) as IRollQty,

ISNULL(SUM(Inventory_GreyIssueDetail.Qty),0) as Qty,
ISNULL((select SUM(Qty) from Inventory_GreyIssueDetail as G  where G.ProgramRegId=PLAN_Program.ProgramRefId and  G.ItemCode=Inventory_GreyIssueDetail.ItemCode and G.ColorRefId=Inventory_GreyIssueDetail.ColorRefId),0)as IQty,

ISNULL((select SUM(Quantity) from PLAN_ProgramDetail  where ProgramId=PLAN_Program.ProgramId and  ItemCode=Inventory_GreyIssueDetail.ItemCode and ColorRefId=Inventory_GreyIssueDetail.ColorRefId  and MType='O'),0) as OrdQty

from Inventory_GreyIssue
inner join Inventory_GreyIssueDetail on Inventory_GreyIssue.GreyIssueId=Inventory_GreyIssueDetail.GreyIssueId
inner join PLAN_Program on Inventory_GreyIssueDetail.ProgramRegId=PLAN_Program.ProgramRefId
where Inventory_GreyIssue.GreyIssueId=@GreyIssueId
group by PLAN_Program.ProgramId,PLAN_Program.ProgramRefId, Inventory_GreyIssueDetail.ItemCode,Inventory_GreyIssueDetail.ColorRefId,Inventory_GreyIssueDetail.SizeRefId,
Inventory_GreyIssueDetail.FinishSizeRefId,Inventory_GreyIssueDetail.GSM,Inventory_GreyIssueDetail.StLength,Inventory_GreyIssueDetail.RollQty


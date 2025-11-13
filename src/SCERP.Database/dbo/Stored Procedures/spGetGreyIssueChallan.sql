CREATE procedure [dbo].[spGetGreyIssueChallan]
@GreyIssueId bigint
as
select *, dbo.fnNumberToWords( isnull((  Select SUM(B.Amt) as TAmt From (select  
(select top(1) Rate from PLAN_ProgramDetail where ProgramId=PLAN_Program.ProgramId and ColorRefId=Inventory_GreyIssueDetail.ColorRefId and ItemCode=Inventory_GreyIssueDetail.ItemCode and MType='O') 

* SUM(Inventory_GreyIssueDetail.Qty) as Amt
from Inventory_GreyIssue
inner join Inventory_GreyIssueDetail on Inventory_GreyIssue.GreyIssueId=Inventory_GreyIssueDetail.GreyIssueId
inner join PLAN_Program on Inventory_GreyIssueDetail.ProgramRegId=PLAN_Program.ProgramRefId
where Inventory_GreyIssue.GreyIssueId=@GreyIssueId
group by PLAN_Program.ProgramId,
Inventory_GreyIssueDetail.ItemCode,
Inventory_GreyIssueDetail.ColorRefId) B  ),0)) as AmtToWord from (select PLAN_Program.ProgramRefId, 
 STUFF((SELECT ',' +'Yarn :'+ ItemName+', Count :'+SizeName+ ', Color :'+ColorName+', Lot : '+Remarks
            FROM VProgramDetail where MType='I' and ProgramId=PLAN_Program.ProgramId
            FOR XML PATH('')) ,1,1,'') AS PgYarnName ,
PLAN_Program.CGRID,
(select top(1) Rate from PLAN_ProgramDetail where ProgramId=PLAN_Program.ProgramId and ColorRefId=Inventory_GreyIssueDetail.ColorRefId and ItemCode=Inventory_GreyIssueDetail.ItemCode and MType='O') as Rate,
(select top(1) ItemName from Inventory_Item where ItemCode=Inventory_GreyIssueDetail.ItemCode) as ItemName,
(select ColorName from OM_Color where ColorRefId=Inventory_GreyIssueDetail.ColorRefId) as ColorName,
(select SizeName from OM_Size where SizeRefId=Inventory_GreyIssueDetail.SizeRefId) as SizeName,
(select SizeName from OM_Size where SizeRefId=Inventory_GreyIssueDetail.FinishSizeRefId) as FSizeName,
Inventory_GreyIssueDetail.FinishSizeRefId,
Inventory_GreyIssueDetail.GSM,
Inventory_GreyIssueDetail.StLength,
Inventory_GreyIssueDetail.RollQty,
Inventory_GreyIssue.ChallanDate,
Inventory_GreyIssue.ChallanNo,
Inventory_GreyIssue.Through,
Inventory_GreyIssue.Remarks ,
Inventory_GreyIssue.RefId,
Inventory_GreyIssue.Mobile as Email,
 (select Name  from Party 
where  PartyId=Inventory_GreyIssue.PartyId) as PartyName, 
(select [Address]  from Party 
where PartyId=Inventory_GreyIssue.PartyId) as PartyAddress,
--(select Email  from Party 
--where  PartyId=Inventory_GreyIssue.PartyId) as Email,
(select Phone  from Party 
where  PartyId=Inventory_GreyIssue.PartyId) as Phone,
(select ContactPersonName  from Party 
where PartyId=Inventory_GreyIssue.PartyId) as ContactPersonName,
(select ContactPhone  from Party 
where  PartyId=Inventory_GreyIssue.PartyId) as ContactPhone,
ISNULL((
select SUM(PROD_KnittingRollIssueDetail.RollQty) from PROD_KnittingRollIssueDetail ,PROD_KnittingRoll 
where PROD_KnittingRollIssueDetail.KnittingRollId=PROD_KnittingRoll.KnittingRollId and PROD_KnittingRoll.ProgramId=PLAN_Program.ProgramId and  ItemCode=Inventory_GreyIssueDetail.ItemCode and ColorRefId=Inventory_GreyIssueDetail.ColorRefId),0) as ProductionQty,
SUM(Inventory_GreyIssueDetail.Qty) as Qty,
ISNULL((select SUM(Qty) from Inventory_GreyIssueDetail as G  where G.ProgramRegId=PLAN_Program.ProgramRefId and  G.ItemCode=Inventory_GreyIssueDetail.ItemCode and G.ColorRefId=Inventory_GreyIssueDetail.ColorRefId),0)as IQty,
ISNULL((select SUM(Quantity) from PLAN_ProgramDetail  where ProgramId=PLAN_Program.ProgramId and  ItemCode=Inventory_GreyIssueDetail.ItemCode and ColorRefId=Inventory_GreyIssueDetail.ColorRefId  and MType='O'),0) as OrdQty
,(select Name from Employee where EmployeeId=Inventory_GreyIssue.CreatedBy) as CreatedBy,
(select Name from Employee where EmployeeId=Inventory_GreyIssue.ApprovedBy) as ApprovedBy,
(select Name from Employee where EmployeeId=Inventory_GreyIssue.AuditeBy) as AuditeBy,
Inventory_GreyIssue.VheicalNo as VheicalNo

from Inventory_GreyIssue
inner join Inventory_GreyIssueDetail on Inventory_GreyIssue.GreyIssueId=Inventory_GreyIssueDetail.GreyIssueId
inner join PLAN_Program on Inventory_GreyIssueDetail.ProgramRegId=PLAN_Program.ProgramRefId
where Inventory_GreyIssue.GreyIssueId=@GreyIssueId
group by PLAN_Program.ProgramId,PLAN_Program.ProgramRefId, Inventory_GreyIssueDetail.ItemCode,Inventory_GreyIssueDetail.ColorRefId,Inventory_GreyIssueDetail.SizeRefId,
Inventory_GreyIssueDetail.FinishSizeRefId,Inventory_GreyIssueDetail.GSM,Inventory_GreyIssueDetail.StLength,Inventory_GreyIssueDetail.RollQty,PLAN_Program.CGRID,
Inventory_GreyIssue.ChallanDate,
Inventory_GreyIssue.ChallanNo,
Inventory_GreyIssue.Through,
Inventory_GreyIssue.Remarks,

Inventory_GreyIssue.RefId,
Inventory_GreyIssue.PartyId,
Inventory_GreyIssue.CreatedBy,
Inventory_GreyIssue.ApprovedBy,
Inventory_GreyIssue.VheicalNo,
Inventory_GreyIssue.Mobile,
Inventory_GreyIssue.AuditeBy) A








--exec spGetGreyIssueChallan 14
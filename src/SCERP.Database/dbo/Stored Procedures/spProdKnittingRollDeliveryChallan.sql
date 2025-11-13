CREATE procedure [dbo].[spProdKnittingRollDeliveryChallan]
@KnittingRollIssueId int
as



select (select Name  from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as PartyName, 
(select [Address] from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as PartyAddress,
(select Email  from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as Email,
(select Phone  from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as Phone,
(select ContactPersonName  from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as ContactPersonName,
(select ContactPhone  from Party 
where PartyId=IIF(KRI.ChallanType=3,KR.PartyId,30216) ) as ContactPhone
,KRI.BatchNo,
KRI.IssueRefNo,
KRI.IssueDate,
KRI.ProgramRefId,
(select top(1) PartyName from VwProgram where ProgramRefId=KRI.ProgramRefId and CompId=KRI.CompId) as KPertyName,
(select BuyerName from VOM_BuyOrdStyle where OrderStyleRefId=KRI.OrderStyleRefId and CompId=KRI.CompId) as BuyerName,
(select RefNo from VOM_BuyOrdStyle where OrderStyleRefId=KRI.OrderStyleRefId and CompId=KRI.CompId) as OrderNo,
(select StyleName from VOM_BuyOrdStyle where OrderStyleRefId=KRI.OrderStyleRefId and CompId=KRI.CompId)  as StyleName,
(select SizeName from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId)  as McDia,
(select FinishSizeName from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as FinishDia,
(select ColorName from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as ColorName,
(select ItemName from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as ItemName,
(select GSM from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as GSM,
(select Quantity from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as Quantity,
--(select CharllRollNo from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as CharllRollNo,
(select Rmks from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as CharllRollNo,
(select RollRefNo from VwKnittingRoll where KnittingRollId=KRID.KnittingRollId) as RollRefNo,
ISNULL(KR.StLength,(
select  top(1) PD.SleeveLength from PLAN_ProgramDetail  as PD
inner join PROD_KnittingRoll as KR on PD.ProgramId=KR.ProgramId
where PD.ProgramId=KR.ProgramId and KR.KnittingRollId=KRID.KnittingRollId and  MType='O' and PD.SizeRefId=KR.SizeRefId and PD.ColorRefId=KR.ColorRefId and PD.ItemCode=KR.ItemCode and PD.FinishSizeRefId=KR.FinishSizeRefId)) as StLength,
KRI.Remarks,
(select top(1) ComponentName from OM_Component where ComponentRefId=KR.ComponentRefId and CompId=KR.CompId) as ComponentName,
KR.RollLength   as QtyInPcs,
KR.RollLength  as RollLength,
(select Name from Employee where EmployeeId=KRI.CreatedBy) as PreparedBy

from PROD_KnittingRollIssue as KRI
inner join PROD_KnittingRollIssueDetail as KRID on KRI.KnittingRollIssueId=KRID.KnittingRollIssueId
inner join PROD_KnittingRoll as KR on KRID.KnittingRollId=KR.KnittingRollId
where KRI.KnittingRollIssueId=@KnittingRollIssueId






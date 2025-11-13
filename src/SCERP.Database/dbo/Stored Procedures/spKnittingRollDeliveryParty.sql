
CREATE procedure [dbo].[spKnittingRollDeliveryParty]
@KnittingRollIssueId bigint
as
select (select Name  from Party 
where  PartyId=KR.PartyId) as PartyName, 
(select [Address]  from Party 
where PartyId=KR.PartyId) as PartyAddress,
(select Email  from Party 
where  PartyId=KR.PartyId) as Email,
(select Phone  from Party 
where  PartyId=KR.PartyId) as Phone,
(select ContactPersonName  from Party 
where PartyId=KR.PartyId) as ContactPersonName,
(select ContactPhone  from Party 
where  PartyId=KR.PartyId) as ContactPhone
,KRI.BatchNo,
KRI.IssueRefNo,
KRI.IssueDate,
(select CGRID from PLAN_Program where ProgramId=KR.ProgramId) as StyleName,
'' as PartyOrderNo,
KR.SizeName  as McDia,
KR.FinishSizeName as FinishDia,
KR.ColorName,
KR.ItemName,
KR.GSM,
KRI.Remarks,
COUNT(*) as RollQty,
SUM(KR.Quantity) as Quantity,
(select top(1) Rate from PLAN_ProgramDetail where ProgramId=KR.ProgramId and  MType='O' and SizeRefId=KR.SizeRefId and ItemCode=KR.ItemCode and ColorRefId=KR.ColorRefId) as Rate,
KR.StLength as StLength,
'' as ApprovedBy,
(select Name from Employee where EmployeeId=KRI.CreatedBy) as PreparedBy
from PROD_KnittingRollIssue as KRI
inner join PROD_KnittingRollIssueDetail as KRID on KRI.KnittingRollIssueId=KRID.KnittingRollIssueId
inner join VwKnittingRoll as KR on KRID.KnittingRollId=KR.KnittingRollId
where KRI.KnittingRollIssueId=@KnittingRollIssueId
group by KR.ItemName,
KR.SizeName,
KR.FinishSizeName ,
KR.ColorName,
KR.ColorRefId
,KRI.BatchNo,
KRI.IssueRefNo,
KRI.IssueDate,
KRI.ProgramRefId,
KR.ProgramId,
KRI.CompId,
KRI.KnittingRollIssueId,
KR.GSM,
KR.FinishSizeRefId,
KR.SizeRefId,
KR.ItemCode,
KRI.CreatedBy,
KR.MachineName,
KRI.Remarks,
KR.StLength ,
KR.PartyId




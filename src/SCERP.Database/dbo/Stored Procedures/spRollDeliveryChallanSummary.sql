CREATE procedure [dbo].[spRollDeliveryChallanSummary]
@KnittingRollIssueId bigint
as
select (select Name  from Party 
where PType='d' and PartyId='30216') as PartyName, 
(select [Address]  from Party 
where PType='d' and PartyId='30216') as PartyAddress,
(select Email  from Party 
where PType='d' and PartyId='30216') as Email,
(select Phone  from Party 
where PType='d' and PartyId='30216') as Phone,
(select ContactPersonName  from Party 
where PType='d' and PartyId='30216') as ContactPersonName,
(select ContactPhone  from Party 
where PType='d' and PartyId='30216') as ContactPhone
,KRI.BatchNo,
KRI.IssueRefNo,
KRI.IssueDate,
KRI.ProgramRefId,
(select top(1) PartyName from VwProgram where ProgramRefId=KRI.ProgramRefId and CompId=KRI.CompId) as KPertyName,
(select BuyerName from VOM_BuyOrdStyle where OrderStyleRefId=KRI.OrderStyleRefId and CompId=KRI.CompId) as BuyerName,
(select RefNo from VOM_BuyOrdStyle where OrderStyleRefId=KRI.OrderStyleRefId and CompId=KRI.CompId) as OrderNo,
KR.StyleName as StyleName,
KR.SizeName  as McDia,
KR.FinishSizeName as FinishDia,
KR.ColorName,
KR.ItemName,
KR.MachineName,
KR.GSM,
KRI.Remarks,
KR.Rmks,
(SELECT STUFF((
    SELECT ',' + D.CharllRollNo
    FROM (select PROD_KnittingRoll.CharllRollNo from  PROD_KnittingRollIssueDetail 
inner join PROD_KnittingRoll on PROD_KnittingRollIssueDetail.KnittingRollId=PROD_KnittingRoll.KnittingRollId
where PROD_KnittingRollIssueDetail.KnittingRollIssueId=KRI.KnittingRollIssueId and PROD_KnittingRoll.ItemCode=KR.ItemCode and PROD_KnittingRoll.ColorRefId=KR.ColorRefId and PROD_KnittingRoll.SizeRefId=KR.SizeRefId and PROD_KnittingRoll.FinishSizeRefId=KR.FinishSizeRefId and PROD_KnittingRoll.MachineId=KR.MachineId ) as D
   order by D.CharllRollNo
    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) as RollJoin,
SUM(KR.Quantity) as Quantity,
(select top(1) SleeveLength from VProgramDetail where ProgramId=KR.ProgramId and  MType='O' and SizeRefId=KR.SizeRefId and ColorRefId=KR.ColorRefId and ItemCode=KR.ItemCode and FinishSizeRefId=KR.FinishSizeRefId) as StLength,
'' as ApprovedBy,
(select Name from Employee where EmployeeId=KRI.CreatedBy) as PreparedBy
from PROD_KnittingRollIssue as KRI
inner join PROD_KnittingRollIssueDetail as KRID on KRI.KnittingRollIssueId=KRID.KnittingRollIssueId
inner join VwKnittingRoll as KR on KRID.KnittingRollId=KR.KnittingRollId
where KRI.KnittingRollIssueId=@KnittingRollIssueId
group by KR.ItemName,KR.StyleName ,
KR.SizeName  ,
KR.FinishSizeName ,
KR.ColorName,
KR.ColorRefId
,KRI.BatchNo,
KRI.IssueRefNo,
KRI.IssueDate,
KRI.ProgramRefId,
KR.ProgramId,
KRI.CompId,
KRI.OrderStyleRefId,
KRI.KnittingRollIssueId,
KR.GSM,
KR.MachineId,
KR.FinishSizeRefId,
KR.SizeRefId,
KR.ItemCode,
KRI.CreatedBy,
KR.MachineName,
KRI.Remarks
,KR.Rmks

--exec [spRollDeliveryChallanSummary] 12412





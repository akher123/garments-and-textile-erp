CREATE procedure [dbo].[spGetKnittingRollIssueSummary]
@FromDate date=NULL,
@ToDate date=NULL,
@PartyId bigint
as

select KI.BatchNo ,KR.PartyName,KI.IssueRefNo,KI.IssueDate,KR.ColorName,KR.SizeName,KR.FinishSizeName,KR.ProgramRefId,KR.ItemName,KR.GSM,KR.Buyer,(select RefNo from OM_BuyerOrder where OrderNo=KR.OrderNo) as OrderNo ,KR.StyleName,
SUM(KR.Quantity -ISNULL(KR.RejQuantity,0)) as Qty,
(
select  top(1) PD.SleeveLength from PLAN_ProgramDetail  as PD
inner join PROD_KnittingRoll as KR on PD.ProgramId=KR.ProgramId
where PD.ProgramId=KR.ProgramId and  MType='O' and PD.SizeRefId=KR.SizeRefId and PD.ColorRefId=KR.ColorRefId and PD.ItemCode=KR.ItemCode and PD.FinishSizeRefId=KR.FinishSizeRefId) as StLength
,
Count(*) as RollCont
from PROD_KnittingRollIssue as KI 
inner join PROD_KnittingRollIssueDetail as KID on KI.KnittingRollIssueId=KID.KnittingRollIssueId
right join VwKnittingRoll as KR on KID.KnittingRollId=KR.KnittingRollId
where KI.ChallanType!=3 and  (KR.PartyId=@PartyId or @PartyId=-1)  and ( CAST(KI.IssueDate AS DATE)>=CAST( @FromDate AS DATE) OR @FromDate IS NULL) and (CAST(KI.IssueDate as DATE)<= CAST( @ToDate as DATE) OR @ToDate IS NULL)
group by KI.BatchNo,KR.PartyName,KI.IssueRefNo,KI.IssueDate,KR.ColorName,KR.SizeName,KR.FinishSizeName,KR.ProgramRefId,KR.ItemName,KR.GSM,KR.Buyer,KR.OrderNo,KR.StyleName
order by KI.IssueRefNo,KI.IssueDate



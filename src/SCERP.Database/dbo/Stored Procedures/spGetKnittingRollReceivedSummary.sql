CREATE procedure [dbo].[spGetKnittingRollReceivedSummary]
@FromDate date=NULL,
@ToDate date=NULL,
@PartyId bigint
as

select KR.RollRefNo ,KR.PartyName,KR.CharllRollNo,KR.RollDate,KR.ColorName,KR.SizeName,KR.FinishSizeName,KR.ProgramRefId,KR.ItemName,KR.GSM,KR.Buyer,(select RefNo from OM_BuyerOrder where OrderNo=KR.OrderNo) as OrderNo ,KR.StyleName,
KR.Quantity -ISNULL(KR.RejQuantity,0) as Qty,
(
select  top(1) PD.SleeveLength from PLAN_ProgramDetail  as PD
inner join PROD_KnittingRoll as KR on PD.ProgramId=KR.ProgramId
where PD.ProgramId=KR.ProgramId and  MType='O' and PD.SizeRefId=KR.SizeRefId and PD.ColorRefId=KR.ColorRefId and PD.ItemCode=KR.ItemCode and PD.FinishSizeRefId=KR.FinishSizeRefId) as StLength,
KR.RollLength AS   RollCont
from  VwKnittingRoll as KR 
where  (KR.PartyId=@PartyId or @PartyId=-1)  and ( CAST(KR.RollDate AS DATE)>=CAST( @FromDate AS DATE) OR @FromDate IS NULL) and (CAST(KR.RollDate as DATE)<= CAST( @ToDate as DATE) OR @ToDate IS NULL)

order by KR.RollDate 






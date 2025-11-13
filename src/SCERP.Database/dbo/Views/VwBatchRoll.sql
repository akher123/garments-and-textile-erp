


CREATE view [dbo].[VwBatchRoll]
as
select BR.KnittingRollIssueId,RI.IssueRefNo as RollIssueRefId, BT.OrderStyleRefId, BR.BatchRollId,BR.BatchId,BR.Remarks,BT.BatchNo, KR.KnittingRollId,KR.RollRefNo,KR.CharllRollNo,KR.GSM,KR.SizeName,KR.FinishSizeName,KR.Quantity,KR.ItemName from PROD_BatchRoll as BR
inner join Pro_Batch as BT on BR.BatchId=BT.BatchId
inner join VwKnittingRoll as KR on BR.KnittingRollId=KR.KnittingRollId
inner join PROD_KnittingRollIssue as RI on BR.KnittingRollIssueId=RI.KnittingRollIssueId


CREATE procedure [dbo].[knittingRollissueChallan]
@RefId varchar(20)
as 
select 
I.ItemId,
I.ItemName,
R.GSM,
ISNULL(R.ComponentRefId,'000') AS ComponentRefId,
ISNULL((select ComponentName from OM_Component where ComponentRefId=R.ComponentRefId),'--') AS ComponentName,
R.FinishSizeRefId,
(select SizeName from OM_Size where SizeRefId= R.FinishSizeRefId ) AS FinishSizeName,
R.ColorRefId,
(select ColorName from OM_Color where ColorRefId=R.ColorRefId ) AS ColorName,
R.SizeRefId,
(select SizeName from OM_Size where SizeRefId=R.SizeRefId ) AS SizeName,
SUM(KD.RollQty) AS Qty,

R.ItemCode,
CAST((select count( D.KnittingRollId) from PROD_KnittingRollIssueDetail AS D
INNER JOIN PROD_KnittingRollIssue AS H ON D.KnittingRollIssueId=H.KnittingRollIssueId
INNER JOIN PROD_KnittingRoll AS RS ON D.KnittingRollId=RS.KnittingRollId
where H.IssueRefNo=@RefId  and RS.ColorRefId=R.ColorRefId and RS.FinishSizeRefId=R.FinishSizeRefId and RS.SizeRefId=R.SizeRefId) AS varchar) AS Remarks
 from PROD_KnittingRollIssueDetail AS KD 
INNER JOIN PROD_KnittingRollIssue AS KI ON KD.KnittingRollIssueId=KI.KnittingRollIssueId
INNER JOIN PROD_KnittingRoll AS R ON KD.KnittingRollId=R.KnittingRollId
INNER JOIN Inventory_Item AS I ON R.ItemCode=I.ItemCode
where KI.IssueRefNo=@RefId
group by I.ItemId,
I.ItemName,R.ComponentRefId,R.GSM,R.FinishSizeRefId,R.ColorRefId,R.SizeRefId,R.ItemCode







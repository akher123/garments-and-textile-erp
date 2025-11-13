



CREATE view [dbo].[VwKnittingPayableBill]
as
select
 CAST( KRI.KnittingRollIssueId as bigint) AS Id,
P.Name AS Party,
1 AS BillType,
P.KglId,
(select CAST(AccountCode AS varchar)+'-'+AccountName from Acc_GLAccounts where Id=P.KglId) AS AccountName,
KRI.IssueRefNo AS RefId,KRI.BatchNo AS InvoiceNo,KRI.IssueDate AS InvoiceDate,
 SUM(T.Qty) AS Qty
,CAST(SUM(T.Qty*T.Rate) AS  decimal(10,2)) AS BillAmount
from PROD_KnittingRollIssue AS KRI
INNER JOIN (select ID.KnittingRollIssueId,ID.RollQty AS Qty,
ISNULL((select top(1)Rate from PLAN_ProgramDetail AS PD where PD.MType='O' and PD.ItemCode=KR.ItemCode and PD.ProgramId=KR.ProgramId ),0) AS Rate
from PROD_KnittingRollIssueDetail AS ID
INNER JOIN PROD_KnittingRoll AS KR ON ID.KnittingRollId=KR.KnittingRollId
) AS T ON KRI.KnittingRollIssueId=T.KnittingRollIssueId
INNER JOIN PLAN_Program AS Pg ON KRI.ProgramRefId=Pg.ProgramRefId
INNER JOIN Party AS P ON Pg.PartyId=P.PartyId
where KRI.Posted='N' and Pg.ProcessRefId IN ('009','002') 
group by KRI.KnittingRollIssueId ,  P.Name, KRI.IssueRefNo,KRI.BatchNo,P.KglId,KRI.IssueDate

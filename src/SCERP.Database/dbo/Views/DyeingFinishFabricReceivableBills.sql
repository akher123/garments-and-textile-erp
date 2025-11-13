CREATE VIEW DyeingFinishFabricReceivableBills
AS
select ST.RefId, ST.AccountName,ST.DglId,ST.InvoiceDate,ST.InvoiceNo,ST.Party,SUM(ST.Qty) AS Qty,ROUND(SUM(ST.Qty*ST.Rate)/SUM(ST.Qty),2) AS Rate,SUM(ST.Qty*ST.Rate) AS BillAmount from (
select 
FI.FinishFabIssureRefId AS RefId,
P.Name as Party,
P.DglId,
FI.ChallanNo AS InvoiceNo,
FI.ChallanDate AS InvoiceDate,
(select CAST(AccountCode AS varchar)+'-'+AccountName from Acc_GLAccounts where Id=P.DglId) AS AccountName,
ISNULL((select top(1) Rate from PROD_BatchDetail where BatchDetailId=FID.BatchDetailId ),0) as Rate,
SUM(FID.GreyWt) AS Qty
from  Inventory_FinishFabricIssue as FI 
inner join Party as P on FI.PartyId=P.PartyId
inner join Inventory_FinishFabricIssueDetail as FID on FI.FinishFabIssueId=FID.FinishFabricIssueId
inner join VProBatch as B on FID.BatchId=B.BatchId
group by FID.BatchDetailId, P.Name ,
FI.FinishFabIssureRefId ,
FI.ChallanNo,
FI.ChallanDate,P.DglId) AS ST
group by ST.RefId,ST.AccountName,ST.DglId,ST.InvoiceDate,ST.InvoiceNo,ST.Party
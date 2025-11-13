CREATE PROCEDURE [dbo].[spKnittingReceivableBills] 
	-- Add the parameters for the stored procedure here
@InvoiceDate date,
@SearchString varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select 
G.GreyIssueId AS Id,
(select CAST(AccountCode AS varchar)+'-'+AccountName from Acc_GLAccounts where Id=P.KRglId) AS AccountName,
 P.Name AS Party, G.RefId,G.ChallanNo AS InvoiceNo,G.ChallanDate AS InvoiceDate,SUM(SQ.Qty) AS Qty, CAST(SUM(SQ.Qty*SQ.Rate) AS decimal(18,2)) AS BillAmount,
2 AS BillType,
P.KRglId AS KglId
from Inventory_GreyIssue AS G
INNER JOIN Party AS P ON G.PartyId=P.PartyId
INNER JOIN (
select GI.GreyIssueId,GID.Qty,
ISNULL((select top(1)Rate from PLAN_ProgramDetail AS PD where PD.MType='O' and PD.ItemCode=GID.ItemCode and PD.PrgramRefId=GID.ProgramRegId ),0) AS Rate
from Inventory_GreyIssue AS GI
INNER JOIN Inventory_GreyIssueDetail AS GID ON GI.GreyIssueId=GID.GreyIssueId) AS SQ
ON G.GreyIssueId=SQ.GreyIssueId
where (G.Posted='N' OR G.Posted IS NULL) 
AND ((P.Name LIKE '%'+@SearchString+'%' or @SearchString='-1')
OR (G.RefId LIKE '%'+@SearchString+'%' or @SearchString='-1')
OR (G.ChallanNo LIKE '%'+@SearchString+'%' or @SearchString='-1'))
and YEAR( G.ChallanDate)=2020 and Month(G.ChallanDate)>=2
group by G.GreyIssueId,G.RefId,G.ChallanNo,G.ChallanDate,P.Name,P.KRglId
order by P.Name
END






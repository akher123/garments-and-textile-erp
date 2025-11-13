CREATE procedure [dbo].[PrintEmbBills]
@CompId varchar(3),
@processRefId varchar(3)
AS
select
PR.ProcessRefId,
(CASE WHEN PR.ProcessRefId='005' THEN  'Printing Bill' 
      ELSE 'Embroidery Bill' 
END) AS ProcessTitle,
PR.InvoiceNo as InvoiceNo,
PR.RefNo as RefId,PR.InvoiceDate,
(CASE WHEN PR.ProcessRefId='005' THEN  P.PGlId
      ELSE P.EmGlId 
END)  as GlId,
(CASE WHEN PR.ProcessRefId='005' THEN  (Select top(1) AccountName from Acc_GLAccounts where Id=P.PGlId)
      ELSE (Select top(1) AccountName from Acc_GLAccounts where Id=P.EmGlId)
END)  as AccountName,
P.Name AS Party,
SUM(ISNULL(PRD.InvocieQty,0)-ISNULL(PRD.FabricReject,0)-ISNULL(PRD.ProcessReject,0)) AS Qty,
ISNULL(CS.Rate/12,0) AS Rate ,
CAST(SUM(ISNULL(PRD.InvocieQty,0)-ISNULL(PRD.FabricReject,0)-ISNULL(PRD.ProcessReject,0))*ISNULL(CS.Rate/12,0) AS float)  AS  BillAmount 
from PROD_ProcessReceive AS PR
INNER JOIN PROD_ProcessReceiveDetail AS PRD ON PR.ProcessReceiveId=PRD.ProcessReceiveId
INNER JOIN PROD_CuttingTagSupplier AS CS ON PRD.CuttingTagId=CS.CuttingTagId and CS.PartyId= PR.PartyId
INNER JOIN Party AS P ON PR.PartyId=P.PartyId
where YEAR(PR.InvoiceDate)=2020 and PR.CompId=@CompId and (PR.ProcessRefId=@processRefId or @processRefId IS NULL)
group by PR.RefNo,PR.InvoiceNo,PR.InvoiceDate,PR.PartyId,CS.Rate,PR.ProcessRefId,P.PGlId,P.EmGlId,P.Name







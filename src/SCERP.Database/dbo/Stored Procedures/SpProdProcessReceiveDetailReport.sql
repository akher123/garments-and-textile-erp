CREATE procedure SpProdProcessReceiveDetailReport
@ProcessRefId varchar(3),
@CompId varchar(3),
@OrderStyleRefId varchar(7)
as
select 
p.Name as Party,
PR.RefNo as RcvRefNo,
PR.GateEntrydate,
PR.GateEntryNo,
PR.InvoiceNo,
PR.InvoiceDate,
CB.JobNo,
CB.CuttingBatchRefId,
CB.ColorName,
S.SizeName,
CM.ComponentName as TagName,
PRD.InvocieQty,
PRD.FabricReject,
PRD.ProcessReject,
PRD.ReceivedQty,
CB.BuyerName,
CB.OrderName,
CB.StyleName,
Cmp.Name as CompanyName,
Cmp.FullAddress
from PROD_ProcessReceive AS PR
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and CT.CompId=CM.CompId
inner join OM_Size as S on PRD.SizeRefId=S.SizeRefId and PRD.CompId=S.CompId
inner join VwCuttingBatch as CB on PRD.CuttingBatchId=CB.CuttingBatchId
inner join Party as P on PR.PartyId=P.PartyId 
inner join Company as Cmp on PR.CompId=Cmp.CompanyRefId
where CB.OrderStyleRefId=@OrderStyleRefId and PR.CompId=@CompId and PR.ProcessRefId=@ProcessRefId
order by PR.RefNo
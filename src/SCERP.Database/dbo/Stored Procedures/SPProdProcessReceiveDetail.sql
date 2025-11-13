CREATE procedure [dbo].[SPProdProcessReceiveDetail]
@ProcessReceiveRefId varchar(8),
@CompId varchar(3)
as 
select 
CB.BuyerName,
CB.OrderName,
CB.StyleName,
PR.RefNo,
0 as SLNO,
PR.ProcessReceiveId,
PR.ProcessRefId,
P.Name as Factory,
PR.InvoiceNo,
PR.InvoiceDate,
PR.GateEntryNo,
PR.GateEntrydate,
CB.ColorName,
CB.JobNo,
CB.CuttingBatchRefId,
CM.ComponentName as TagName,
 SZ.SizeName as SizeName,
(select SUM(Quantity) from PROD_ProcessDeliveryDetail  as PDD
inner join PROD_ProcessDelivery as PD on PDD.ProcessDeliveryId=PD.ProcessDeliveryId
where PDD.CuttingBatchId= PRD.CuttingBatchId and PDD.CuttingTagId=PRD.CuttingTagId and PDD.SizeRefId=PRD.SizeRefId and PD.ProcessRefId=PR.ProcessRefId and PDD.CompId=PRD.CompId) as SendingQty,
PRD.ReceivedQty,
PRD.InvocieQty,
PRD.FabricReject,
PRD.ProcessReject,
PR.Remarks,
C.Name as CompanyName,
C.FullAddress,
E.Name as Employee
from PROD_ProcessReceive as PR
inner join PROD_ProcessReceiveDetail as PRD on PR.ProcessReceiveId=PRD.ProcessReceiveId
inner join VwCuttingBatch as CB on PRD.CuttingBatchId=CB.CuttingBatchId and PR.CompId=CB.CompId
inner join OM_Size as SZ on PRD.SizeRefId=SZ.SizeRefId and PR.CompId=SZ.CompId
inner join Party as P on PR.PartyId=P.PartyId
inner join PROD_CuttingTag as CT on PRD.CuttingTagId=CT.CuttingTagId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentId and CT.CompId=CM.CompId
inner join Company as C on PR.CompId=C.CompanyRefId
inner join Employee as E on PR.ReceivedBy=E.EmployeeId
where PR.RefNo=@ProcessReceiveRefId and PR.CompId=@CompId
order by PRD.ProcessReceiveDetailId
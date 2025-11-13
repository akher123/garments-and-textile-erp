



CREATE view [dbo].[VwProdReceiveBalance]
as
select distinct PD.ProcessRefId, 
PD.CompId, 
P.Name as Factory,
PD.PartyId, 
COP.Name as CompanyName,
COP.FullAddress,  
CB.OrderStyleRefId,
CT.IsPrint,
CT.IsEmbroidery, 
CB.BuyerName,
CB.OrderName as OrderNo ,
CB.StyleName, 
CB.JobNo,
CB.CuttingBatchRefId as CutRefNo,
PD.InvoiceNo ,
PD.InvDate,
CM.ComponentName as Part,
CB.ColorName ,
CB.ColorRefId,
(select SUM(Quantity) from  PROD_ProcessDeliveryDetail

where ColorRefId=PDD.ColorRefId and CuttingBatchId=PDD.CuttingBatchId and CuttingTagId=PDD.CuttingTagId and ProcessDeliveryId=PD.ProcessDeliveryId) as Quantity,

(select SUM(Quantity) from  PROD_ProcessDeliveryDetail

where  CuttingBatchId=PDD.CuttingBatchId and CuttingTagId=PDD.CuttingTagId and ProcessDeliveryId=PD.ProcessDeliveryId) as SendingQty,

PR.InvoiceNo as ChallanNo,ISNULL(PR.InvoiceDate,0) as ChallanDate,

ISNULL((select SUM(PRD1.InvocieQty) from PROD_ProcessReceive as PR1

inner join PROD_ProcessReceiveDetail   as PRD1 on PR1.ProcessReceiveId=PRD1.ProcessReceiveId

where PR1.ProcessReceiveId=PR.ProcessReceiveId and PRD1.CuttingBatchId=CB.CuttingBatchId and PRD1.ColorRefId=CB.ColorRefId and PRD1.CuttingTagId=PRD.CuttingTagId  and PR1.ProcessRefId=PR.ProcessRefId),0) as ChalanQty,


ISNULL((select SUM(PRD1.ReceivedQty) from PROD_ProcessReceive as PR1

inner join PROD_ProcessReceiveDetail   as PRD1 on PR1.ProcessReceiveId=PRD1.ProcessReceiveId

where PR1.ProcessReceiveId=PR.ProcessReceiveId and PRD1.CuttingBatchId=CB.CuttingBatchId and PRD1.ColorRefId=CB.ColorRefId and PRD1.CuttingTagId=CT.CuttingTagId and PR1.ProcessRefId=PR.ProcessRefId),0) as ReceiveQty,


ISNULL((select SUM(PRD1.ProcessReject) from PROD_ProcessReceive as PR1

inner join PROD_ProcessReceiveDetail   as PRD1 on PR1.ProcessReceiveId=PRD1.ProcessReceiveId

where PR1.ProcessReceiveId=PR.ProcessReceiveId and PRD1.CuttingBatchId=CB.CuttingBatchId and PRD1.ColorRefId=CB.ColorRefId and PRD1.CuttingTagId=CT.CuttingTagId and PR1.ProcessRefId=PR.ProcessRefId),0) as ProcessRejectQty,


ISNULL((select SUM(PRD1.FabricReject) from PROD_ProcessReceive as PR1
inner join PROD_ProcessReceiveDetail   as PRD1 on PR1.ProcessReceiveId=PRD1.ProcessReceiveId
where PR1.ProcessReceiveId=PR.ProcessReceiveId and  PRD1.CuttingBatchId=CB.CuttingBatchId and PRD1.ColorRefId=PRD.ColorRefId and PRD1.CuttingTagId=CT.CuttingTagId  and PR1.ProcessRefId=PR.ProcessRefId),0) as FabricRejectQty

from PROD_ProcessDelivery as PD
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
inner join VwCuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId
inner join PROD_CuttingTag as CT on PDD.CuttingTagId=CT.CuttingTagId

inner join Company as COP on CB.CompId=COP.CompanyRefId
inner join Party as P on PD.PartyId =P.PartyId
left join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and PD.CompId=CM.CompId
left join PROD_ProcessReceiveDetail as PRD on PDD.CuttingBatchId=PRD.CuttingBatchId and PDD.CuttingTagId=PRD.CuttingTagId 
left join PROD_ProcessReceive as PR on PR.ProcessReceiveId=PRD.ProcessReceiveId and  P.PartyId=PR.PartyId 







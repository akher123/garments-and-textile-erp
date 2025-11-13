CREATE procedure [dbo].[PartyWiseCuttiongProcess]
@ProcessRefId varchar(3),
@CompId varchar(3),
@PartyId bigint,
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4)='0000',
@ComponentRefId varchar(3),
@isPrintable bit=0,
@isEmbroidery bit=0
as 
select 
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName, 
COMP1.ComponentName as SequenceName, 
COMP2.ComponentName as TagName, 
CLR.ColorName , 
CTS.PartyId,
CS.OrderStyleRefId,
CB.CuttingBatchId, 
CB.CuttingBatchRefId,
CB.ApprovalStatus,
CB.CuttingStatus,
CB.JobNo,
CT.IsEmbroidery,
CT.IsPrint,
CT.IsSolid,
CT.CompId,
CT.CuttingTagId,
CLR.ColorRefId,
CS.ComponentRefId ,
ISNULL((select SUM(PROD_BundleCutting.Quantity) from PROD_BundleCutting 
where PROD_BundleCutting.CuttingBatchId=CB.CuttingBatchId and PROD_BundleCutting.CompId=CB.CompId),0)-ISNULL((select  SUM(RejectQty)  from PROD_RejectAdjustment where PROD_RejectAdjustment.CuttingBatchId=CB.CuttingBatchId and PROD_RejectAdjustment.CompId=CB.CompId),0) as FinalCutt,

ISNULL((select sum(PDD.Quantity) from PROD_ProcessDelivery as PD 
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId=@ProcessRefId and PDD.CuttingBatchId=CB.CuttingBatchId and PDD.CuttingTagId=CT.CuttingTagId  and PDD.ColorRefId=CB.ColorRefId  and PD.CompId=CB.CompId),0) as TotalSent

from PROD_CuttingTagSupplier as CTS 
inner join PROD_CuttingTag CT on CTS.CuttingTagId=CT.CuttingTagId and CTS.CompId=CT.CompId 
inner join PROD_CuttingSequence as CS on CT.CuttingSequenceId=CS.CuttingSequenceId and CTS.CompId=CS.CompId and CS.ComponentRefId=@ComponentRefId
inner join dbo.VOM_BuyOrdStyle as BOST on CS.OrderStyleRefId=BOST.OrderStyleRefId and CTS.CompId=BOST.CompId
INNER JOIN dbo.PROD_CuttingBatch as CB on CS.OrderStyleRefId=CB.OrderStyleRefId and  CB.ColorRefId=CS.ColorRefId and CTS.CompId=CB.CompId
INNER JOIN dbo.OM_Component as COMP1 on CB.ComponentRefId=COMP1.ComponentRefId and CS.CompId=COMP1.CompId
INNER JOIN dbo.OM_Component as COMP2 on CT.ComponentRefId=COMP2.ComponentRefId and CT.CompId=COMP2.CompId
INNER JOIN dbo.OM_Color as CLR on CB.ColorRefId=CLR.ColorRefId  and CTS.CompId=CLR.CompId 
where CB.ApprovalStatus='A' and CTS.PartyId=@PartyId and CB.OrderStyleRefId=@OrderStyleRefId and (CB.ColorRefId=@ColorRefId or @ColorRefId ='0000') and CB.ComponentRefId=@ComponentRefId  and CB.CompId=@CompId and (CT.IsEmbroidery=@isEmbroidery or @isEmbroidery=0)  and (CT.IsPrint=@isPrintable or @isPrintable=0)

order by COMP2.ComponentName



--exec [PartyWiseCuttiongProcess] '10092','ST00693','0133','001','001','1','0'



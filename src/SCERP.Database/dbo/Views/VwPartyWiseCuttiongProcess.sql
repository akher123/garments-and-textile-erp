




CREATE view [dbo].[VwPartyWiseCuttiongProcess]
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
CB.JobNo,
CB.CuttingBatchId, 
CB.CuttingBatchRefId,
CB.ApprovalStatus,
CB.CuttingStatus,
CT.IsEmbroidery,
CT.IsPrint,
CT.IsSolid,
CT.CompId,
CT.CuttingTagId,
CLR.ColorRefId,
CS.ComponentRefId 
from PROD_CuttingTagSupplier as CTS 
inner join PROD_CuttingTag CT on CTS.CuttingTagId=CT.CuttingTagId and CTS.CompId=CT.CompId
inner join PROD_CuttingSequence as CS on CT.CuttingSequenceId=CS.CuttingSequenceId and CTS.CompId=CS.CompId
inner join dbo.VOM_BuyOrdStyle as BOST on CS.OrderStyleRefId=BOST.OrderStyleRefId and CTS.CompId=BOST.CompId
INNER JOIN dbo.PROD_CuttingBatch as CB on CS.OrderStyleRefId=CB.OrderStyleRefId and  CB.ColorRefId=CS.ColorRefId and CTS.CompId=CB.CompId
INNER JOIN dbo.OM_Component as COMP1 on CS.ComponentRefId=COMP1.ComponentRefId and CTS.CompId=COMP1.CompId
INNER JOIN dbo.OM_Component as COMP2 on CT.ComponentRefId=COMP2.ComponentRefId and CTS.CompId=COMP2.CompId
INNER JOIN dbo.OM_Color as CLR on CB.ColorRefId=CLR.ColorRefId   and CTS.CompId=CLR.CompId 
where CB.ApprovalStatus='A'




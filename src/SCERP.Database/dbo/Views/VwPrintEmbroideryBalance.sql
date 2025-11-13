


CREATE View [dbo].[VwPrintEmbroideryBalance]
AS
Select CB.CuttingBatchRefId,CB.CompId,C.Name AS CompanyName,C.FullAddress,CB.CuttingDate,CB.JobNo,CB.BuyerRefId,B.BuyerName,CB.OrderNo,BO.RefNo AS OrderName,CB.OrderStyleRefId,OMS.StyleName,CB.ColorRefId,OMC.ColorName,CB.ComponentRefId,
(SELECT ComponentName  from OM_Component where ComponentRefId=CB.ComponentRefId) AS Sequence,
CS.CuttingSequenceId,
CT.CuttingTagId,CT.ComponentRefId AS TagRefId,(SELECT ComponentName  from OM_Component where ComponentRefId=CT.ComponentRefId) AS TagName,CT.IsPrint,CT.IsEmbroidery,
S.PartyId,S.EmblishmentStatus,
P.Name AS PartyName,
(Select SUM(Quantity)-(select SUM(RejectQty) from PROD_RejectAdjustment where CuttingBatchId=CB.CuttingBatchId) as Quantity  from PROD_BundleCutting 

Where CompID=CB.CompId AND CuttingBatchRefId=CB.CuttingBatchRefId AND ComponentRefId=CB.ComponentRefId) AS FinalCut,
IsNull((select SUM(PDD.Quantity)  from PROD_ProcessDeliveryDetail  as PDD
inner join PROD_ProcessDelivery as PD on PDD.ProcessDeliveryId=PD.ProcessDeliveryId
 where PDD.CuttingBatchId= CB.CuttingBatchId and PDD.CuttingTagId=CT.CuttingTagId and  PDD.CompId=CB.CompId and (( PD.ProcessRefId='006' or 2=CT.IsEmbroidery) or (PD.ProcessRefId='005' or 1=CT.IsPrint))),0) AS TotalSend
from PROD_CuttingBatch AS CB
LEFT JOIN PROD_CuttingSequence AS CS
ON CB.CompId=CS.CompId AND CB.OrderStyleRefId=CS.OrderStyleRefId AND CB.ColorRefId=CS.ColorRefId AND CB.ComponentRefId=CS.ComponentRefId
LEFT JOIN PROD_CuttingTag AS CT
ON CS.CuttingSequenceId=CT.CuttingSequenceId AND CS.CompId=CT.CompId
LEFT JOIN PROD_CuttingTagSupplier AS S
ON CT.CuttingTagId=S.CuttingTagId AND CT.CompId=S.CompId
LEFT JOIN Party AS P
ON S.CompId=P.CompId AND S.PartyId=P.PartyId
LEFT JOIN OM_Buyer AS B
ON CB.BuyerRefId=B.BuyerRefId AND CB.CompId=B.CompId
LEFT JOIN OM_Color AS OMC
ON CB.ColorRefId=OMC.ColorRefId AND CB.CompId=OMC.CompId
LEFT JOIN OM_BuyerOrder AS BO 
ON CB.OrderNo=BO.OrderNo AND CB.CompId=BO.CompId
LEFT JOIN OM_BuyOrdStyle AS BOS
ON CB.OrderStyleRefId=BOS.OrderStyleRefId AND CB.CompId=BOS.CompId
LEFT JOIN OM_Style AS OMS
ON BOS.StyleRefId=OMS.StylerefId AND BOS.CompId=OMS.CompID
LEFT JOIN Company AS C
ON CB.CompId=C.CompanyRefId
Where CT.IsPrint=1 OR CT.IsEmbroidery=1





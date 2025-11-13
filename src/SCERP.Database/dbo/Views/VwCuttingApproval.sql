CREATE view [dbo].[VwCuttingApproval]
as
SELECT 
C.CompId,
C.ColorRefId,
C.ComponentRefId,
C.OrderStyleRefId,
C.BuyerRefId,
C.CuttingDate ,
C.CuttingBatchRefId,
C.CuttingBatchId,
OMB.BuyerName,
BO.OrderNo as OrderRefId,
Bo.RefNo AS OrderNo,
OMS.StyleName ,
CLR.ColorName ,
CM.ComponentName ,
C.JobNo,
C.ApprovalStatus,
ISNULL(( select sum(Ratio) from PROD_LayCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0) as MarkerPcs ,
ISNULL((SELECT SUM(Quantity) FROM PROD_RollCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0) as Ply ,
(ISNULL(( select sum(Ratio) from PROD_LayCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0)*ISNULL((SELECT SUM(Quantity) FROM PROD_RollCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0)) AS TotalQty ,
ISNULL((Select sum(RejectQty)from PROD_RejectAdjustment where CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0.0001) as RejectQty
 FROM PROD_CuttingBatch AS C
INNER JOIN OM_Buyer AS OMB  ON C.BuyerRefId=OMB.BuyerRefId 
inner join OM_BuyOrdStyle as OST on C.OrderStyleRefId=OST.OrderStyleRefId and C.OrderNo=OST.OrderNo and C.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
left join OM_Component as CM on C.ComponentRefId=CM.ComponentRefId and CM.CompId=C.CompId
inner join OM_Color as CLR on C.ColorRefId=CLR.ColorRefId and C.CompId=CLR.CompId
INNER JOIN OM_Style AS OMS ON OST.StyleRefId=OMS.StylerefId
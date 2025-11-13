


CREATE VIEW [dbo].[VwCuttingBatch]
AS
SELECT C.BuyerRefId,
C.OrderStyleRefId,
C.CompId,
C.CuttingBatchId,
C.CuttingBatchRefId,
C.CuttingDate,
C.CuttingStatus,
C.MachineId,
C.FIT,
C.JobNo,
C.OrderNo ,
Bo.RefNo as OrderName,
(
 SELECT STUFF((SELECT distinct ',' +  BatchNo
            FROM PROD_BundleCutting  where CuttingBatchId=C.CuttingBatchId
            FOR XML PATH('')) ,1,1,'') ) AS Rmks,
C.StyleRefId,
C.ColorRefId,
OMB.BuyerName,
OMS.StyleName,
OMC.ColorName,
C.ComponentRefId,
CM.ComponentName,
cutTable.Name as TableName,
ISNULL((select SUM(RejectQty) from PROD_RejectAdjustment
where CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0) AS RejectQty,
ISNULL(( select sum(Ratio) from PROD_LayCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0) as MarkerPcs ,
ISNULL((SELECT SUM(Quantity) FROM PROD_RollCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId),0) as PLY ,
((select count(RollNo) from PROD_RollCutting where CuttingBatchRefId=C.CuttingBatchRefId  and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId)*ISNULL(( select sum(Ratio) from PROD_LayCutting where CuttingBatchRefId=C.CuttingBatchRefId and CompId=C.CompId),0)) as BundleQuantity,
ISNULL(((select sum(Ratio) from PROD_LayCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId)*(SELECT SUM(Quantity) FROM PROD_RollCutting where CuttingBatchRefId=C.CuttingBatchRefId and CuttingBatchId=C.CuttingBatchId and CompId=C.CompId)),0) AS Total  FROM PROD_CuttingBatch AS C
INNER JOIN OM_Buyer AS OMB  ON C.BuyerRefId=OMB.BuyerRefId 
inner join OM_BuyOrdStyle as OST on C.OrderStyleRefId=OST.OrderStyleRefId and C.OrderNo=OST.OrderNo and C.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
left join OM_Component as CM on C.ComponentRefId=CM.ComponentRefId and CM.CompId=C.CompId
left join Production_Machine as cutTable on C.MachineId=cutTable.MachineId
INNER JOIN OM_Style AS OMS ON OST.StyleRefId=OMS.StylerefId and OST.CompId=OMS.CompID
INNER JOIN OM_Color AS OMC ON C.ColorRefId=OMC.ColorRefId AND C.CompId=OMC.CompId










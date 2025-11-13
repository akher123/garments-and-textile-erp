CREATE procedure SpProdBundel
(
@CuttingBatchRefId VARCHAR(6),
@CompId VARCHAR(3)
)

as

Select B.RollName,BCS.XSCName,B.BatchNo,ROW_NUMBER() OVER(ORDER BY B.BundleCuttingId ) as BundleNo,B.BundleStart,B.BundleEnd,(B.BundleEnd-B.BundleStart) AS Quantity,B.SSL+'-'+ S.SizeName  as SizeName From PROD_BundleCutting AS B 
INNER JOIN OM_Size AS S
ON B.SizeRefId=S.SizeRefId AND B.CompId=S.CompId
INNER JOIN PROD_CuttingBatch AS CB
ON B.CuttingBatchId=CB.CuttingBatchId AND B.CompId=CB.CompId
inner join PROD_BundleCuttingSeqChar as BCS on B.XSC=BCS.XSC 
where B.CuttingBatchRefId=@CuttingBatchRefId AND B.CompId=@CompId

order by B.SSL
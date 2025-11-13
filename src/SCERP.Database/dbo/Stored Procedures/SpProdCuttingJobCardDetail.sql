CREATE procedure [dbo].[SpProdCuttingJobCardDetail]
 @OrderStyleRefId varchar(7),
 @ComponentRefId varchar(3),
 @ColorRefId varchar(4),
 @CompId varchar(3)

as
select  CB.JobNo,CB.CuttingBatchRefId,CB.CuttingDate, SUM(BC.Quantity) as CuttingQty,SZ.SizeName,
ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
where RA.CuttingBatchId=CB.CuttingBatchId and RA.SizeRefId=BC.SizeRefId and RA.CompId=CB.CompId  ),0) as RejectQty,
(STUFF((
         SELECT distinct ', ' +  BatchNo 
            FROM PROD_BundleCutting where CuttingBatchId=CB.CuttingBatchId
            FOR XML PATH('')
         ), 1, 1, '')) as BatchStirng
from PROD_CuttingBatch as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId and CB.CompId=BC.CompId
inner join OM_Size as SZ on BC.SizeRefId=SZ.SizeRefId and BC.CompId=SZ.CompId
  where CB.OrderStyleRefId=@OrderStyleRefId and CB.ComponentRefId=@ComponentRefId and BC.ColorRefId=@ColorRefId and CB.CompId=@CompId 
group by CB.JobNo,CB.CuttingBatchRefId,CB.CuttingDate,  SZ.SizeName ,BC.SSL,CB.CuttingBatchId,BC.SizeRefId,CB.CompId
order by  CB.CuttingBatchRefId


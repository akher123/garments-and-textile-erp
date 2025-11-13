create procedure SpProdJobWiseRejectAdjusment
       @CuttingBatchId bigint,
	   @CompId varchar(3)
as
 select BC.SizeRefId,SZ.SizeName,BC.[SSL],SUM(BC.Quantity) as Quantity,
 ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
  where RA.CuttingBatchId=BC.CuttingBatchId and RA.SizeRefId=BC.SizeRefId and RA.CompId=BC.CompId  ),0) as RejectQty
 from PROD_BundleCutting as BC
inner join OM_Size as SZ on BC.SizeRefId=SZ.SizeRefId and BC.CompId=SZ.CompId
where BC.CuttingBatchId=  @CuttingBatchId and BC.CompId=@CompId 
group by BC.SizeRefId,SZ.SizeName,BC.[SSL],BC.CompId  ,BC.CuttingBatchId
order by BC.[SSL]

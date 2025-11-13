create procedure [dbo].[SpProdJobWiseRejectReplacement]
       @CuttingBatchId bigint,
	   @CompId varchar(3)
as
 select BC.SizeRefId,SZ.SizeName,BC.[SSL],SUM(BC.Quantity) as Quantity,
 ISNULL((select sum(RR.RejectQty)  from PROD_RejectReplacement as RR
  where RR.CuttingBatchId=BC.CuttingBatchId and RR.SizeRefId=BC.SizeRefId and RR.CompId=BC.CompId  ),0) as RejectQty
 from PROD_BundleCutting as BC
inner join OM_Size as SZ on BC.SizeRefId=SZ.SizeRefId and BC.CompId=SZ.CompId
where BC.CuttingBatchId=  @CuttingBatchId and BC.CompId=@CompId 
group by BC.SizeRefId,SZ.SizeName,BC.[SSL],BC.CompId  ,BC.CuttingBatchId
order by BC.[SSL]

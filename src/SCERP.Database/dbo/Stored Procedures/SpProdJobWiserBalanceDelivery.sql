 CREATE procedure [dbo].[SpProdJobWiserBalanceDelivery]
       @CuttingBatchId bigint,
	   @CompId varchar(3),
	   @ProcessRefId varchar(3),
	   @CuttingTagId bigint
as
 select BC.SizeRefId,SZ.SizeName,BC.[SSL],SUM(BC.Quantity) as Quantity,
 ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
  where RA.CuttingBatchId=BC.CuttingBatchId and RA.SizeRefId=BC.SizeRefId and RA.CompId=BC.CompId  ),0) as RejectQty,
 

ISNULL((select sum(PDD.Quantity) from PROD_ProcessDelivery as PD 
inner join PROD_ProcessDeliveryDetail as PDD on PD.ProcessDeliveryId=PDD.ProcessDeliveryId
where PD.ProcessRefId=@ProcessRefId and PDD.SizeRefId=BC.SizeRefId and PDD.CuttingBatchId=BC.CuttingBatchId and PDD.CuttingTagId=@CuttingTagId  and PDD.ColorRefId=BC.ColorRefId  and PD.CompId=BC.CompId),0) as TotalDelivery

 from PROD_BundleCutting as BC
inner join OM_Size as SZ on BC.SizeRefId=SZ.SizeRefId and BC.CompId=SZ.CompId
where BC.CuttingBatchId= @CuttingBatchId and BC.CompId=@CompId 
group by BC.SizeRefId,SZ.SizeName,BC.[SSL],BC.CompId,BC.ColorRefId  ,BC.CuttingBatchId,BC.ComponentRefId
order by BC.[SSL]







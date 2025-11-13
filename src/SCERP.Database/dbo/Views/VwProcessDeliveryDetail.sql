create view VwProcessDeliveryDetail
as

select PDD.*,CLR.ColorName,SZ.SizeName,CB.CuttingBatchRefId from PROD_ProcessDeliveryDetail PDD

inner join PROD_CuttingBatch as CB on PDD.CuttingBatchId=CB.CuttingBatchId

inner join OM_Size as SZ on PDD.SizeRefId=SZ.SizeRefId and PDD.CompId=SZ.CompId

inner join OM_Color as CLR on PDD.ColorRefId=CLR.ColorRefId and PDD.CompId=CLR.CompId
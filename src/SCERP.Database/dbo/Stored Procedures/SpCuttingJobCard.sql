CREATE procedure [dbo].[SpCuttingJobCard]
     @OrderStyleRefId varchar(7),
     @ColorRefId varchar(4),
	 @ComponentRefId varchar(3),
     @CompId varchar(3),
	 @OrderShipRefId varchar(8)
AS

select SD.SizeRow 
,S.SizeName , SUM(SD.Quantity) as Quantity

,
ISNULL(( SELECT        ISNULL(SUM(Quantity), 0) AS SQty
FROM            PROD_BundleCutting as B
inner join PROD_CuttingBatch as CB on B.CuttingBatchId=CB.CuttingBatchId
WHERE       B.ComponentRefId=@ComponentRefId and (B.OrderStyleRefId = SD.OrderStyleRefId) AND (B.CompId = SD.CompId) AND (B.ColorRefId = SD.ColorRefId)
And B.SizeRefId=SD.SizeRefId and (CB.StyleRefId=@OrderShipRefId or @OrderShipRefId='-1')),0) as CuttingQuantity,

0 as Ratio,
SD.SizeRefId ,
SD.OrderStyleRefId 
from VBuyOrdShipDetail as SD
inner join OM_Size as S on SD.SizeRefId=S.SizeRefId and SD.CompId=S.CompId
where SD.OrderStyleRefId=@OrderStyleRefId and SD.ColorRefId= @ColorRefId and SD.CompId=@CompId and (SD.OrderShipRefId=@OrderShipRefId or @OrderShipRefId='-1')

group by S.SizeName,SD.SizeRefId,SD.SizeRow, SD.OrderStyleRefId  , SD.ColorRefId,SD.CompId,SD.SizeRow
order by SD.SizeRow


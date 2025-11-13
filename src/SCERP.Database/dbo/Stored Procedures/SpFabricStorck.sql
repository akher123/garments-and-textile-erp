CREATE procedure [dbo].[SpFabricStorck]
@AsOnDate datetime,
@PartyId bigint
as

select 
B.BatchDate,
BatchDetailId,
B.PartyName,
B.BtRefNo,
B.BatchNo,
ISNULL(B.BuyerName,'---') as BuyerName,
ISNULL(B.OrderName,B.Gsm) as OrderName,
B.StyleName,
B.GColorName,
'Kg' as UnitName,
(select  top(1)ItemName from VwProdBatchDetail where BatchDetailId=FSD.BatchDetailId) as FabricType,
(select  top(1)ComponentName from VwProdBatchDetail where BatchDetailId=FSD.BatchDetailId) as ComponentName,
(select  top(1)Gsm from PROD_BatchDetail where BatchDetailId=FSD.BatchDetailId) as GSM,
SUM(FSD.GreyWt) as GreyWt,
SUM(FSD.RcvQty)  as RQty,
(select ISNULL(SUM(FabQty),0) from Inventory_FinishFabricIssueDetail where BatchDetailId=FSD.BatchDetailId and BatchId=FSD.BatchId) AS IQty
 from Inventory_FinishFabStore as FS
inner join Inventory_FinishFabDetailStore as FSD on FS.FinishFabStoreId=FSD.FinishFabStoreId
inner join VProBatch as B on FSD.BatchId=B.BatchId
where FS.CompId='001' and  B.BatchDate<=@AsOnDate and (b.PartyId=@PartyId or @PartyId=-1)
group by
BatchDate,
 BatchDetailId,FSD.BatchId,
B.PartyName,
B.BtRefNo,
B.BatchNo,
B.BuyerName,
B.OrderName,B.Gsm,
B.StyleName,
B.GColorName
HAVING ROUND((SUM(FSD.RcvQty)-(select ISNULL(SUM(FabQty),0) from Inventory_FinishFabricIssueDetail where BatchDetailId=FSD.BatchDetailId and BatchId=FSD.BatchId)),1) <>0












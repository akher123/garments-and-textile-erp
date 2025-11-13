CREATE procedure SpProdMonthlyProductionStatus
@YarId int ,
@Month int,
@CompId varchar(3)
as 

select PROD_CuttingBatch.CuttingDate,SUM(PROD_BundleCutting.Quantity) as Quantity from PROD_CuttingBatch
inner join PROD_BundleCutting  on PROD_CuttingBatch.CuttingBatchId=PROD_BundleCutting.CuttingBatchId
where MONTH(PROD_CuttingBatch.CuttingDate)=@Month and YEAR(PROD_CuttingBatch.CuttingDate)=@YarId and PROD_CuttingBatch.CompId=@CompId  and PROD_CuttingBatch.ComponentRefId='001'
group by PROD_CuttingBatch.CuttingDate

order by PROD_CuttingBatch.CuttingDate
CREATE  procedure [dbo].[SpProdCuttingProductionSummaryReport]
			@BuyerRefId varchar(3) = NULL,
			@OrderNo varchar(12) = NULL,
			@OrderStyleRefId varchar(7) = NULL,
			@ComponentRefId varchar(3) = NULL,
			@CompId varchar(3),
			@Date date
AS
SELECT       
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName ,
CLR.ColorName,
CLR.ColorRefId,
SUM(SHD.Quantity) AS Quantity, 
ISNULL((SELECT sum(BDC.Quantity) FROM PROD_BundleCutting as BDC
inner join PROD_CuttingBatch as CTB on BDC.CuttingBatchId=CTB.CuttingBatchId
WHERE Convert(date,CTB.CuttingDate)<=Convert(date,@Date) and  BDC.OrderStyleRefId=SH.OrderStyleRefId and BDC.ColorRefId=SHD.ColorRefId and BDC.ComponentRefId=@ComponentRefId  and BDC.CompId=SHD.CompId),0) as CuttingQuantity,

(ISNULL((select SUM(BD1.Quantity) FROM PROD_BundleCutting as BD1
inner join PROD_CuttingBatch AS CB1 ON BD1.CuttingBatchId=CB1.CuttingBatchId and BD1.CompId=CB1.CompId
where Convert(date,CB1.CuttingDate)=Convert(date,@Date) and  CB1.ComponentRefId=@ComponentRefId  and CB1.OrderStyleRefId=SH.OrderStyleRefId and BD1.ColorRefId=SHD.ColorRefId 
group by BD1.OrderStyleRefId,BD1.ColorRefId),0)) as TodayCuttingQty,

(ISNULL((select sum(RA.RejectQty) FROM PROD_RejectAdjustment AS RA
inner join PROD_CuttingBatch AS ICB ON RA.CuttingBatchId=ICB.CuttingBatchId and RA.CompId=ICB.CompId
where  Convert(date,ICB.CuttingDate)<=Convert(date,@Date) and  ICB.ColorRefId=SHD.ColorRefId and ICB.ComponentRefId=@ComponentRefId and ICB.OrderStyleRefId=SH.OrderStyleRefId and RA.CompId=SHD.CompId),0) ) as RejectQty
,'' as ComponentName
,COM.Name as CompanyName,
COM.FullAddress
FROM OM_BuyOrdShipDetail AS SHD 
INNER JOIN OM_BuyOrdShip AS SH ON SHD.CompId = SH.CompId AND SHD.OrderShipRefId = SH.OrderShipRefId
inner join OM_Color as CLR on SHD.ColorRefId=CLR.ColorRefId and SHD.CompId=CLR.CompId
inner join VOM_BuyOrdStyle as BOST on SH.OrderStyleRefId=BOST.OrderStyleRefId and SH.CompId=BOST.CompId
inner join PROD_CuttingProcessStyleActive as SINCUT on BOST.OrderStyleRefId=SINCUT.OrderStyleRefId and BOST.CompId=SINCUT.CompId
left join Company as COM on SHD.CompId=COM.CompanyRefId

WHERE (BOST.BuyerRefId=@BuyerRefId OR @BuyerRefId IS NULL)  and (BOST.OrderNo=@OrderNo  OR  @OrderNo IS NULL) and (BOST.OrderStyleRefId=@OrderStyleRefId  OR @OrderStyleRefId  IS NULL) and Convert(date,SINCUT.StartDate)<=Convert(date,@Date) and (SINCUT.EndDate is null or Convert(date,SINCUT.EndDate)>=Convert(date,@Date))  and BOST.CompId=@CompId 
group by CLR.ColorRefId, COM.Name ,COM.FullAddress, BOST.BuyerName,CLR.ColorName,BOST.RefNo,BOST.StyleName,SHD.CompId,SHD.ColorRefId,SH.OrderStyleRefId



--select * from PROD_CuttingProcessStyleActive where OrderStyleRefId='ST01791'
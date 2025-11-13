CREATE procedure [dbo].[SpProdCuttingProductionDetail]
	        @BuyerRefId varchar(3) = NULL,
			@OrderNo varchar(12) = NULL,
			@OrderStyleRefId varchar(7) = NULL,
			@ComponentRefId varchar(3) = NULL,
			@CompId varchar(3),
			@Date date
as

SELECT       
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName ,
CLR.ColorName,
SZ.SizeName,
SUM(SHD.Quantity) AS Quantity, 
ISNULL((SELECT sum(Quantity) FROM PROD_BundleCutting WHERE OrderStyleRefId=SH.OrderStyleRefId and ColorRefId=SHD.ColorRefId and SizeRefId=SHD.SizeRefId and ComponentRefId=IIF(@ComponentRefId is null,'001',@ComponentRefId) and CompId=SHD.CompId),0) as CuttingQuantity,
(ISNULL((select SUM(BD1.Quantity) FROM PROD_BundleCutting as BD1
inner join PROD_CuttingBatch AS CB1 ON BD1.CuttingBatchId=CB1.CuttingBatchId and BD1.CompId=CB1.CompId
where  CB1.CuttingDate=@Date and CB1.ComponentRefId=IIF(@ComponentRefId is null,'001',@ComponentRefId)  and CB1.OrderStyleRefId=SH.OrderStyleRefId and BD1.ColorRefId=SHD.ColorRefId and BD1.SizeRefId=SHD.SizeRefId
group by BD1.OrderStyleRefId,BD1.ColorRefId,BD1.SizeRefId),0)) as TodayCuttingQty,

(ISNULL((select sum(RA.RejectQty) FROM PROD_RejectAdjustment AS RA
inner join PROD_CuttingBatch AS ICB ON RA.CuttingBatchId=ICB.CuttingBatchId and RA.CompId=ICB.CompId
where ICB.ColorRefId=SHD.ColorRefId and ICB.ComponentRefId=IIF(@ComponentRefId is null,'001',@ComponentRefId) and ICB.OrderStyleRefId=SH.OrderStyleRefId and RA.SizeRefId=SHD.SizeRefId and RA.CompId=SHD.CompId),0) ) as RejectQty,

(select SUM(SIP.InputQuantity) from PROD_SewingInputProcess as SI
inner join PROD_SewingInputProcessDetail as SIP on SI.SewingInputProcessId=SIP.SewingInputProcessId
where SI.OrderStyleRefId=SH.OrderStyleRefId and SI.ColorRefId=SHD.ColorRefId and SIP.SizeRefId=SHD.SizeRefId ) as InputQty,
(select SUM(SIP.Quantity) from PROD_SewingOutPutProcess as SI
inner join PROD_SewingOutPutProcessDetail as SIP on SI.SewingOutputProcessId=SIP.SewingOutputProcessId
where  SI.OrderStyleRefId=SH.OrderStyleRefId and SI.ColorRefId=SHD.ColorRefId and SIP.SizeRefId=SHD.SizeRefId ) as OutputQty,
COM.Name as CompanyName,
COM.FullAddress
FROM OM_BuyOrdShipDetail AS SHD 
INNER JOIN OM_BuyOrdShip AS SH ON SHD.CompId = SH.CompId AND SHD.OrderShipRefId = SH.OrderShipRefId
inner join OM_Color as CLR on SHD.ColorRefId=CLR.ColorRefId and SHD.CompId=CLR.CompId
INNER JOIN OM_Size as SZ on SHD.SizeRefId=SZ.SizeRefId and SHD.CompId=SZ.CompId
inner join VOM_BuyOrdStyle as BOST on SH.OrderStyleRefId=BOST.OrderStyleRefId and SH.CompId=BOST.CompId
left join Company as COM on SHD.CompId=COM.CompanyRefId
WHERE (BOST.BuyerRefId=@BuyerRefId OR @BuyerRefId IS NULL)  and (BOST.OrderNo=@OrderNo  OR  @OrderNo IS NULL) and (BOST.OrderStyleRefId=@OrderStyleRefId  OR @OrderStyleRefId  IS NULL)  and BOST.CompId=@CompId 


group by COM.Name ,COM.FullAddress, BOST.BuyerName,CLR.ColorName,SZ.SizeName,BOST.RefNo,BOST.StyleName,SHD.CompId,SHD.ColorRefId,SH.OrderStyleRefId,SHD.SizeRefId




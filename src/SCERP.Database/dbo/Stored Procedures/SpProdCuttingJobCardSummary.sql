CREATE procedure [dbo].[SpProdCuttingJobCardSummary]
 @OrderStyleRefId varchar(7),
 @ComponentRefId varchar(3),
  @ColorRefId varchar(4),
 @CompId varchar(3)

as
--select 
--distinct SD.SizeRow, S.SizeName , 
-- SD.Quantity 
--,ISNULL(( SELECT  ISNULL(SUM(B.Quantity), 0) AS SQty FROM  PROD_BundleCutting as B WHERE (B.OrderStyleRefId = SD.OrderStyleRefId and B.ColorRefId=CBT.ColorRefId and B.SizeRefId=SD.SizeRefId and B.ComponentRefId=CBT.ComponentRefId) AND (B.CompId = SD.CompId)
--And B.SizeRefId=SD.SizeRefId),0) as CuttingQuantity,
-- ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
--  inner join PROD_CuttingBatch as CB on RA.CuttingBatchId=CB.CuttingBatchId and RA.CompId=CB.CompId
--  where CB.ComponentRefId=CBT.ComponentRefId and CB.ColorRefId=CBT.ColorRefId and  CB.OrderStyleRefId=SD.OrderStyleRefId and RA.SizeRefId=SD.SizeRefId and RA.CompId=SD.CompId  ),0) as RejectQty,
--  CBT.ComponentName,
--  CLR.ColorName
--  ,OST.RefNo as OrderNo
--  ,OST.StyleName
--  ,OST.BuyerName,
--  C.Name as CompanyName,
--  C.FullAddress
--from VBuyOrdShipDetail as SD
--inner join VwCuttingBatch as CBT on SD.ColorRefId=CBT.ColorRefId and SD.OrderStyleRefId=CBT.OrderStyleRefId and SD.CompId=CBT.CompId
--inner join VOM_BuyOrdStyle as OST on SD.OrderStyleRefId=OST.OrderStyleRefId  and SD.CompId=SD.CompId
--inner join OM_Size as S on SD.SizeRefId=S.SizeRefId and SD.CompId=S.CompId
--inner join OM_Color as CLR on CBT.ColorRefId=CLR.ColorRefId and CBT.CompId=CLR.CompId
--inner join Company as C on SD.CompId=C.CompanyRefId
--where CBT.OrderStyleRefId=@OrderStyleRefId and CBT.ComponentRefId=@ComponentRefId and CBT.ColorRefId= @ColorRefId and CBT.CompId=@CompId
----group by S.SizeName,SD.SizeRefId,SD.SizeRow, SD.OrderStyleRefId , SD.CompId,SD.SizeRow, OST.RefNo ,OST.StyleName,OST.BuyerName,C.Name ,C.FullAddress,CBT.ComponentRefId,  CBT.ComponentName,  CLR.ColorName,CBT.ColorRefId
--order by SD.SizeRow


select SHD.SizeRow,S.SizeName,CLR.ColorName ,SUM(SHD.QuantityP) as Quantity
,ISNULL(( SELECT ISNULL(SUM(B.Quantity), 0) AS SQty FROM  PROD_BundleCutting as B WHERE (B.OrderStyleRefId = SH.OrderStyleRefId and B.ColorRefId=SHD.ColorRefId and B.ComponentRefId=@ComponentRefId) AND (B.CompId = SH.CompId)
And B.SizeRefId=SHD.SizeRefId),0) as CuttingQuantity,

 ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
  inner join PROD_CuttingBatch as CB on RA.CuttingBatchId=CB.CuttingBatchId and RA.CompId=CB.CompId
  where CB.ComponentRefId=@ComponentRefId and CB.ColorRefId=SHD.ColorRefId and  CB.OrderStyleRefId=SH.OrderStyleRefId and RA.SizeRefId=SHD.SizeRefId and RA.CompId=SH.CompId  ),0) as RejectQty
   ,(select Top(1) ComponentName from OM_Component where ComponentRefId=@ComponentRefId and CompId=SH.CompId) as ComponentName,
   CLR.ColorName
  ,OST.RefNo as OrderNo
  ,OST.StyleName
  ,OST.BuyerName,
  C.Name as CompanyName,
  C.FullAddress
 from OM_BuyOrdShip as SH
inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId
inner join VOM_BuyOrdStyle as OST on SH.OrderStyleRefId=OST.OrderStyleRefId  and SH.CompId=OST.CompId
inner join OM_Size as S on SHD.SizeRefId=S.SizeRefId and SH.CompId=S.CompId
inner join OM_Color as CLR on SHD.ColorRefId=CLR.ColorRefId and SH.CompId=CLR.CompId
inner join Company as C on SH.CompId=C.CompanyRefId

where SH.OrderStyleRefId=@OrderStyleRefId and SHD.ColorRefId=@ColorRefId and SH.CompId=@CompId
group by SHD.SizeRow, SH.OrderStyleRefId,SHD.SizeRefId,SHD.ColorRefId,SH.CompId,S.SizeName,CLR.ColorName,OST.RefNo,OST.BuyerName,OST.StyleName, C.Name,C.FullAddress
CREATE PROCEDURE [dbo].[SPProdBundleSlip]
(
@CuttingBatchRefId VARCHAR(6),
@CompId VARCHAR(3)
)
AS
BEGIN
--Select CB.CompId, 
--CB.JobNo,

--B.CuttingBatchRefId,
--B.BatchNo,
--CB.ComponentRefId,
-- (select ComponentName from OM_Component where ComponentRefId= CB.ComponentRefId and CompId=CB.CompId) as ComponentName,
--CONVERT(varchar(10),  B.BundleCuttingId) as BundleCuttingId,
--ROW_NUMBER() OVER(ORDER BY B.BundleCuttingId ) as BundleNo,
--B.BundleStart,
--B.BundleEnd,
--(B.BundleEnd-B.BundleStart) AS Quantity,
--BOS.BuyerName,S.SizeName,
--C.ColorName,BOS.RefNo as OrderNo,
--BOS.StyleName From PROD_BundleCutting AS B 
--Inner JOIN OM_Size AS S
--ON B.SizeRefId=S.SizeRefId AND B.CompId=S.CompId
--Inner JOIN OM_Color AS C
--ON B.ColorRefId=C.ColorRefId AND B.CompId=C.CompId
--INNER JOIN PROD_CuttingBatch AS CB
--ON B.CuttingBatchId=CB.CuttingBatchId AND B.CompId=CB.CompId
--INNER JOIN VOM_BuyOrdStyle AS BOS 
--ON CB.BuyerRefId=BOS.BuyerRefId and CB.OrderStyleRefId=BOS.OrderStyleRefId and CB.CompId=BOS.CompId 
--where B.CuttingBatchRefId=@CuttingBatchRefId AND B.CompId=@CompId
--END

--Select CB.CompId, 
--CB.JobNo,

--B.CuttingBatchRefId,
--B.BatchNo,
--CB.ComponentRefId,
-- (select ComponentName from OM_Component where ComponentRefId= CB.ComponentRefId and CompId=CB.CompId) as ComponentName,
--CONVERT(varchar(10),  B.BundleCuttingId) as BundleCuttingId,
--ROW_NUMBER() OVER(ORDER BY B.BundleCuttingId ) as BundleNo,
--B.BundleStart,
--B.BundleEnd,
--(B.BundleEnd-B.BundleStart) AS Quantity,
--BOS.BuyerName,S.SizeName,
--C.ColorName,BOS.RefNo as OrderNo,
--BOS.StyleName From PROD_BundleCutting AS B 
--Inner JOIN OM_Size AS S
--ON B.SizeRefId=S.SizeRefId AND B.CompId=S.CompId
--Inner JOIN OM_Color AS C
--ON B.ColorRefId=C.ColorRefId AND B.CompId=C.CompId
--INNER JOIN PROD_CuttingBatch AS CB
--ON B.CuttingBatchId=CB.CuttingBatchId AND B.CompId=CB.CompId
--INNER JOIN VOM_BuyOrdStyle AS BOS 
--ON CB.BuyerRefId=BOS.BuyerRefId and CB.OrderStyleRefId=BOS.OrderStyleRefId and CB.CompId=BOS.CompId 
--where B.CuttingBatchRefId=@CuttingBatchRefId AND B.CompId=@CompId


Select CB.CompId, 
CB.JobNo,

B.CuttingBatchRefId,
B.BatchNo,
CB.ComponentRefId,
 (select top(1) CT.CountryName from OM_BuyOrdShip as SH
inner join Country as CT on SH.CountryId=CT.Id
where  SH.OrderShipRefId=CB.StyleRefId and OrderStyleRefId=CB.OrderStyleRefId and CompId=CB.CompId) as Country,
 (select ComponentName from OM_Component where ComponentRefId= CB.ComponentRefId and CompId=CB.CompId) as ComponentName,
CONVERT(varchar(10),  B.BundleCuttingId) as BundleCuttingId,
ROW_NUMBER() OVER(ORDER BY B.BundleCuttingId ) as BundleNo,
B.BundleStart,
B.BundleEnd,
(B.BundleEnd-B.BundleStart) AS Quantity,
BOS.BuyerName,S.SizeName,
C.ColorName,BOS.RefNo as OrderNo,
BOS.StyleName From PROD_BundleCutting AS B 
Inner JOIN OM_Size AS S
ON B.SizeRefId=S.SizeRefId AND B.CompId=S.CompId
Inner JOIN OM_Color AS C
ON B.ColorRefId=C.ColorRefId AND B.CompId=C.CompId
INNER JOIN PROD_CuttingBatch AS CB
ON B.CuttingBatchId=CB.CuttingBatchId AND B.CompId=CB.CompId
INNER JOIN VOM_BuyOrdStyle AS BOS 
ON CB.BuyerRefId=BOS.BuyerRefId and CB.OrderStyleRefId=BOS.OrderStyleRefId and CB.CompId=BOS.CompId 
where B.CuttingBatchRefId=@CuttingBatchRefId AND B.CompId=@CompId

order by B.BundleCuttingId
END





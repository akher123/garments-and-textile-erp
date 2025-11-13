CREATE procedure [dbo].[SpProdBundelChart]
(
@CuttingBatchRefId VARCHAR(6),
@CompId VARCHAR(3)
)

as

select CB.CuttingBatchRefId, BOST.BuyerName,BOST.RefNo as OrderNo,CLR.ColorName,BOST.StyleName,CMP.ComponentName ,count( BC.SizeRefId ) as BundleNo, CB.JobNo,BC.SizeRefId, SUM(BC.Quantity) as CuttingQty,BC.SSL+'#'+SZ.SizeName as SizeName,
ISNULL((select sum(RA.RejectQty)  from PROD_RejectAdjustment as RA
where RA.CuttingBatchId=CB.CuttingBatchId and RA.SizeRefId=BC.SizeRefId and RA.CompId=CB.CompId  ),0) as RejectQty,
(select Ratio from PROD_LayCutting

where CuttingBatchId=CB.CuttingBatchId and SizeRefId=BC.SizeRefId and CompId=CB.CompId) as Ratio,
(select top(1) CountryName from OM_BuyOrdShip 
left join Country on OM_BuyOrdShip.CountryId=Country.Id
where OM_BuyOrdShip.CompId=CB.CompId and OM_BuyOrdShip.OrderShipRefId=CB.StyleRefId) as CountryName
from PROD_CuttingBatch as CB

inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId and CB.CompId=BC.CompId
inner join VOM_BuyOrdStyle as BOST on CB.OrderStyleRefId=BOST.OrderStyleRefId and CB.CompId=BOST.CompId
inner join OM_Size as SZ on BC.SizeRefId=SZ.SizeRefId and BC.CompId=SZ.CompId
inner join OM_Color as CLR on BC.ColorRefId=CLR.ColorRefId and BC.CompId=CLR.CompId
inner join OM_Component as CMP on CB.ComponentRefId=CMP.ComponentRefId and CB.CompId=CMP.CompId
where CB.CuttingBatchRefId=@CuttingBatchRefId and CB.CompId=@CompId
group by BC.SizeRefId, CMP.ComponentName , CLR.ColorName, CB.JobNo,CB.CuttingBatchRefId,  SZ.SizeName ,BC.SSL,CB.CuttingBatchId,BC.SizeRefId,CB.CompId, BOST.BuyerName,BOST.RefNo,BOST.StyleName,CB.StyleRefId
order by BC.SSL






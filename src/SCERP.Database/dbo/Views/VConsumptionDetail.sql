CREATE view [dbo].[VConsumptionDetail]
as
select 
GC.ColorName as GColorName,
GS.SizeName as GSizeName,
PC.ColorName as PColorName,
PS.SizeName as PSizeName,
CD.CompId,
CD.QuantityP,
CD.ConsumptionDetailId,
CD.ConsRefId,
CD.GColorRefId,
CD.GSizeRefId,
CD.PColorRefId,
CD.PSizeRefId,
CD.PAllow,
CD.PPQty,
(ISNULL(CD.PPQty,0)*12) as PDzQty,
CD.TotalQty,
CD.Remarks,

ISNULL((select top(1)OM_BuyOrdStyleSize.SizeRow  from OM_Consumption
 inner join OM_BuyOrdStyleSize on OM_Consumption.OrderStyleRefId=OM_BuyOrdStyleSize.OrderStyleRefId and OM_Consumption.CompId=OM_BuyOrdStyleSize.CompId and OM_BuyOrdStyleSize.SizeRefId=CD.GSizeRefId and OM_Consumption.ConsRefId=CD.ConsRefId),0) as SizeRow
 from OM_ConsumptionDetail as CD
 left join OM_Color as GC on CD.GColorRefId=GC.ColorRefId  and CD.CompId=GC.CompId
 left join OM_Size as GS on CD.GSizeRefId=GS.SizeRefId and  CD.CompId=GS.CompId 
 left join OM_Color as PC on CD.PColorRefId=PC.ColorRefId  and CD.CompId=PC.CompId
 left join OM_Size as PS on CD.PSizeRefId=PS.SizeRefId and  CD.CompId=PS.CompId
 

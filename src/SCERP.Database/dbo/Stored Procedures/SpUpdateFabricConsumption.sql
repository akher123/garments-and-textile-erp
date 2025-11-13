
CREATE procedure [dbo].[SpUpdateFabricConsumption]
@ConsRefId varchar(10),
@CompId varchar(3)
as 

delete OM_ConsumptionDetail where  (ConsRefId =@ConsRefId) AND (CompID = @CompId)

INSERT INTO OM_ConsumptionDetail
                         (CompId, ConsRefId, GColorRefId, GSizeRefId, PColorRefId, PSizeRefId, QuantityP, PPQty, PAllow, TotalQty)
SELECT        CompID, ConsRefId, GColorRefId, GSizeRefId, PColorRefId, PSizeRefId, 0 AS QuantityP, SUM(QuantityP * PPQty) / SUM(QuantityP) AS PPQty, (SUM(TQty) - SUM(QuantityP * PPQty)) * 100 / SUM(QuantityP * PPQty) 
                         AS PAllow, SUM(TQty) AS TQty
FROM            OM_CompConsumptionDetail 
WHERE        (ConsRefId =@ConsRefId) AND (CompID = @CompId)
GROUP BY CompID, ConsRefId, GColorRefId, GSizeRefId, PColorRefId, PSizeRefId
having SUM(QuantityP) > 0

UPDATE       OM_ConsumptionDetail
SET                QuantityP = ROUND((1 - PAllow * 0.01) * TotalQty / PPQty, 0)
WHERE        (ConsRefId =@ConsRefId) AND (CompId =@CompId)


UPDATE OM_Consumption 
SET Quantity=(select sum(TotalQty) from OM_ConsumptionDetail where  (OM_ConsumptionDetail.ConsRefId =@ConsRefId ) AND (OM_ConsumptionDetail.CompId =@CompId))
where  (OM_Consumption.ConsRefId =@ConsRefId ) AND (OM_Consumption.CompId =@CompId)


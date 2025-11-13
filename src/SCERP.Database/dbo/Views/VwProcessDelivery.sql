CREATE view [dbo].[VwProcessDelivery]
as
select PD.*,BOST.RefNo as OrderName ,BOST.BuyerName,BOST.StyleName,P.Name as PartyName,
(select SUM(Quantity) from PROD_ProcessDeliveryDetail where ProcessDeliveryId=PD.ProcessDeliveryId) as TotalQuantity
 from PROD_ProcessDelivery as PD
inner join VOM_BuyOrdStyle as BOST on PD.OrderNo=BOST.OrderNo and PD.BuyerRefId=BOST.BuyerRefId and PD.OrderStyleRefId=BOST.OrderStyleRefId and PD.CompId=BOST.CompId
inner join Party as P on PD.PartyId=P.PartyId and PD.CompId=P.CompId
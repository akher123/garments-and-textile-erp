CREATE PROCEDURE [dbo].[SpProdStyleWiseTagCuttingReport]
(
@BuyerRefId varchar(3)=NULL,
@OrderNo varchar(12)=NULL,
@OrderStyleRefId varchar(7)=NULL,
@ComponentRefId varchar(3)=NULL,
@CompId Varchar(3)
)
AS
BEGIN
Select CS.CuttingSequenceId,CS.BuyerRefId, ombo.RefNo as OrderNo,CS.OrderStyleRefId,CS.ColorRefId,CS.ComponentRefId,OMB.BuyerName,
OMC.ColorName,OMS.StyleName,COM.ComponentName AS Sequence from PROD_CuttingSequence AS CS

INNER JOIN OM_Buyer AS OMB
ON CS.BuyerRefId=OMB.BuyerRefId AND CS.CompId=OMB.CompId
INNER JOIN OM_Color AS OMC
ON CS.ColorRefId=OMC.ColorRefId AND CS.CompId=OMC.CompId
INNER JOIN OM_BuyOrdStyle AS OMBOS
ON CS.OrderStyleRefId=OMBOS.OrderStyleRefId AND CS.CompId=OMBOS.CompId
INNER JOIN OM_BuyerOrder AS OMBO
ON OMBOS.OrderNo=OMBO.OrderNo AND OMBOS.CompId=OMBO.CompId
INNER JOIN OM_Style AS OMS
ON OMS.StylerefId=OMBOS.StyleRefId AND OMS.CompID=OMBOS.CompId
INNER JOIN OM_Component AS COM
ON COM.ComponentRefId=CS.ComponentRefId AND COM.CompId=CS.CompId
WHERE  CS.CompId=@CompId and (CS.BuyerRefId=@BuyerRefId OR @BuyerRefId IS NULL) and (CS.OrderNo=@OrderNo OR @OrderNo IS NULL) and (CS.OrderStyleRefId=@OrderStyleRefId OR @OrderStyleRefId IS NULL) AND (CS.ComponentRefId=@ComponentRefId OR @ComponentRefId IS NULL) 
END


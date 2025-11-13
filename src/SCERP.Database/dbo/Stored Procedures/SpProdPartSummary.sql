CREATE PROCEDURE [dbo].[SpProdPartSummary]
(
@BuyerRefId varchar(3)=NULL,
@OrderNo varchar(12)=NULL,
@OrderStyleRefId varchar(7)=NULL,
@ComponentRefId varchar(3)=NULL,
@CompId Varchar(3)
)
AS
BEGIN
Select CT.CuttingTagId,CT.CuttingSequenceId,CT.CompId,CT.IsSolid,CT.IsPrint,CT.IsEmbroidery,
CS.CuttingSequenceId,CS.BuyerRefId,OMBO.RefNo AS OrderNo,CS.OrderStyleRefId,CS.ColorRefId,CS.ComponentRefId,OMB.BuyerName,
OMC.ColorName,OMS.StyleName,COM.ComponentName AS Sequence from PROD_CuttingTag AS CT
INNER JOIN PROD_CuttingSequence AS CS
ON CT.CuttingSequenceId=CS.CuttingSequenceId 
INNER JOIN OM_Buyer AS OMB
ON CS.BuyerRefId=OMB.BuyerRefId
INNER JOIN OM_Color AS OMC
ON CS.ColorRefId=OMC.ColorRefId
INNER JOIN OM_BuyOrdStyle AS OMBOS
ON CS.OrderStyleRefId=OMBOS.OrderStyleRefId
INNER JOIN OM_Style AS OMS
ON OMS.StylerefId=OMBOS.StyleRefId
INNER JOIN OM_Component AS COM
ON COM.ComponentRefId=CS.ComponentRefId
INNER JOIN OM_BuyerOrder AS OMBO
ON OMBOS.OrderNo=OMBO.OrderNo
WHERE (CS.BuyerRefId=@BuyerRefId OR @BuyerRefId IS NULL) and (CS.OrderNo=@OrderNo OR @OrderNo IS NULL) and (CS.OrderStyleRefId=@OrderStyleRefId OR @OrderStyleRefId IS NULL)  and (CS.CompId=@CompId)
END


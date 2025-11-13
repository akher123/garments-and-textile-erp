create VIEW [dbo].[VwCuttingProcessStyleActive]
AS
SELECT CPA.CuttingProcessStyleActiveId,CPA.CompId,CPA.BuyerRefId,CPA.OrderNo,CPA.OrderStyleRefId,OMB.BuyerName,BO.RefNo AS OrderRefNo,BOS.StyleName,CPA.StartDate,CPA.EndDate
 from PROD_CuttingProcessStyleActive AS CPA
INNER JOIN OM_Buyer AS OMB 
ON CPA.BuyerRefId=OMB.BuyerRefId AND CPA.CompId=OMB.CompId
INNER JOIN OM_BuyerOrder AS BO
ON CPA.OrderNo=BO.OrderNo AND CPA.CompId=BO.CompId
INNER JOIN VOMBuyOrdStyle AS BOS 
ON CPA.OrderStyleRefId=BOS.OrderStyleRefId AND CPA.CompId=BOS.CompId


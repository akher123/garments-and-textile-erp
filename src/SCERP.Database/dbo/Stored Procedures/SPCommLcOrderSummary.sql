
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <28/08/2016>
-- Description:	<> exec SPCommLcOrderSummary  
-- =============================================

CREATE PROCEDURE [dbo].[SPCommLcOrderSummary]
				
																									 
AS

BEGIN
	
	SET NOCOUNT ON;
						   						
						SELECT OM_Buyer.BuyerName
							  ,COMMLcInfo.LcNo
							  ,COMMLcInfo.LcDate  
							  ,COMMLcInfo.LcAmount
							  ,COMMLcInfo.LcQuantity   
							  ,CommBbLcInfo.BbLcNo
							  ,ISNULL(CommBbLcInfo.BbLcAmount, 0) AS BbLcAmount
							  ,ISNULL(CommExportDetail.ItemQuantity, 0) AS ExportQuantity
							  ,ISNULL(CommExportDetail.Rate, 0) AS Rate
							  ,ISNULL((CommExportDetail.ItemQuantity * CommExportDetail.Rate), 0)AS ExportAmount
							  ,ISNULL((CommExportDetail.ItemQuantity * CommExportDetail.Rate), 0) - ISNULL(CommBbLcInfo.BbLcAmount, 0) AS GrossProfit
		 		 							         
							  FROM [COMMLcInfo]
							  LEFT JOIN CommBbLcInfo ON CommBbLcInfo.LcRefId = COMMLcInfo.LcId AND CommBbLcInfo.IsActive = 1
							  LEFT JOIN OM_Buyer ON OM_Buyer.BuyerId = COMMLcInfo.BuyerId
							  LEFT JOIN CommExport ON CommExport.LcId = COMMLcInfo.LcId
							  LEFT JOIN CommExportDetail ON CommExportDetail.ExportId = CommExport.ExportId AND CommExportDetail.IsActive = 1
							  WHERE COMMLcInfo.IsActive = 1  

							  ORDER BY COMMLcInfo.LcDate DESC, OM_Buyer.BuyerName	
							  							 													
END
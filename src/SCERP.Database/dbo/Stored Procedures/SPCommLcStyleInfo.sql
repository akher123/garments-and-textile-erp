
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <29/02/2016>
-- Description:	<> exec SPCommLcStyleInfo  1 ,  '800070'
-- =============================================

CREATE PROCEDURE [dbo].[SPCommLcStyleInfo]
				

						@LcId			INT
					   ,@OrderNo		NVARCHAR(30)
																				 
AS

BEGIN
	
	SET NOCOUNT ON;
						   
						
			SELECT    CommLcInfo.LcId
					 ,CommLcInfo.LcNo		AS LcNo				
					 ,OM_BuyerOrder.RefNo	AS OrderNo		
					 ,SUM(StyleQuantity)		AS Quantity
																							
					  FROM dbo.COMMLcStyle	
					  LEFT JOIN CommLcInfo ON COMMLcInfo.LcId = COMMLcStyle.LcRefId	
					  LEFT JOIN OM_BuyerOrder ON OM_BuyerOrder.OrderNo  = COMMLcStyle.OrderNo

					  WHERE COMMLcStyle.IsActive = 1
					  AND (CommLcInfo.LcId = @LcId OR @LcId = 0 )
					  AND (OM_BuyerOrder.RefNo = @OrderNo OR @OrderNo='')
					   
					  GROUP BY  CommLcInfo.LcId, CommLcInfo.LcNo, OM_BuyerOrder.RefNo, COMMLcStyle.OrderNo																				
																	 													
END
-- =============================================
-- Author:		<Author,Md.akheruzzaman,Name>
-- Create date: <Create Date,2019-08-25,>
-- Description:	<Description,Style wise TNA Operation Status,>
-- =============================================
CREATE PROCEDURE [dbo].[SpMisGetStyleWiseTnaOperations] 
	@OrderStyleRefId varchar(7),
	@CompId varchar(3)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	   SET NOCOUNT ON;
	      --- Start SECTION FOR BULK SWEING
          DECLARE @RQT_QTY INT,@AC_QTY INT;
		  SELECT   @AC_QTY=SUM(SOD.Quantity) FROM PROD_SewingOutPutProcess AS SO
		  INNER JOIN PROD_SewingOutPutProcessDetail AS SOD ON SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
		  WHERE SO.OrderStyleRefId=@OrderStyleRefId and SO.CompId=@CompId

		  SELECT @RQT_QTY= SUM(OSD.Quantity) from OM_BuyOrdShip AS OS
          INNER JOIN OM_BuyOrdShipDetail AS OSD ON OS.OrderShipRefId=OSD.OrderShipRefId AND OS.CompId=OSD.CompId
          WHERE OS.OrderStyleRefId=@OrderStyleRefId and OS.CompId=@CompId
          UPDATE OM_TNA SET ActualQty=@AC_QTY, RequiredQty=@RQT_QTY WHERE OrderStyleRefId=@OrderStyleRefId and  ShortName='BSW' AND CompId=@CompId
	      
		  --- END SECTION FOR BULK SWEING
		
		 SELECT @AC_QTY= SUM(BC.Quantity) from PROD_CuttingBatch AS CB
         INNER JOIN PROD_BundleCutting AS BC ON CB.CuttingBatchId=BC.CuttingBatchId
		 WHERE CB.OrderStyleRefId=@OrderStyleRefId and CB.CompId=@CompId AND CB.ComponentRefId='001' -- Body Component is 001 Code
		 UPDATE OM_TNA SET  RequiredQty=@RQT_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		 and  ShortName='BCUT' -- BULK CUTTING  
		 AND CompId=@CompId  

		 select @AC_QTY= SUM(PFD.InputQuantity) from PROD_FinishingProcess AS PF
         INNER JOIN PROD_FinishingProcessDetail AS PFD ON PF.FinishingProcessId=PFD.FinishingProcessId
	     WHERE PF.OrderStyleRefId=@OrderStyleRefId and PF.CompId=@CompId and PF.FType=2
		 UPDATE OM_TNA SET  RequiredQty=@RQT_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId   AND CompId=@CompId
		 AND  ShortName='FISH' -- FINISHING 

		 SELECT @AC_QTY=SUM(SHD.ShipmentQty) FROM Inventory_StyleShipment AS SH
         INNER JOIN Inventory_StyleShipmentDetail AS SHD ON SH.StyleShipmentId=SHD.StyleShipmentId
		  WHERE SH.OrderStyleRefId=@OrderStyleRefId and SH.CompId=@CompId 
		 UPDATE OM_TNA SET  RequiredQty=@RQT_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		 AND  ShortName='FRI' -- FRI 

		 DECLARE @GREY_QTY DECIMAL(10,4),@FINISH_QTY DECIMAL(10,4);
		 select  @GREY_QTY=SUM([GreyQty]),@FINISH_QTY=SUM([TQty]) from VCompConsumptionDetail WHERE OrderStyleRefId=@OrderStyleRefId and CompId=@CompId  

		  select @AC_QTY= SUM(POD.Quantity) from CommPurchaseOrder AS PO
          INNER JOIN CommPurchaseOrderDetail AS POD ON PO.PurchaseOrderId=POD.PurchaseOrderId
          WHERE PO.OrderStyleRefId=@OrderStyleRefId AND PO.CompId=@CompId AND  PO.PType='Y'
		  UPDATE OM_TNA SET  RequiredQty=@GREY_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		  AND  ShortName='YBD' -- YARN BOOKING

		 
		  select @GREY_QTY= SUM(RD.ReceivedQty-RD.RejectedQty) from Inventory_MaterialReceiveAgainstPoDetail AS RD
          INNER JOIN Inventory_MaterialReceiveAgainstPo AS MR ON RD.MaterialReceiveAgstPoId=MR.MaterialReceiveAgstPoId
          where RD.OrderStyleRefId=@OrderStyleRefId AND MR.StoreId=1
		 
		  UPDATE OM_TNA SET  RequiredQty=@AC_QTY,ActualQty=@GREY_QTY WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		  AND  ShortName='YID' -- YARN INHOUSE


		
		 select @AC_QTY=SUM(KR.Quantity) from PROD_KnittingRoll AS KR
         INNER JOIN PLAN_Program AS PG ON KR.ProgramId=PG.ProgramId
		 WHERE PG.OrderStyleRefId=@OrderStyleRefId and PG.CompId=@CompId 
		 UPDATE OM_TNA SET  RequiredQty=@GREY_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId   AND CompId=@CompId
		 AND  ShortName='KNIT' -- Knitting  


		 SELECT @AC_QTY=SUM(BD.Quantity) from Pro_Batch AS B
         INNER JOIN PROD_BatchDetail AS BD ON B.BatchId=BD.BatchId
         WHERE B.OrderStyleRefId=@OrderStyleRefId and B.CompId=@CompId
		 UPDATE OM_TNA SET  RequiredQty=@FINISH_QTY,ActualQty=@AC_QTY WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		 AND  ShortName='BDY' -- BULK DYEING SOLID FAB  

		 
		 DECLARE @ACS_DATE Date;
		 select @ACS_DATE=MIN(InvDate) from PROD_ProcessDelivery AS B
		 WHERE B.OrderStyleRefId=@OrderStyleRefId and B.CompId=@CompId
		 UPDATE OM_TNA SET  ASDate=CONVERT(VARCHAR(10), @ACS_DATE, 103) WHERE OrderStyleRefId=@OrderStyleRefId  AND CompId=@CompId
		 AND  ShortName='PES' -- PRINT/EM  

		

	    SELECT OM_TNA.TnaRowId, 
	    VOM_BuyOrdStyle.BuyerName,
		VOM_BuyOrdStyle.RefNo as OrderNo,
		VOM_BuyOrdStyle.StyleName,
		Quantity,
		VOM_BuyOrdStyle.Merchandiser,
		OM_TNA.SerialId,
		OM_TNA.ActivityName,
		OM_TNA.PSDate,
		OM_TNA.PEDate,
		dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'Rmks') +' '+ OM_TNA.Rmks AS Rmks,
		dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'XWho') +' '+ OM_TNA.XWho AS XWho,
	    dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'XWhen')+' '+OM_TNA.XWhen AS XWhen,
		dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'ASDate')+' '+CONVERT(varchar,OM_TNA.ASDate,103) AS ASDate,
		dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'AEDate')+' '+CONVERT(varchar,OM_TNA.AEDate,103) AS AEDate,
		OM_TNA.RequiredQty,
		OM_TNA.ActualQty,
		OM_TNA.Responsible,
		dbo.TnalActivityLogCount(OM_TNA.TnaRowId,'UpdateRemarks')+' '+OM_TNA.UpdateRemarks  AS UpdateRemarks
		FROM OM_TNA
		INNER JOIN VOM_BuyOrdStyle on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId AND OM_TNA.CompId=VOM_BuyOrdStyle.CompId
		WHERE  OM_TNA.OrderStyleRefId=@OrderStyleRefId  AND OM_TNA.CompId=@CompId  AND VOM_BuyOrdStyle.ActiveStatus=1 
		ORDER BY VOM_BuyOrdStyle.RefNo,VOM_BuyOrdStyle.StyleName, OM_TNA.SerialId
        
		
END



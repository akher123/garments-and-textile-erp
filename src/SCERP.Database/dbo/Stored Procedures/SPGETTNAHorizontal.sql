-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <28/11/2015>
-- Description:	<> exec SPGETTNAHorizontal '00', '000', '0000', '0000000', '001'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPGETTNAHorizontal]
			
							@SeasonRefId			NVARCHAR(2)
						   ,@BuyerRefId				NVARCHAR(3)
						   ,@MerchandiserRefId		NVARCHAR(5)
						   ,@OrderStyleRefId		NVARCHAR(7)
						   ,@CompanyId				NVARCHAR(7)		
AS
BEGIN
							
	
			SET NOCOUNT ON;

						   SELECT OM_BuyOrdStyle.OrderStyleId

						  ,OM_Buyer.BuyerName				AS BuyerName
						  ,OM_Style.StyleName				AS StyleName				
						  ,OM_Merchandiser.EmpName			AS MerchandiserName
						  ,OM_BuyerOrder.OrderDate			AS OrderDate
						  ,Inventory_Item.ItemName			AS ItemName
						  ,OM_BuyOrdStyle.LCSTID			AS LCStatus
						  ,(SELECT COUNT(*) from OM_BuyOrdStyleColor WHERE OM_BuyOrdStyleColor.OrderStyleRefId = OM_BuyOrdStyle.OrderStyleRefId) AS NoOfColor
						  ,OM_BuyOrdStyle.CompId			AS CompId
						  ,OM_BuyOrdStyle.OrderStyleRefId   AS OrderStyleRefId
						  ,OM_BuyOrdStyle.StyleentDate		AS StyleentDate
						  ,OM_BuyerOrder.RefNo				AS OrderNo			
						  ,OM_BuyOrdStyle.StyleRefId		AS StyleRefId
						  ,OM_BuyOrdStyle.BuyerArt			AS BuyerArt
						  ,OM_Season.SeasonName				AS SeasonRefId
						  ,OM_BuyOrdStyle.BrandRefId		AS BrandRefId
						  ,OM_BuyOrdStyle.CatIRefId			AS CatIRefId
						  ,OM_BuyOrdStyle.Rmks				AS Remarks
						  ,OM_BuyOrdStyle.Quantity			AS Quantity
						  ,OM_BuyOrdStyle.Rate				AS Rate
						  ,OM_BuyOrdStyle.DiscPer			AS DiscPer
						  ,OM_BuyOrdStyle.Discount			AS Discount
						  ,OM_BuyOrdStyle.Amt				AS Amount
						  ,OM_BuyOrdStyle.ProductionQty		AS ProductionQty
						  ,OM_BuyOrdStyle.WORKORDER			AS WORKORDER
						  ,OM_BuyOrdStyle.JobQty			AS JobQty
						  ,OM_BuyOrdStyle.IsCostEstimated
						  ,OM_BuyOrdStyle.AllowancePer
						  ,OM_BuyOrdStyle.PackplanThru
						  ,OM_BuyOrdStyle.Amend
						  ,OM_BuyOrdStyle.YarnAmend
						  ,OM_BuyOrdStyle.AccAmend
						  ,OM_BuyOrdStyle.PackAmend
						  ,OM_BuyOrdStyle.DespatchClosed
						  ,OM_BuyOrdStyle.despatchQty
						  ,OM_BuyOrdStyle.UnitType
						  ,OM_BuyOrdStyle.CutGAmend
						  ,OM_BuyOrdStyle.GroupedStyleId
						  ,OM_BuyOrdStyle.IsGrouped
						  ,OM_BuyOrdStyle.TransferredTo
						  ,OM_BuyOrdStyle.CompanyUnitId
						  ,OM_BuyOrdStyle.Transfer
						  ,OM_BuyOrdStyle.BaseUnit
						  ,OM_BuyOrdStyle.EnquiryId
						  ,OM_BuyOrdStyle.BuyerItem
						  ,OM_BuyOrdStyle.OpenPrgAmend
						  ,OM_BuyOrdStyle.GUOMid
						  ,OM_BuyOrdStyle.GUomConv
						  ,OM_BuyOrdStyle.StyleGroupId
						  ,OM_BuyOrdStyle.StyleDeptId
						  ,OM_BuyOrdStyle.QCId
						  ,OM_BuyOrdStyle.QuotationId
						  ,OM_BuyOrdStyle.GroupItem
						  ,OM_BuyOrdStyle.FitId
						  ,OM_BuyOrdStyle.MisView
						  ,OM_BuyOrdStyle.PI
						  ,OM_BuyOrdStyle.LCSTID
						  ,OM_BuyOrdStyle.YBD
						  ,OM_BuyOrdStyle.YRD
						  ,OM_BuyOrdStyle.FBD
						  ,OM_BuyOrdStyle.KSD
						  ,OM_BuyOrdStyle.KCD
						  ,OM_BuyOrdStyle.DSD
						  ,OM_BuyOrdStyle.DCD
						  ,OM_BuyOrdStyle.PAD
						  ,OM_BuyOrdStyle.CSD
						  ,OM_BuyOrdStyle.CCD
						  ,OM_BuyOrdStyle.IPD
						  ,OM_BuyOrdStyle.SSD
						  ,OM_BuyOrdStyle.SCD
						  ,OM_BuyOrdStyle.FSD
						  ,OM_BuyOrdStyle.FCD
						  ,OM_BuyOrdStyle.IND
						  ,OM_BuyOrdStyle.EFD
						  ,OM_BuyOrdStyle.EXD
						  ,OM_BuyOrdStyle.PSD
						  ,OM_BuyOrdStyle.LCR
						  ,OM_BuyOrdStyle.YID
						  ,OM_BuyOrdStyle.AID
						  ,OM_BuyOrdStyle.KnMC
						  ,OM_BuyOrdStyle.Factory
						  ,OM_BuyOrdStyle.KnProd
						  ,OM_BuyOrdStyle.KnDays
						  ,OM_BuyOrdStyle.LnDays
						  ,OM_BuyOrdStyle.LCD
						  ,OM_BuyOrdStyle.BZF
						  ,OM_BuyOrdStyle.EMF
						  ,OM_BuyOrdStyle.ABD
						  ,OM_BuyOrdStyle.TAD
						  ,OM_BuyOrdStyle.PPSD
						  ,OM_BuyOrdStyle.PPMD
						  ,OM_BuyOrdStyle.PSND
						  ,OM_BuyOrdStyle.PRD
						  ,OM_BuyOrdStyle.ESD
						  ,OM_BuyOrdStyle.CCRD
						  ,OM_BuyOrdStyle.LDSD
						  ,OM_BuyOrdStyle.LDAD
						  ,OM_BuyOrdStyle.FID
						  ,OM_BuyOrdStyle.THRD
						  ,OM_BuyOrdStyle.WSD
						  ,OM_BuyOrdStyle.WRD
						  ,OM_BuyOrdStyle.FBDD
						  ,OM_BuyOrdStyle.FSource
						  ,OM_BuyOrdStyle.ActYBD
						  ,OM_BuyOrdStyle.ActYRD
						  ,OM_BuyOrdStyle.ActFBD
						  ,OM_BuyOrdStyle.ActKSD
						  ,OM_BuyOrdStyle.ActKCD
						  ,OM_BuyOrdStyle.ActDSD
						  ,OM_BuyOrdStyle.ActDCD
						  ,OM_BuyOrdStyle.ActPAD
						  ,OM_BuyOrdStyle.ActCSD
						  ,OM_BuyOrdStyle.ActCCD
						  ,OM_BuyOrdStyle.ActIPD
						  ,OM_BuyOrdStyle.ActSSD
						  ,OM_BuyOrdStyle.ActSCD
						  ,OM_BuyOrdStyle.ActFSD
						  ,OM_BuyOrdStyle.ActFCD
						  ,OM_BuyOrdStyle.ActIND
						  ,OM_BuyOrdStyle.ActEFD
						  ,OM_BuyOrdStyle.ActEXD
						  ,OM_BuyOrdStyle.ActPSD
						  ,OM_BuyOrdStyle.ActYID
						  ,OM_BuyOrdStyle.ActAID
						  ,OM_BuyOrdStyle.ActLCD
						  ,OM_BuyOrdStyle.ActBZD
						  ,OM_BuyOrdStyle.ActEMF
						  ,OM_BuyOrdStyle.ActABD
						  ,OM_BuyOrdStyle.ActTAD
						  ,OM_BuyOrdStyle.ActPPSD
						  ,OM_BuyOrdStyle.ActPPMD
						  ,OM_BuyOrdStyle.ActPSND
						  ,OM_BuyOrdStyle.ActPRD
						  ,OM_BuyOrdStyle.ActESD
						  ,OM_BuyOrdStyle.ActCCRD
						  ,OM_BuyOrdStyle.ActLDSD
						  ,OM_BuyOrdStyle.ActLDAD
						  ,OM_BuyOrdStyle.ActFID
						  ,OM_BuyOrdStyle.ActTHRD
						  ,OM_BuyOrdStyle.ActWSD
						  ,OM_BuyOrdStyle.ActWRD
						  ,OM_BuyOrdStyle.ActFBDD
						  ,OM_BuyOrdStyle.ActBZF
						  ,OM_BuyOrdStyle.SAPD
						  ,OM_BuyOrdStyle.ActSAPD
						  ,OM_BuyOrdStyle.CGRID

						  FROM OM_BuyOrdStyle									
						  LEFT JOIN OM_BuyerOrder ON OM_BuyerOrder.OrderNo = OM_BuyOrdStyle.OrderNo AND OM_BuyerOrder.CompId = @CompanyId
				          LEFT JOIN OM_Buyer ON OM_Buyer.BuyerRefId = OM_BuyerOrder.BuyerRefId AND OM_Buyer.CompId = OM_BuyerOrder.CompId AND OM_Buyer.CompId = @CompanyId
						  LEFT JOIN OM_Style ON OM_Style.StylerefId = OM_BuyOrdStyle.StyleRefId AND OM_Style.CompID = @CompanyId
						  LEFT JOIN Inventory_Item ON Inventory_Item.ItemId = OM_Style.ItemId 
						  LEFT JOIN OM_Merchandiser ON OM_Merchandiser.EmpId = OM_BuyerOrder.MerchandiserId AND OM_Merchandiser.CompId = @CompanyId
						  LEFT JOIN OM_Season ON OM_Season.SeasonRefId = OM_BuyOrdStyle.SeasonRefId AND OM_Season.CompId = @CompanyId

						  WHERE (OM_BuyOrdStyle.SeasonRefId = @SeasonRefId OR @SeasonRefId = '00')
						  AND (OM_Buyer.BuyerRefId = @BuyerRefId OR @BuyerRefId ='000')
						  AND (OM_Merchandiser.EmpId = @MerchandiserRefId OR @MerchandiserRefId = '0000')
						  AND (OM_BuyOrdStyle.OrderStyleRefId = @OrderStyleRefId OR @OrderStyleRefId = '0000000')
						  AND OM_BuyOrdStyle.CompId = @CompanyId
						  								
END

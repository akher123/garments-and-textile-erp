
CREATE procedure [dbo].[SpReportShipmentAlert]
@buyerRefId varchar(4)='',
@orderNo varchar(12)='',
@orderStyleRefId varchar(7)='',
@compId varchar(3)
as

truncate table REPORT_ShipmentAlert

INSERT INTO REPORT_ShipmentAlert
                         (BuyerRefId, MerchandiserId, OrderNo, OrderStyleRefId, Quantity, EFD, CuttingQty, SewingQty, FinshQty, ShipedQty, XStatus, ColorCode)
SELECT        BO.BuyerRefId, BO.MerchandiserId, BO.OrderNo AS OrderNo, ST.OrderStyleRefId, ST.Quantity, ST.EFD, 0 AS CuttingQty, 0 AS SewingQty, 0 AS FinshQty, 0 AS ShipedQty, '' AS XStatus, 'WHITE' AS ColorCode
FROM            OM_BuyOrdStyle AS ST INNER JOIN
                         OM_BuyerOrder AS BO ON ST.OrderNo = BO.OrderNo AND ST.CompId = BO.CompId
WHERE        (BO.SCont = 'N') AND (ST.ActiveStatus = 1) AND (BO.Closed = 'O')

update REPORT_ShipmentAlert set ShipedQty=ISNULL((select SUM(S.ShipmentQty) from Inventory_StyleShipmentDetail as S

where S.OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId),0)

update REPORT_ShipmentAlert set ShipedDate=(select MAX(S.ShipDate) from Inventory_StyleShipment as S

where S.OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId)


update REPORT_ShipmentAlert set XStatus='Pending'
where  REPORT_ShipmentAlert.ShipedDate is null

update REPORT_ShipmentAlert set XStatus='Shiped Out'
where  REPORT_ShipmentAlert.ShipedDate is not null 

update REPORT_ShipmentAlert set XStatus='Late Shiped Out'
where  REPORT_ShipmentAlert.ShipedDate is not null  and  REPORT_ShipmentAlert.ShipedDate>REPORT_ShipmentAlert.EFD

update REPORT_ShipmentAlert set ColorCode='YELLOW'
where  DATEDIFF(day,GETDATE(),REPORT_ShipmentAlert.EFD )>=0 and DATEDIFF(day,GETDATE(),REPORT_ShipmentAlert.EFD )<=5

update REPORT_ShipmentAlert set ColorCode='RED'
where  DATEDIFF(day,GETDATE(),REPORT_ShipmentAlert.EFD )<0 


update REPORT_ShipmentAlert set ColorCode='GREEN'
where  REPORT_ShipmentAlert.ShipedDate is not null 

update REPORT_ShipmentAlert set ColorCode='Magenta'
where  REPORT_ShipmentAlert.ShipedDate is not null  and  REPORT_ShipmentAlert.ShipedDate>REPORT_ShipmentAlert.EFD

update REPORT_ShipmentAlert set CuttingQty=ISNULL((select SUM(BC.Quantity) from PROD_CuttingBatch as CB
inner join PROD_BundleCutting as BC on CB.CuttingBatchId=BC.CuttingBatchId
 where CB.OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId and CB.ComponentRefId='001'),0)


update REPORT_ShipmentAlert set SewingQty=ISNULL(( select SUM(SD.Quantity) from PROD_SewingOutPutProcess as SI
inner join PROD_SewingOutPutProcessDetail as SD on SI.SewingOutPutProcessId=SD.SewingOutPutProcessId
where SI.OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId),0)

update REPORT_ShipmentAlert set FinshQty=ISNULL(( select SUM(FPD.InputQuantity) from PROD_FinishingProcess as FP
inner join PROD_FinishingProcessDetail as FPD on FP.FinishingProcessId=FPD.FinishingProcessId
where FP.OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId and FP.FType=2),0)


update REPORT_ShipmentAlert set EFD=(select MAX(ShipDate) from OM_BuyOrdShip where OrderStyleRefId=REPORT_ShipmentAlert.OrderStyleRefId)


select SA.*,BO.Merchandiser,BO.BuyerName,BO.RefNo as OrderName,BO.StyleName
 from  REPORT_ShipmentAlert as SA
 inner join VOM_BuyOrdStyle as BO on BO.OrderStyleRefId=SA.OrderStyleRefId
 where BO.CompId=@compId and  ((BO.BuyerRefId=@buyerRefId) or ( @buyerRefId='')) and ((BO.OrderNo=@orderNo) or (@orderNo =''))and ((BO.OrderStyleRefId=@orderStyleRefId) or (@orderStyleRefId=''))
--exec SpReportShipmentAlert






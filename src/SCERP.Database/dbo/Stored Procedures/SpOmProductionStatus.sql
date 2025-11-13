-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <2/24/2016,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOmProductionStatus]
	-- Add the parameters for the stored procedure here
   @CompId varchar(3),
   @FromDate datetime = NULL,
   @ToDate datetime = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select
					B.BuyerName as Buyer,
				    BO.OrderNo as OrderRef,
				    BO.RefNo as OrderNo,
					FORMAT(BO.OrderDate,'dd-MM-yyy') as [OrderDate],
					FORMAT((select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo),'dd-MM-yyy') as ShipDate,
					ST.StyleName as Style,
					BO.Quantity ,
				    BO.Quantity  as [PlanQty],
				    (select SUM(TQty)  from OM_CompConsumptionDetail where OrderStyleRefId=OST.OrderStyleRefId and CompId=OST.CompId) as [RFabric],
					 (select ISNULL(SUM(TQty/(1-(ProcessLoss*0.01))),0)  from OM_CompConsumptionDetail where OrderStyleRefId=OST.OrderStyleRefId and CompId=OST.CompId) as [RYarn],
					
					    (select SUM(PROD_KnittingRoll.Quantity) from PROD_KnittingRoll
                        inner join PLAN_Program on PROD_KnittingRoll.ProgramId=PLAN_Program.ProgramId
                        where PLAN_Program.OrderStyleRefId=OST.OrderStyleRefId and PLAN_Program.CompId=OST.CompId) as Knit,
					      (select  SUM(Inventory_FinishFabDetailStore.RcvQty) from Inventory_FinishFabDetailStore
                         inner join Pro_Batch on Inventory_FinishFabDetailStore.BatchId=Pro_Batch.BatchId
                        where Pro_Batch.OrderStyleRefId=OST.OrderStyleRefId  and Pro_Batch.CompId=OST.CompId) as [FProcess],
				   (select SUM(PROD_BundleCutting.Quantity) from PROD_CuttingBatch
				   inner join PROD_BundleCutting on PROD_CuttingBatch.CuttingBatchId=PROD_BundleCutting.CuttingBatchId
                   where PROD_CuttingBatch.OrderStyleRefId=OST.OrderStyleRefId and PROD_CuttingBatch.CompId=OST.CompId and PROD_CuttingBatch.ComponentRefId='001') as Cutting,
					(
select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId
where PROD_SewingOutPutProcess.OrderStyleRefId=OST.OrderStyleRefId and PROD_SewingOutPutProcess.CompId=OST.CompId) as Sewing,
					(
					
select SUM(PROD_FinishingProcessDetail.InputQuantity) from PROD_FinishingProcess
inner join PROD_FinishingProcessDetail on PROD_FinishingProcess.FinishingProcessId=PROD_FinishingProcessDetail.FinishingProcessId
where PROD_FinishingProcess.FType=2 and PROD_FinishingProcess.OrderStyleRefId=OST.OrderStyleRefId and PROD_FinishingProcess.CompId=OST.CompId) as Finish,
					(select SUM(ShipmentQty) from Inventory_StyleShipmentDetail
where OrderStyleRefId=OST.OrderStyleRefId and CompId=OST.CompId ) as Ship
					from OM_BuyerOrder as BO
					inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
					inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and Bo.CompId=M.CompId
					inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and Bo.CompId=OST.CompId
					inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompId
					where BO.CompId=@CompId
					 and (select MAX(ShipDate) from OM_BuyOrdShip as BSP where  BSP.CompId=BSP.CompId and BSP.OrderNo=BO.OrderNo) between @FromDate and @ToDate
					order by BO.BuyerOrderId
END






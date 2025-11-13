CREATE PROCEDURE [dbo].[SpProductionForecast]
@OutputDate DateTime
AS
BEGIN
DECLARE @OrderQty bigint,@dayCount int ;

--set @OrderQty=ISNULL((select SUM(SHD.QuantityP) from OM_BuyOrdShip as SH 
--inner join OM_BuyOrdShipDetail as SHD on SH.OrderShipRefId=SHD.OrderShipRefId and SH.CompId=SHD.CompId
--inner join OM_BuyOrdStyle as BST on SH.OrderStyleRefId=BST.OrderStyleRefId and SH.CompId=BST.CompId
--inner join OM_BuyerOrder as BO on BST.OrderNo=BO.OrderNo and BST.CompId=BO.CompId 
--where BO.SCont='N' and BST.ActiveStatus=1 and year(SH.ShipDate)=year(@OutputDate) and Month(SH.ShipDate)=Month(@OutputDate)),0) 
--set @dayCount=(select Count(*) from PLAN_WorkingDay where DayStatus=1 and WorkingDate>=@OutputDate and month(WorkingDate)=month(@OutputDate)  and year(WorkingDate)=year(@OutputDate))

set @OrderQty=ISNULL((select SUM(SH.Quantity) from 
 OM_BuyOrdStyle as BST 
inner join OM_BuyerOrder as BO on BST.OrderNo=BO.OrderNo and BST.CompId=BO.CompId 
inner join OM_BuyOrdShip as SH on BST.OrderStyleRefId=SH.OrderStyleRefId and BST.CompId=SH.CompId
where BO.SCont='N' and BST.ActiveStatus=1 and year(SH.ShipDate)=year(@OutputDate) and Month(SH.ShipDate)=Month(@OutputDate)),0) 
set @dayCount=(select Count(*) from PLAN_WorkingDay where DayStatus=1 and WorkingDate>=@OutputDate and month(WorkingDate)=month(@OutputDate)  and year(WorkingDate)=year(@OutputDate))


SELECT Convert(bigint, @OrderQty) AS TargetQty ,
Convert(bigint,SUM(PROD_SewingOutPutProcessDetail.Quantity))  AS ProductionQty,
 Convert(bigint,@OrderQty- SUM(PROD_SewingOutPutProcessDetail.Quantity)) AS RemainingtQty,
Convert(bigint,SUM(Quantity)/Count(distinct PROD_SewingOutPutProcess.OutputDate)) AS ProductionRate,
Convert(bigint,((Count(distinct PROD_SewingOutPutProcess.OutputDate)+@dayCount))*(SUM(Quantity)/Count(distinct PROD_SewingOutPutProcess.OutputDate))) AS ForecastQty,
Convert(bigint,@OrderQty-((Count(distinct PROD_SewingOutPutProcess.OutputDate)+@dayCount))*(SUM(Quantity)/Count(distinct PROD_SewingOutPutProcess.OutputDate))) AS ShortQty  FROM PROD_SewingOutPutProcessDetail 
inner join PROD_SewingOutPutProcess on PROD_SewingOutPutProcessDetail.SewingOutPutProcessId=PROD_SewingOutPutProcess.SewingOutPutProcessId
where  year(PROD_SewingOutPutProcess.OutputDate)=year(@OutputDate) and Month(PROD_SewingOutPutProcess.OutputDate)=Month(@OutputDate)  and PROD_SewingOutPutProcess.LineId>0
END

--exec [SpProductionForecast] '2016-10-30'

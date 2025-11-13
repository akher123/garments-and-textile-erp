-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>  exec SpProdDailyFabricReceive '001','2016-04-03','010' ,'45-163-203'
-- Description:	<Daily Fabric Received> exec SpProdDailyFabricReceive '001','2016-04-03','010','45-163-203'
-- =============================================
CREATE PROCEDURE [dbo].[SpProdDailyFabricReceive]
   @CompId varchar(3),
   @ReceivedDate datetime,
   @BuyerRefId varchar(3),
   @StyleName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
select BO.RefNo as OrderNo,
OST.OrderStyleRefId,
BO.BuyerRefId,
B.BuyerName,
ST.StyleName,
CPM.ComponentRefId,
CPM.ComponentName ,
I.ItemName,
C.ConsRefId,
BO.CompId,
CLR.ColorName,
COMPD.PColorRefId as ColorRefId,
COMPD.PColorRefId,
SUM(COMPD.TQty) as ReqFabric,
sum(COMPD.QuantityP) as Quantity,
(SUM(COMPD.TQty)/sum(COMPD.QuantityP))*12 as ConsPDZ,
(sum(COMPD.GSM)/Count(*)) as GSM,
CPM.ComponentName ,
(select isnull(SUM(FabricQty),0)  from PROD_DailyFabricReceive where OrderStyleRefId=OST.OrderStyleRefId and ConsRefId=C.ConsRefId and ComponentRefId=CCOM.ComponentRefId and CompId=C.CompId and ColorRefId=COMPD.PColorRefId and ReceivedDate=@ReceivedDate    ) as ToDayReceived,
(select isnull(SUM(FabricQty),0)  from PROD_DailyFabricReceive where OrderStyleRefId=OST.OrderStyleRefId and ConsRefId=C.ConsRefId and ComponentRefId=CCOM.ComponentRefId and CompId=C.CompId and ColorRefId=COMPD.PColorRefId and  ReceivedDate<=@ReceivedDate ) as TotalFabricQty
 from OM_CompConsumption as CCOM 
inner join OM_CompConsumptionDetail as COMPD on CCOM.ComponentSlNo=COMPD.CompomentSlNo and CCOM.CompId=COMPD.CompID and CCOM.ConsRefId=COMPD.ConsRefId
inner join OM_Consumption as C on CCOM.ConsRefId=C.ConsRefId and CCOM.CompId=C.CompId
inner join OM_Component as CPM on CCOM.ComponentRefId=CPM.ComponentRefId and CCOM.CompId=CPM.CompId
inner join Inventory_Item as I on C.ItemCode=I.ItemCode
inner join OM_BuyOrdStyle as OST on C.OrderStyleRefId=OST.OrderStyleRefId and C.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Color as CLR on COMPD.PColorRefId=CLR.ColorRefId 
where CCOM.CompId=@CompId and (Bo.BuyerRefId=@BuyerRefId or @BuyerRefId='-1')  and (ST.StyleName LIKE '%' + @StyleName + '%')
group by BO.RefNo ,OST.OrderStyleRefId,BO.BuyerRefId,B.BuyerName,ST.StyleName,CPM.ComponentRefId,I.ItemName,C.ConsRefId,BO.CompId,COMPD.PColorRefId,CPM.ComponentName ,COMPD.PColorRefId ,CLR.ColorName,CCOM.ComponentRefId,C.CompId

END

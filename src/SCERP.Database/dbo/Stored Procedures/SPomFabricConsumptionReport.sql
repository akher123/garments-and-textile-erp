CREATE procedure [dbo].[SPomFabricConsumptionReport]

as
select BO.RefNo as OrderNo,
OST.OrderStyleRefId,
BO.BuyerRefId,
B.BuyerName,
ST.StyleName,
CPM.ComponentRefId,
I.ItemName,
C.ConsRefId,
BO.CompId,
(select sum(QuantityP) from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId) as Quantity,
(select TOP(1)PColorRefId from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId) as ColorRefId,
(select TOP(1)CLR.ColorName from OM_CompConsumptionDetail as COPD 
inner join OM_Color as CLR on COPD.PColorRefId=CLR.ColorRefId 
where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId
) as ColorName,
(select TOP(1)PPQty*12 from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId) as ConsPDZ,
(select TOP(1)GSM from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId) as GSM,
(select sum(ISNULL(TQty,0)) from OM_CompConsumptionDetail as COPD where  COPD.ConsRefId=C.ConsRefId and COPD.CompomentSlNo=CCOM.ComponentSlNo and COPD.OrderStyleRefId=C.OrderStyleRefId) as ReqFabric,
(select isnull(SUM(FabricQty),0)  from PROD_DailyFabricReceive where OrderStyleRefId=OST.OrderStyleRefId and ConsRefId=C.ConsRefId and ComponentRefId=CCOM.ComponentRefId and CompId=C.CompId  ) as ToDayReceived,
(select isnull(SUM(FabricQty),0)  from PROD_DailyFabricReceive where OrderStyleRefId=OST.OrderStyleRefId and ConsRefId=C.ConsRefId and ComponentRefId=CCOM.ComponentRefId and CompId=C.CompId ) as TotalFabricQty,
CPM.ComponentName  from OM_CompConsumption as CCOM 
inner join OM_Consumption as C on CCOM.ConsRefId=C.ConsRefId and CCOM.CompId=C.CompId
inner join OM_Component as CPM on CCOM.ComponentRefId=CPM.ComponentRefId and CCOM.CompId=CPM.CompId
inner join Inventory_Item as I on C.ItemCode=I.ItemCode
inner join OM_BuyOrdStyle as OST on C.OrderStyleRefId=OST.OrderStyleRefId and C.CompId=OST.CompId
inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
where CCOM.CompId='001'





CREATE procedure spSweingOrderPlanStatus
@CompId varchar(3)
as

select 
(select top(1) EmpName from OM_Merchandiser where EmpId=BO.MerchandiserId and CompId=BO.CompId) as Merchcandiser,
( select top(1) BuyerName from OM_Buyer where BuyerRefId=BO.BuyerRefId and CompId=BO.CompId) as BuyerName ,
 BO.RefNo as OrderNo, ST.Rate,
(select top(1) ItemName from VStyle where StylerefId=ST.StyleRefId) as ItemName,
(select top(1) StyleName from VStyle where StylerefId=ST.StyleRefId) as StyleName,
( select top(1) SeasonName from OM_Season where SeasonRefId=BO.SeasonRefId and CompId=BO.CompId) as Season,
BO.Fab,
BO.SMode,
BO.OrderDate,
ST.Quantity,
ST.EFD,
ISNULL((select SUM(TotalTargetQty) from PLAN_TargetProduction where OrderStyleRefId=ST.OrderStyleRefId),0) as PlanedQty,
ISNULL((select top(1) StMv from PROD_StanderdMinValue where OrderStyleRefId=ST.OrderStyleRefId),0) as Smv,
 (select Count(*) from OM_TNA where OrderStyleRefId=ST.OrderStyleRefId) as IsEntry
from  OM_BuyOrdStyle as ST 
inner join OM_BuyerOrder as BO on ST.OrderNo=BO.OrderNo and ST.CompId=BO.CompId
where BO.SCont='N' and ST.ActiveStatus=1 and BO.Closed='O' and ST.CompId=@CompId


--exec spSweingOrderPlanStatus '001'
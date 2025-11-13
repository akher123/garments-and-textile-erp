
CREATE view [dbo].[VwStyleFollowupStatus]
as
select 
BO.CompId,
M.EmpName as Merchandiser, 
BO.MerchandiserId,
BO.BuyerRefId,
B.BuyerName,
BO.RefNo,
BO.OrderNo ,
BOST.OrderStyleRefId,
ST.StyleName,
I.ItemName,
BO.Quantity as OrdQty ,
(select max(ShipDate) from OM_BuyOrdShip as SHIP where SHIP.OrderNo=BO.OrderNo and SHIP.CompId=BO.CompId) as ShipDate,
CAST((select count(*)from dbo.OM_BuyOrdShip  as SHIP where  SHIP.OrderStyleRefId=BOST.OrderStyleRefId and SHIP.CompId=BOST.CompId) as bit) as IsAssort,
CAST((select count(*) from OM_Consumption where left(ItemCode,2)='03' and CompId=BOST.CompId) as bit) as IsAccsCons,
CAST((select count(*)from dbo.OM_CompConsumption  as C where  C.OrderStyleRefId=BOST.OrderStyleRefId and C.CompId=BOST.CompId) as bit) as IsFabCons,
(select ISNULL(SUM(CD.TQty),0) from dbo.OM_CompConsumptionDetail as CD   where CD.CompId=BOST.CompId and CD.OrderStyleRefId=BOST.OrderStyleRefId) as FabConsQty,
CAST((select Count(*) from OM_YarnConsumption as YC
inner join OM_Consumption as C on YC.ConsRefId=C.ConsRefId and YC.CompId=C.CompId and YC.CompId=BOST.CompId and C.OrderStyleRefId=BOST.OrderStyleRefId) as bit) as IsYarnCons,
CAST((select Count(*) from PLAN_ProcessSequence where CompId=BOST.CompId and OrderStyleRefId=BOST.OrderStyleRefId) as bit) as IsProcsSeq,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId) as bit) as IsProdProg,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='I' and Pr.ProcessRefId='002') as bit) as IsKIn,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='O' and Pr.ProcessRefId='002') as bit) as IsKOut,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='I' and Pr.ProcessRefId='003') as bit) as IsDIn,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='O' and Pr.ProcessRefId='003') as bit) as IsDOut,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='I' and Pr.ProcessRefId='004') as bit) as IsCIn,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='O' and Pr.ProcessRefId='004') as bit) as IsCOut,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='I' and Pr.ProcessRefId='007') as bit) as IsSIn,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='O' and Pr.ProcessRefId='007') as bit) as IsSOut,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='I' and Pr.ProcessRefId='008') as bit) as IsFIn,
CAST((select count(*) from PROD_Production as Pr inner join PLAN_Program as Pg on Pr.ProrgramRefId=Pg.ProgramRefId and Pr.CompId=Pg.CompId and Pr.CompId=BOST.CompId and Pg.OrderStyleRefId=BOST.OrderStyleRefId and Pr.PType='O' and Pr.ProcessRefId='008') as bit) as IsFOut,
ISNULL((select sum(ShQty)  from OM_BuyOrdShipDetail as BOSH inner join OM_BuyOrdShip as OSH on BOSH.OrderShipRefId=OSH.OrderShipRefId and BOSH.CompId=OSH.CompId where   BOSH.CompId=BOST.CompId and OSH.OrderStyleRefId=BOST.OrderStyleRefId),0) as ShipQty
from OM_BuyerOrder as BO
inner join dbo.OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
inner join dbo.OM_BuyOrdStyle as BOST on BO.OrderNo=BOST.OrderNo and BO.CompId=BOST.CompId
inner join dbo.OM_Style as ST on BOST.StyleRefId=ST.StylerefId and BOST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId  and ST.CompID=I.CompId






















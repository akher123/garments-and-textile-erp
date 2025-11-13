
CREATE view [dbo].[VwDailyProductionReport]
as 
select
PR.CompId, 
M.EmpName as Merchandiser,
B.BuyerName,
BO.RefNo ,
BO.Quantity as OrderQty,
ST.StyleName,
I.ItemName
,MC.Name as LneName,
PR.ProdDate,
M.MerchandiserId,
B.BuyerRefId,
0 as TodayPoly,
0 as TotalPoly,

ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='004' and PR.PType='O' and PR.ProdDate=PR.ProdDate  and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TodayCutting,
ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='004' and PR.PType='O' and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TotalCutting,

ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='007' and PR.PType='I'  and PR.ProdDate=PR.ProdDate and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TodayTarget,

ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='007' and PR.PType='O'  and PR.ProdDate=PR.ProdDate and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TodaySewing,
ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='007' and PR.PType='O' and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TotalSewing,

ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='008' and PR.PType='O' and PR.ProdDate=PR.ProdDate and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TodayFinish,

ISNULL((select sum(PRD.Qty) from PROD_Production as PR
inner join PLAN_Program  as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join PROD_ProductionDetaill as PRD on PR.ProductionId=PRD.ProductionId and PR.CompId=PRD.CompId
where PR.ProcessRefId='008' and PR.PType='O' and PG.OrderStyleRefId=OST.OrderStyleRefId and PR.CompId=BO.CompId),0) as TotalFinish

from PROD_Production as PR
inner join PLAN_Program as PG on PR.ProrgramRefId=PG.ProgramRefId and PR.CompId=PG.CompId
inner join OM_BuyOrdStyle as OST on PG.OrderStyleRefId=OST.OrderStyleRefId and PG.CompId=OST.CompId
inner join OM_BuyerOrder as BO on OST.OrderNo=BO.OrderNo and OST.CompId=BO.CompId
inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
inner join OM_Style as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
inner join Inventory_Item as I on ST.ItemId=I.ItemId and ST.CompID=I.CompId
inner join Production_Machine as MC on PR.MachineRefId=MC.MachineRefId and PR.CompId=MC.CompId
where PType='O'




CREATE view [dbo].[VwKnittingRoll]
as
select KR.*,
PR.ProgramRefId,
  (select  top(1) BuyerName from OM_Buyer where BuyerRefId=PR.BuyerRefId and CompId=PR.CompId) as Buyer,
PR.OrderNo,
(select top(1) OM_Style.StyleName from OM_BuyOrdStyle 
inner join OM_Style on OM_BuyOrdStyle.StyleRefId=OM_Style.StylerefId and OM_BuyOrdStyle.CompId=OM_Style.CompID
 where OM_BuyOrdStyle.OrderStyleRefId=PR.OrderStyleRefId and OM_BuyOrdStyle.CompId=PR.CompId) as StyleName,
PR.OrderStyleRefId, 
   (select top(1)SizeName from OM_Size where SizeRefId=KR.SizeRefId  and CompId=PR.CompId ) as SizeName,
(select top(1)Name from Party where PartyId=PR.PartyId   ) as PartyName,
(select top(1)ColorName from OM_Color where ColorRefId=KR.ColorRefId  and CompId=PR.CompId)as ColorName,
  (select top(1)SizeName from OM_Size where SizeRefId=KR.FinishSizeRefId  and CompId=PR.CompId ) as FinishSizeName ,
((select top(1)Name from Production_Machine where MachineId=KR.MachineId  ))as MachineName,
 
(select ItemName from Inventory_Item where ItemCode=KR.ItemCode and CompId=KR.CompId) as ItemName ,
 (select ComponentName from OM_Component where ComponentRefId=KR.ComponentRefId and CompId=KR.CompId) as ComponentName
 from PROD_KnittingRoll as KR
inner join PLAN_Program as PR on KR.ProgramId=PR.ProgramId










CREATE view [dbo].[VOMBuyOrdStyle]
as
select 
BO.MerchandiserId,
ISNULL(S.StyleName,'---') as StyleName ,
ISNULL( Br.BrandName,'----') as BrandName ,
ISNULL( Ct.CatName,'---') as CatName,
ISNULL(Sn.SeasonName,'---') as SeasonName ,
ISNULL(I.ItemName,'---') as ItemName ,
BO.RefNo,
BO.BuyerRefId as BuyerRef,
OS.*
      from OM_BuyOrdStyle as OS
      Inner join OM_Style as S on OS.StyleRefId=s.StylerefId  and OS.CompId=s.CompID
	  left join OM_BuyerOrder as BO on OS.OrderNo=BO.OrderNo and  OS.CompId=BO.CompId
	  left join Inventory_Item as I on I.ItemId=S.ItemId
      left join OM_Brand as Br on OS.BrandRefId=Br.BrandRefId  and OS.CompId=Br.CompId
      left join OM_Category as Ct on OS.CatIRefId=Ct.CatRefId and OS.CompId=Ct.CompId
      left join OM_Season as Sn on OS.SeasonRefId=Sn.SeasonRefId and OS.CompId=Sn.CompId






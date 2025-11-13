CREATE procedure [dbo].[SpProdCutBankReport]
@OrderStyleRefId varchar(7),
@CompId varchar(3)
as
select  VOM_BuyOrdStyle.BuyerName,VOM_BuyOrdStyle.RefNo as OrderName,VOM_BuyOrdStyle.StyleName, OM_Size.SizeName,OM_Color.ColorName,OM_Component.ComponentName, PROD_CutBank.* from PROD_CutBank
inner join OM_Size on PROD_CutBank.SizeRefId=OM_Size.SizeRefId and PROD_CutBank.CompId=OM_Size.CompId
inner join OM_Color on PROD_CutBank.ColorRefId=OM_Color.ColorRefId and PROD_CutBank.CompId=OM_Color.CompId
inner join OM_Component  on PROD_CutBank.ComponentRefId=OM_Component.ComponentRefId and PROD_CutBank.CompId=OM_Component.CompId
inner join VOM_BuyOrdStyle on VOM_BuyOrdStyle.OrderStyleRefId=PROD_CutBank.OrderStyleRefId and VOM_BuyOrdStyle.CompId=PROD_CutBank.CompId

where PROD_CutBank.OrderStyleRefId=@OrderStyleRefId and PROD_CutBank.CompId=@CompId



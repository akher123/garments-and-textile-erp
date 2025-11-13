CREATE procedure [dbo].[SpProdTagSupplyer]
@OrderStyleRefId varchar(7),
@ColorRefId varchar(4),
@CuttingTagId bigint,
@CompId varchar(3)
as 
select distinct BOS.BuyerName,BOS.RefNo as OrderNo ,BOS.StyleName,CL.ColorName,CM.ComponentName as TagName,P.Name as Factory,P.Address,P.ContactPersonName,P.ContactPhone,P.Email from PROD_CuttingTagSupplier as CTS 
inner join PROD_CuttingTag as CT on CTS.CuttingTagId=CT.CuttingTagId
inner join PROD_CuttingSequence as CS on CT.CuttingSequenceId=CS.CuttingSequenceId
inner join OM_Color as CL on CS.ColorRefId=CL.ColorRefId and CTS.CompId=CL.CompId
inner join OM_Component as CM on CT.ComponentRefId=CM.ComponentRefId and CTS.CompId=CM.CompId
inner join Party as P on CTS.PartyId=P.PartyId
inner join VOM_BuyOrdStyle as BOS on CS.OrderStyleRefId=BOS.OrderStyleRefId and CTS.CompId=BOS.CompId
where   CT.CuttingTagId=@CuttingTagId and CS.ColorRefId=@ColorRefId and   CS.OrderStyleRefId=@OrderStyleRefId and BOS.CompId=@CompId 
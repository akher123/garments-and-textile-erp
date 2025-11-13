CREATE procedure [dbo].[spTnaSweingPandingStatus]
as 
--EXEC SPDateConversion
SELECT VOM_BuyOrdStyle.Merchandiser,  VOM_BuyOrdStyle.BuyerName as Buyer,VOM_BuyOrdStyle.RefNo as OrderName,VOM_BuyOrdStyle.StyleName, convert(varchar(10),SDate,103) as [Sweing Start date as per plan], 'Over due '+ CAST( DATEDIFF(d , SDate,GETDATE()) AS VARCHAR(MAX))+' days' as SweingStatus,VOM_BuyOrdStyle.OrderStyleRefId
FROM         OM_TNA
inner join VOM_BuyOrdStyle  on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
inner join OM_BuyOrdStyle as ST on  OM_TNA.OrderStyleRefId=ST.OrderStyleRefId
WHERE     (FlagValue = 'SSD') AND (ST.ActiveStatus = 1) AND (SDate < CAST(GETDATE() as DATE)) AND (ASDate IS NULL)
order by VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName ,SDate

--exec [spTnaSweingPandingStatus]

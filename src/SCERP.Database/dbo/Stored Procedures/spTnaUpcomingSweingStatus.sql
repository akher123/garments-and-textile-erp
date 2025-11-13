CREATE procedure [dbo].[spTnaUpcomingSweingStatus]
as 
--EXEC SPDateConversion
SELECT VOM_BuyOrdStyle.Merchandiser,  VOM_BuyOrdStyle.BuyerName as Buyer,VOM_BuyOrdStyle.RefNo as OrderName,VOM_BuyOrdStyle.StyleName, convert(varchar(10),SDate,103) as [Sweing Start date as per plan],'Remains '+ CAST( DATEDIFF(d , GETDATE(),SDate) AS VARCHAR(MAX))+' days' as SweingStatus
FROM         OM_TNA
inner join VOM_BuyOrdStyle  on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId
inner join OM_BuyOrdStyle as ST on  OM_TNA.OrderStyleRefId=ST.OrderStyleRefId
WHERE     (FlagValue = 'SSD') AND (ST.ActiveStatus = 1) AND (SDate >= CAST(GETDATE() as DATE)) AND (SDate <= GETDATE()+7) AND (ASDate IS NULL)
order by VOM_BuyOrdStyle.Merchandiser, VOM_BuyOrdStyle.BuyerName ,SDate
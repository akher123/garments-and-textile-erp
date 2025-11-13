

CREATE view [dbo].[VCostOrderStyle]
as
select CO.CostOrderStyleId,
CO.Unit,
CO.Qty,
CD.CostGroup,
CD.CostName,
CO.CompId,
CO.OrderStyleRefId,
CO.CostRefId,CO.CostDate,
CO.CostRate,
OS.StyleRefId,
ST.StyleName from OM_CostOrdStyle as CO
inner join OM_BuyOrdStyle as OS on CO.OrderStyleRefId=OS.OrderStyleRefId and CO.CompId=OS.CompId

inner join OM_CostDefination as CD on CO.CostRefId=CD.CostRefId and CO.CompId=CD.CompId

inner join OM_Style as ST on OS.StyleRefId=ST.StylerefId and OS.CompId=ST.CompID


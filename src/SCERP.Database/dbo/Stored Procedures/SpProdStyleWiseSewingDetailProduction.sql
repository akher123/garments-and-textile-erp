CREATE procedure [dbo].[SpProdStyleWiseSewingDetailProduction]
@CurrentMonth as varchar(20),
@CurrentYear int,
@LineId int,
@CompId varchar(3)
as 
select
Format(SO.OutputDate,'dd/MM/yyyy') as [SEWING DATE],
(select top(1) Merchandiser from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as  MERCHANDISER,
(select top(1) BuyerName from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as BUYER,
(select top(1) RefNo from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as [ORDER],
(select top(1) StyleName from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as [STYLE],
SUM(ISNULL(SOD.Quantity,0)) as [SEWING QTY]
from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where Year(SO.OutputDate)=@CurrentYear and DATENAME(month, SO.OutputDate)=@CurrentMonth and SO.LineId=@LineId and SO.CompId=@CompId
group by SO.OutputDate, SO.OrderStyleRefId ,SO.CompId
order by SO.OutputDate
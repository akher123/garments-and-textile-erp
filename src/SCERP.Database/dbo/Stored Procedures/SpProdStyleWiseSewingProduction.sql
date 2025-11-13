CREATE procedure [dbo].[SpProdStyleWiseSewingProduction]
@CurrentMonth as varchar(20),
@CurrentYear int,
@LineId int,
@CompId varchar(3)
as 

select

(select top(1) Merchandiser from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as  MERCHANDISER,
(select top(1) BuyerName from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as BUYER,
(select top(1) RefNo from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as [ORDER],
(select top(1) StyleName from VOM_BuyOrdStyle where OrderStyleRefId=SO.OrderStyleRefId and CompId=SO.CompId) as [STYLE],
(select top (1)Name from Production_Machine where MachineId=@LineId) as LINE, 
 SUM(ISNULL(SOD.Quantity,0)) as [SEWING QTY]
 from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where Year(SO.OutputDate)=@CurrentYear and DATENAME(month, SO.OutputDate)=@CurrentMonth and SO.LineId=@LineId
group by SO.OrderStyleRefId ,SO.CompId

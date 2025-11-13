CREATE procedure [dbo].[SpOmSeasonWiseOrderSummaryReport]
@CompId varchar(3),
@FromDate datetime,
@ToDate DateTime
as
select BOST.Merchandiser,
BOST.BuyerName,
BOST.RefNo as OrderNo,
BOST.StyleName,
BOST.Quantity,
 ST.Rate,Convert(varchar(2), DENSE_RANK() OVER(Order By MONTH(ST.EFD)))+'--'+
 LEFT(DATENAME(MONTH,ST.EFD),3)  as ShipMothd,ST.EFD as ShipDate from VOM_BuyOrdStyle as BOST 
inner join OM_BuyOrdStyle as ST on BOST.OrderStyleRefId=ST.OrderStyleRefId and BOST.CompId=ST.CompId
where ST.ActiveStatus=1 and ST.CompId=@CompId and ST.EFD>=@FromDate and ST.EFD<=@ToDate



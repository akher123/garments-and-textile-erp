CREATE procedure SpOmPandingConsumptionReport
@CompId varchar(3),
@MerchandiserId varchar(4)
as
		select M.EmpName,
		B.BuyerName,
		BO.RefNo as OrderNo,
		SZN.SeasonName,
		ST.StyleName,
		ST.ItemName, 
		OST.Quantity,
		OST.EFD as ShipDate,
		ISNULL(C.Quantity,0) as FabricQty 
		from OM_BuyerOrder as BO 
		inner join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId
		inner join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
		inner join OM_BuyOrdStyle as OST on BO.OrderNo=OST.OrderNo and BO.CompId=OST.CompId
		inner join OM_Season as SZN on OST.SeasonRefId=SZN.SeasonRefId and OST.CompId=SZN.CompId
		inner join VStyle as ST on OST.StyleRefId=ST.StylerefId and OST.CompId=ST.CompID
		left join OM_Consumption as C on OST.OrderStyleRefId=C.OrderStyleRefId and OST.CompId=C.CompId
		where C.OrderStyleRefId is null  and  BO.CompId=@CompId and (BO.MerchandiserId=@MerchandiserId or @MerchandiserId='-1')

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOmShipmentStatus]
	  @CompId varchar(3),
	  @BuyerRefId varchar(3),
      @MerchandiserId varchar(4),
	  @SeasonRefId varchar(2)
AS
BEGIN
	
	SET NOCOUNT ON;

				select 
					  B.BuyerName as BuyerName,
					  BO.RefNo as OrderNo,
					  BOS.BuyerArt as Articale,
					  S.StyleName as Style,
					  M.EmpName as Merchandiser,
					  I.ItemName as Item,
					  ( select ISNULL(Count(OrderStyleRefId),0) from OM_BuyOrdStyleColor
					   where OrderStyleRefId=BOS.OrderStyleRefId and CompId=BOS.CompId) as NoOfColor,
					   BOS.Quantity as OrderQty,
					   ( select ISNULL(SUM(Shipd.Quantity),0) from OM_BuyOrdShip as Ship
					   inner join OM_BuyOrdShipDetail as Shipd on Ship.OrderShipRefId=Shipd.OrderShipRefId and Ship.CompId=Shipd.CompId
						where Ship.OrderStyleRefId=BOS.OrderStyleRefId and Ship.CompId=BOS.CompId) as
						 AssortedQty,
						ISNULL( BOS.despatchQty,0) as ShippedQty,
						ISNULL( (BOS.Quantity-BOS.despatchQty),0) as BalanceQry,
						 BOS.EFD as ShipmentDate
					  from dbo.OM_BuyerOrder as BO

					  left join OM_Buyer as B on BO.BuyerRefId=B.BuyerRefId and BO.CompId=B.CompId

					  left join OM_Merchandiser as M on BO.MerchandiserId=M.EmpId and BO.CompId=M.CompId
	  
					  left join OM_BuyOrdStyle  as BOS on BO.OrderNo=BOS.OrderNo and BO.CompId=BO.CompId

					  left join OM_Style as S on BOS.StyleRefId=s.StylerefId  and BOS.CompId=s.CompID

					  left join Inventory_Item as I on I.ItemId=S.ItemId
				 where BO.CompId=@CompId  and  (BO.BuyerRefId=@BuyerRefId or @BuyerRefId='-1') and (BO.MerchandiserId= @MerchandiserId or @MerchandiserId='-1') and (BO.SeasonRefId=@SeasonRefId or @SeasonRefId='-1')
END

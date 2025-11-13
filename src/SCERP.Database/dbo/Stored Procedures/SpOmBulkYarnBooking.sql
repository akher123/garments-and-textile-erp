CREATE procedure SpOmBulkYarnBooking
@BulkBookingId bigint,
@CompId varchar(3)
as
SELECT   BK.BulkBookingRefId,
         BK.BookingDate, 
		 BK.Attention, 
		 BK.MerchadiserId,
		  BK.Note,
		   M.EmpName AS Merchandiser,
		    BKD.SequenceNo,
			 BKD.Fabrication,
			  BKD.ItemName,
			   B.BuyerName,
			    BKD.OrderNo, 
				BKD.StyleNo, 
                 BKD.ShipDate, BKD.GSM, BYD.ItemName AS YarnName, BYD.ColorName, BYD.CountName, BYD.OrdQty, BYD.ConsQty, BYD.Remarks
FROM            OM_BulkBooking AS BK INNER JOIN
                         OM_BulkBookingDetail AS BKD ON BK.BulkBookingId = BKD.BulkBookingId inner  JOIN
                         OM_BulkBookingYarnDetail AS BYD ON BKD.BulkBookingDetailId = BYD.BulkBookingDetailId INNER JOIN
                         OM_Buyer AS B ON BKD.BuyerRefId = B.BuyerRefId AND BKD.CompId = B.CompId INNER JOIN
                         OM_Merchandiser AS M ON BK.MerchadiserId = M.MerchandiserId
						 where BK.CompId=@CompId and BK.BulkBookingId=@BulkBookingId
-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOMRateQuittedYarn]
	-- Add the parameters for the stored procedure here
	@CompId varchar(3),
	@OrderStyleRefId varchar(7),
    @SupplierId int,
	@GroupCode varchar(2)
	
AS
BEGIN
   
     --and left(CN.ItemCode,2)=@GroupCode
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
   select
         CONVERT(bigint,0)as  PurchaseOrderDetailId,
		 CONVERT(bigint,0) as PurchaseOrderId,
		 null as PurchaseOrderRefId,   
		 '0000' as GSizeRefId,
		  '0000' as GColorRefId,
        CN.OrderStyleRefId,
         YCN.CompId,
		 I.ItemName,
		 YCN.ItemCode,
		 YCN.Rate as xRate,
		 YCN.SupplierId,
		 SPL.CompanyName as SupplierName,
		 KC.ColorRefId,
		 KC.ColorName,
		 KS.SizeName,
		 KS.SizeRefId,
		 YCN.KQty as Quantity,
		 MU.UnitName
		 from OM_Consumption as CN
		 inner join OM_YarnConsumption as YCN on CN.ConsRefId=YCN.ConsRefId and CN.CompId=YCN.CompId
		 inner join Mrc_SupplierCompany as SPL on YCN.SupplierId=SPL.SupplierCompanyId
		 left join OM_Color as KC on YCN.KColorRefId=KC.ColorRefId and YCN.CompID=KC.CompId
         left join OM_Size as KS on YCN.KSizeRefId=KS.SizeRefId and YCN.CompId=KS.CompId
         inner join Inventory_Item as I on YCN.ItemCode=I.ItemCode 
         left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
		 where CN.OrderStyleRefId=@OrderStyleRefId and CN.CompId=@CompId  and YCN.SupplierId=@SupplierId and CN.ConsGroup='P'
END



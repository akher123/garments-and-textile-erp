-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOMConsumtionItem]
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
		 CONVERT(bigint,CND.ConsumptionDetailId)as  PurchaseOrderDetailId,
		 CONVERT(bigint,0) as PurchaseOrderId,
		 null as PurchaseOrderRefId,
		 CN.CompId,
		 I.ItemName,
		 I.ItemId,
		 CN.ItemCode,
		 CN.Rate as xRate,
		 CN.SupplierId,
		 SPL.CompanyName as SupplierName,
		 PC.ColorRefId,
		 PC.ColorName,
		 PS.SizeName,
		 PS.SizeRefId,
		 CND.GColorRefId,
		 CND.GSizeRefId,
		 SUM(CND.TotalQty) as Quantity,
		 MU.UnitName
		 from OM_Consumption as CN
         inner join OM_ConsumptionDetail as CND on CN.ConsRefId=CND.ConsRefId and CN.CompId=CND.CompId
		 inner join Mrc_SupplierCompany as SPL on CN.SupplierId=SPL.SupplierCompanyId
         left join OM_Color as PC on ISNULL(CND.PColorRefId,'0000')=ISNULL(PC.ColorRefId,'0000') and CND.CompID=PC.CompId
		 left join Inventory_Item as I on CN.ItemCode=I.ItemCode 
         left join OM_Size as PS on ISNULL(CND.PSizeRefId,'0000') = ISNULL(PS.SizeRefId,'0000') and CND.CompID=PS.CompId
         left join MeasurementUnit as MU on I.MeasurementUinitId=MU.UnitId
		 where  CN.OrderStyleRefId=@OrderStyleRefId and CN.CompId=@CompId  and CN.SupplierId=@SupplierId and CN.ConsGroup='B'
		 group by CN.CompId,I.ItemName,I.ItemId, CN.ItemCode, CN.Rate, CN.SupplierId,SPL.CompanyName, PC.ColorRefId, PC.ColorName,PS.SizeName, PS.SizeRefId, MU.UnitName, CND.GColorRefId,CND.ConsumptionDetailId,
		 CND.GSizeRefId
END




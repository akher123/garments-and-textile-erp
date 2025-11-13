--Exec [dbo].[SPFabricConsumtion] 14
-- =============================================
-- Author:		<Md.Akheruzzaman>
-- Create date: <20/11/2014,,>
-- Description:	<Fabric fonsumtion with price >
-- =============================================
CREATE PROCEDURE [dbo].[SPFabricConsumtion]
	@SpecSheetId INT 
AS
BEGIN
DECLARE @FabricMenuCostPerDzn numeric(18,2)
DECLARE @UnitPrice numeric(18,2)

    --Fabric Menufecturing cost per dzn
	 SET @FabricMenuCostPerDzn= IsNull((SELECT 
	 SUM(MrcFabMen.NetConsPerDzn*MrcFabMen.PricePerKg)
	  FROM Mrc_FabricManufacturingCost AS MrcFabMen WHERE MrcFabMen.SpecSheetId=@SpecSheetId),0)

	  SET @UnitPrice=(@FabricMenuCostPerDzn/12)

    -- Select statement of all fabric consumtion with cost
	SELECT MrcFab.FabricConsumptionId, MrcSpec.FabricationDescription, MrcFab.StyleSize,MrcFab.Color,
	[dbo].[fnGetTotalFabricConsumption](MrcFab.FabricConsumptionId)AS ConsPerDzn ,
	@FabricMenuCostPerDzn as CostPerDzn,@UnitPrice AS UnitPrice,
	MrcFab.MeasurementUnit AS Unit,
	[dbo].[fnGetTotalFabricConsumption](MrcFab.FabricConsumptionId)*@UnitPrice AS Amount
	
	from Mrc_FabricConsumption as MrcFab
	inner join  Mrc_SpecificationSheet AS MrcSpec on MrcFab.SpecificationSheetId=MrcSpec.SpecificationSheetId
	where MrcFab.SpecificationSheetId=@SpecSheetId
END






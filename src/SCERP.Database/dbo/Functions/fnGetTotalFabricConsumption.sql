-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2014.11.19
-- Description:	To get total consumption cost
-- =============================================
CREATE FUNCTION [dbo].[fnGetTotalFabricConsumption]
(
	-- Add the parameters for the function here
	@FabricConsumptionId int
)
RETURNS numeric(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @BodyFabricConsumtion numeric(18,2)
	DECLARE @RibFabricConsumption numeric(18,2)
    DECLARE @MiscellaneousFabricConsumption numeric(18,2)= 0.0;
	DECLARE @TotalFabricConsumption numeric(18,2)

    -- Insert statements for procedure here
	SELECT @BodyFabricConsumtion =  [dbo].[fnGetBodyFabricConsumption](@FabricConsumptionId);
	SELECT @RibFabricConsumption =  [dbo].[fnGetRibFabricConsumption](@FabricConsumptionId);
	SELECT @MiscellaneousFabricConsumption =  [dbo].[fnGetMiscellaneousFabricConsumption](@FabricConsumptionId);
		
	SET @TotalFabricConsumption = @BodyFabricConsumtion + @RibFabricConsumption + @MiscellaneousFabricConsumption;

	RETURN @TotalFabricConsumption;

END






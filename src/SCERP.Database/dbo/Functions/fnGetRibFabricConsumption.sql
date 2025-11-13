
-- =============================================
-- Author:		Golam Rabbi	
-- Create date: 2014.18.11
-- Description:	Calculate Rib Fabric Consumption
-- =============================================

CREATE FUNCTION [dbo].[fnGetRibFabricConsumption]
(
	-- Add the parameters for the function here
	@FabricConsumptionId int
)
RETURNS numeric(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RibFabricConsumption numeric(18,2)
	DECLARE @NeckWidth numeric(18,2)
	DECLARE @FontNeckDrop numeric(18,2)
	DECLARE @RibHeight numeric(18,2)
	DECLARE @Allowance numeric(18,2)
	DECLARE @GSM numeric(18,2)
	DECLARE @Wastage numeric(18,2)

	-- Add the T-SQL statements to compute the return value here
	SELECT  @NeckWidth = ISNULL(NeckWidth,1),
			@FontNeckDrop = ISNULL(FrontNeckDrop,1),
			@RibHeight = ISNULL(RibHeight,1),
			@Allowance = ISNULL(Allowance,0),
			@GSM = ISNULL(GSM,1),
			@Wastage = ISNULL(Wastage,0)
	FROM  Mrc_RibFabricConsumption WHERE FabricConsumptionId = @FabricConsumptionId
	
	SET @RibFabricConsumption = (((@NeckWidth * @RibHeight * 12 * @GSM * @FontNeckDrop) + @Allowance)/10000000) + (@Wastage/100); 
	
	-- Return the result of the function
	RETURN @RibFabricConsumption

END






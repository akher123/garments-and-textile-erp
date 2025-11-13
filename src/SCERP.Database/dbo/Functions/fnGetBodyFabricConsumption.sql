-- =============================================
-- Author:		Golam Rabbi	
-- Create date: 2014.18.11
-- Description:	Calculate Body Fabric Consumption
-- =============================================
CREATE FUNCTION [dbo].[fnGetBodyFabricConsumption]
(
	-- Add the parameters for the function here
	@FabricConsumptionId int
)
RETURNS numeric(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @BodyFabricConsumtion numeric(18,2)
	DECLARE @BodyLength numeric(18,2)
	DECLARE @SleevLength numeric(18,2)
	DECLARE @LengthWiseAllowance numeric(18,2)
	DECLARE @HalfChest numeric(18,2)
	DECLARE @WidthWiseAllowance numeric(18,2)
	DECLARE @GSM numeric(18,2)
	DECLARE @Wastage numeric(18,2)

	-- Add the T-SQL statements to compute the return value here
	SELECT  @BodyLength = IsNull(BodyLength,0),
		    @SleevLength =  IsNull(SleeveLength,0),
			@LengthWiseAllowance =  IsNull(LengthWiseAllowance,0),
			@HalfChest =  IsNull(HalfChest,0),
			@WidthWiseAllowance =  IsNull(WidthWiseAllowance,0),
			@GSM = IsNull(GSM,1),
			@Wastage =  IsNull(Wastage,0)
	FROM  Mrc_BodyFabricConsumption WHERE FabricConsumptionId = @FabricConsumptionId
	
	SET @BodyFabricConsumtion = (((@BodyLength + @SleevLength + @LengthWiseAllowance) * (@HalfChest + @WidthWiseAllowance) * (@GSM * 2 * 12))/10000000) + (@Wastage/100); 
	-- Return the result of the function
	RETURN @BodyFabricConsumtion

END






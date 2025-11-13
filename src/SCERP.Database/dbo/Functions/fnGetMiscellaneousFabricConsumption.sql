
-- =============================================
-- Author:		Golam Rabbi	
-- Create date: 2014.18.11
-- Description:	Calculate Miscellaneous Fabric Consumption
-- =============================================

CREATE FUNCTION [dbo].[fnGetMiscellaneousFabricConsumption]
(
	-- Add the parameters for the function here
	@FabricConsumptionId int
)
RETURNS numeric(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @MiscellaneousFabricConsumption numeric(18,2)
	DECLARE @Length numeric(18,2)
	DECLARE @Width numeric(18,2)
	DECLARE @GSM numeric(18,2)
	DECLARE @Wastage numeric(18,2)
	DECLARE @NumberOfPcs int
	DECLARE @Allowance numeric(18,2)

	-- Add the T-SQL statements to compute the return value here
	SELECT  @Length = IsNull([Length],1),
			@Width = IsNull(Width, 1),
			@GSM = IsNull(GSM,1), 
			@NumberOfPcs = IsNull(NumberOfPcs,1), 
			@Allowance = IsNull(Allowance,0),
			@Wastage = IsNull(Wastage,0) 
	From  Mrc_MiscellaneousFabricConsumption WHERE FabricConsumptionId = @FabricConsumptionId
	
 
	SET @MiscellaneousFabricConsumption = (((@Length * @Width * @GSM * @NumberOfPcs * 12) + @Allowance)/10000000) + (@Wastage/100);

	-- Return the result of the function
	RETURN @MiscellaneousFabricConsumption

END






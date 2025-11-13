-- =============================================
-- Author:		Golam Rabbi
-- Create date: 2014.11.18
-- Description:	To get total fabric consumption
-- =============================================
CREATE PROCEDURE [dbo].[SPGetTotalFabricConsumption]
	-- Add the parameters for the stored procedure here
	@FabricConsumptionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalFabricConsumption numeric(18,2)
	SET @TotalFabricConsumption = [dbo].[fnGetTotalFabricConsumption](@FabricConsumptionId);
	SELECT @TotalFabricConsumption AS TotalFabricationConsumption;

END






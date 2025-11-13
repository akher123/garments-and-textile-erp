-- =============================================
-- Author:		<Md.Akheruzzaman>
-- Create date: <23/11/2014,,>
-- Description:	<Get style cost summary according to costing head,>
-- =============================================
CREATE PROCEDURE [dbo].[SPStyleCostingGroupByCostionHead]
	-- Add the parameters for the stored procedure here
     @SpecSheetId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT Mrc_CostingHead.CostingHeadId,  Mrc_CostingHead.Title,SUM(UnitPrice*12*QuantityPerPc) AS Amount FROM Mrc_StyleCost

inner join Mrc_CostingHead ON Mrc_StyleCost.CostingHeadId=Mrc_CostingHead.CostingHeadId

WHERE Mrc_StyleCost.SpecSheetId= @SpecSheetId

GROUP BY Mrc_CostingHead.Title,Mrc_CostingHead.CostingHeadId
END





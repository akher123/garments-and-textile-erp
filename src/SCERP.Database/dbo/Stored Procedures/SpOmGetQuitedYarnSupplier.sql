-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpOmGetQuitedYarnSupplier]
	-- Add the parameters for the stored procedure here
	@CompId varchar(3),
	@OrderStyleRefId varchar(7)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select distinct SPL.* from OM_YarnConsumption as YCON
	inner join OM_Consumption as CN on YCON.ConsRefId=CN.ConsRefId and YCON.CompId=CN.CompId
    inner join Mrc_SupplierCompany as SPL on YCON.SupplierId=SPL.SupplierCompanyId 
    where CN.OrderStyleRefId=@OrderStyleRefId and YCON.CompId=@CompId
    -- Insert statements for procedure here

END

-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[SpOmGetAssignedSupplier]
	-- Add the parameters for the stored procedure here
	@CompId varchar(3),
	@OrderStyleRefId varchar(7)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select distinct SPL.* from OM_Consumption as CON
    inner join Mrc_SupplierCompany as SPL on CON.SupplierId=SPL.SupplierCompanyId 
    where CON.OrderStyleRefId=@OrderStyleRefId and CON.CompId=@CompId
    -- Insert statements for procedure here

END

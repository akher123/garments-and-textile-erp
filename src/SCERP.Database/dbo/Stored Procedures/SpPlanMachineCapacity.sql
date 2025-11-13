-- =============================================
-- Author:		<Author,,Md.Akheruzzaman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpPlanMachineCapacity]
	-- Add the parameters for the stored procedure here
	@CompId varchar(3),
	@ProcessRefId varchar(3)
AS
BEGIN

	SET NOCOUNT ON;
          select PS.ProcessName,P.ProcessorName ,M.Name,M.NoMachine,M.EfficiencyPer,M.IdelPer,M.RatedCapacity from dbo.Production_Machine as M
		  inner join dbo.PROD_Processor as P on M.ProcessorRefId=P.ProcessorRefId and M.CompId=P.CompId
		  inner join PLAN_Process as PS on P.ProcessRefId=PS.ProcessRefId and P.CompId=PS.CompId
          where M.CompId=@CompId and M.IsActive=1 and P.ProcessorRefId=@ProcessRefId
END

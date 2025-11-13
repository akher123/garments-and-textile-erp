
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <04/04/2015>
-- Description:	<> exec SPPlanGetTemplate 1
-- =============================================

CREATE PROCEDURE [dbo].[SPPlanGetTemplate]


					  @TemplateId INT
						 
AS

BEGIN
	
	SET NOCOUNT ON;

																					
				SELECT [PLAN_Activity].[Id]
					  ,[CompId]
					  ,[ActivityCode]
					  ,[ActivityName]
					  ,[ActivityMode]
					  ,CAST([PLAN_TNA_Template].FromLeadTime AS NVARCHAR(3)) AS StartField
					  ,CAST([PLAN_TNA_Template].ToLeadTime AS NVARCHAR(3))AS EndField
					  ,[BufferDay]
					  ,[IsRelative]
					  ,[PLAN_Activity].[SerialId]
					  ,[PLAN_Activity].[CreatedDate]
					  ,[PLAN_Activity].[CreatedBy]
					  ,[PLAN_Activity].[EditedDate]
					  ,[PLAN_Activity].[EditedBy]
					  ,[PLAN_Activity].[IsActive]						  					  						

					  FROM [dbo].[PLAN_Activity]
					  LEFT OUTER JOIN [dbo].[PLAN_TNA_Template] ON  [dbo].[PLAN_TNA_Template].ActivityId = [dbo].[PLAN_Activity].Id
					  AND [PLAN_TNA_Template].TemplateId = @TemplateId
					   
					  WHERE [PLAN_Activity].CompId = '001' 
					  AND [PLAN_Activity].ActivityMode !='N'

					  ORDER BY [PLAN_Activity].[SerialId]
																	 													
END









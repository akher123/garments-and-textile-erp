
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <30/01/2016>
-- Description:	<> exec [SPPlanDynamicTnaColumn] 'YBD', '001'
-- =============================================

CREATE PROCEDURE [dbo].[SPPlanDynamicTnaColumn]


					@ColumnName		NVARCHAR(10) = ''
				   ,@CompanyId 		NVARCHAR(10) 
						 
AS

BEGIN
	
					SET NOCOUNT ON;

					Declare @DSQL NVARCHAR(MAX)			
		
					SET @DSQL = 'SELECT OrderStyleRefId,' + @ColumnName + ' AS YBD FROM dbo.[OM_BuyOrdStyle] WHERE CompId= '+ @CompanyId

					EXECUTE sp_executesql @DSQL
																																 													
END
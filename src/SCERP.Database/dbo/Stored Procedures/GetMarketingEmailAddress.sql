
-- ==========================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2017-11-18>
-- Description:	<> EXEC [GetMarketingEmailAddress]
-- ==========================================================================================================================

CREATE PROCEDURE [dbo].[GetMarketingEmailAddress]
				
AS

BEGIN
	
	SET NOCOUNT ON;
						  												
					SELECT [SerialId]
						  ,[Factory Name]
						  ,[Contact Person]
						  ,[Address]
						  ,[Email Address]
						  ,[Contact Number]
					  FROM [dbo].[MarketingEmailAddress2]	
					 WHERE [Email Address] IS NOT NULL	
					   AND SerialId BETWEEN 1 AND 2
				  ORDER BY SerialId		  						
END
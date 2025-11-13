-- ====================================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <11/04/2018>
-- Description:	<> EXEC SPDateConversion
-- ====================================================================================================================

CREATE PROCEDURE [dbo].[SPDateConversion]
			
																
AS

BEGIN
	
			SET NOCOUNT ON;

					  UPDATE [OM_TNA]
					  SET [SDate] = [dbo].[fnDateConvert]([PSDate])
					  WHERE [SDate] IS NULL 
					  AND [PSDate] IS NOT NULL 	
					  
					  
					  UPDATE OM_TNA SET ASDate = ( SELECT MIN(OutputDate) AS Expr1
                            FROM PROD_SewingOutPutProcess WHERE 
							(OrderStyleRefId = OM_TNA.OrderStyleRefId)) 
							WHERE (ASDate IS NULL) AND FlagValue = 'SSD'
							
					  UPDATE OM_TNA SET ActiveStatus = (SELECT TOP 1 ActiveStatus FROM OM_BuyOrdStyle
                            WHERE OrderStyleRefId = OM_TNA.OrderStyleRefId) WHERE FlagValue = 'SSD'
										
END

-- ===========================================================================================================
-- Author:		<Yasir>
-- Create date: <07-APRIL-2018>
-- Description:	<> EXEC [SPGetHiddenGlAccounts] 
-- ===========================================================================================================


CREATE PROCEDURE [dbo].[SPGetHiddenGlAccounts]
							

AS

BEGIN
	
	SET NOCOUNT ON;
			 
					 IF EXISTS(SELECT *  FROM [dbo].[Acc_GLAccounts_Hidden_Status] WHERE Status = 1)
					 BEGIN
												 
							SELECT [Id]
								  ,[ControlCode]
								  ,[AccountCode]
								  ,[AccountName]
								  ,[BalanceType]
								  ,[OpeningBalance]
								  ,[AccountType]
								  ,[IsActive]
							  FROM [dbo].[Acc_GLAccounts_Hidden]
							  WHERE [IsActive] = 1
					 END    	

					 ELSE

					 BEGIN
							SELECT [Id]
								  ,[ControlCode]
								  ,[AccountCode]
								  ,[AccountName]
								  ,[BalanceType]
								  ,[OpeningBalance]
								  ,[AccountType]
								  ,[IsActive]
							  FROM [dbo].[Acc_GLAccounts_Hidden]
							  WHERE [ControlCode] = 2000000
					 END			
END







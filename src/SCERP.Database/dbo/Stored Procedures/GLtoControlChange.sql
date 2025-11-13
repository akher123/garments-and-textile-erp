-- =====================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <27/10/2019>
-- Description:	<> EXEC  GLtoControlChange '4020175001', '4010106'
-- =====================================================================

CREATE PROCEDURE [dbo].[GLtoControlChange]
			

									@GLCode				DECIMAL(18,0)
								   ,@ControlCode		DECIMAL(18,0)
																					   
AS

BEGIN
	
			SET NOCOUNT ON;

					  	BEGIN TRAN
									DECLARE @MaxGLCode	  DECIMAL(18,0)
									DECLARE @Total		  DECIMAL(18,0)	


									SELECT @MaxGLCode = MAX(AccountCode) + 1  FROM Acc_GLAccounts WHERE ControlCode = @ControlCode																					
									SELECT @Total = COUNT(1) FROM Acc_GLAccounts WHERE ControlCode = @ControlCode AND AccountCode LIKE '%000%'

									IF(@Total = 1)  -- delete default 000 GL if exists
									BEGIN
										UPDATE Acc_GLAccounts
										SET IsActive = 0
										WHERE AccountCode = CAST(CAST(@ControlCode AS NVARCHAR(10)) + '000' AS DECIMAL(18,0))
									END

									IF(@MaxGLCode IS NULL)
									BEGIN
										SET @MaxGLCode = CAST(CAST(@ControlCode AS NVARCHAR(10)) + '001' AS DECIMAL(18,0))																													
									END																																																							
								
									IF EXISTS(SELECT AccountCode FROM Acc_GLAccounts WHERE AccountCode = @GLCode )
									BEGIN	
																																				
										UPDATE Acc_GLAccounts										 
										SET AccountCode = @MaxGLCode
										   ,ControlCode = @ControlCode
										WHERE AccountCode = @GLCode
									END
						COMMIT TRAN		
						

						DECLARE @Result INT = 1;

						IF (@@ERROR <> 0)
							SET @Result = 0;
		
						SELECT @Result;																									 					  					  														  						  											  							
END
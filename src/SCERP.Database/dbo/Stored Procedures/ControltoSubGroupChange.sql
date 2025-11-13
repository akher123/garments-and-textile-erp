-- ===============================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <27/10/2019>
-- Description:	<> EXEC ControltoSubGroupChange '40101', '4020175'
-- ===============================================================

CREATE PROCEDURE [dbo].[ControltoSubGroupChange]
			

									@SubGroupCode		DECIMAL(18,0)
								   ,@ControlCode		DECIMAL(18,0)
																					   
AS

BEGIN
	
			SET NOCOUNT ON;
					  
					  BEGIN TRAN
					  				
									DECLARE @MaxControlCode		DECIMAL(18,0)	

									SELECT @MaxControlCode = MAX(Controlcode) + 1  FROM Acc_ControlAccounts WHERE ParentCode = @SubGroupCode
									
									IF(@MaxControlCode IS NULL)
									BEGIN
										SET @MaxControlCode = CAST(CAST(@SubGroupCode AS NVARCHAR(10)) + '01' AS DECIMAL(18,0))
									END
									

								    -- Change control code
									UPDATE Acc_ControlAccounts
									SET ParentCode = @SubGroupCode
									   ,ControlCode = @MaxControlCode
									WHERE ControlCode = @ControlCode							
										

									-- Change control's child	
									IF EXISTS(SELECT AccountCode FROM Acc_GLAccounts WHERE ControlCode = @ControlCode)
									BEGIN									
										UPDATE Acc_GLAccounts
										SET ControlCode = @MaxControlCode
										   ,AccountCode = CAST(REPLACE(CAST([AccountCode] AS NVARCHAR(10)), @ControlCode, @MaxControlCode) AS DECIMAL(18,0))
										WHERE ControlCode = @ControlCode
									END

						COMMIT TRAN		
						

						DECLARE @Result INT = 1;

						IF (@@ERROR <> 0)
							SET @Result = 0;
		
						SELECT @Result;																								 					  					  														  						  											  							
END
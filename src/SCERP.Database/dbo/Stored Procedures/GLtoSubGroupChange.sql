-- =====================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <11/02/2020>
-- Description:	<> EXEC  GLtoSubGroupChange '4020175001', '40101'
-- =====================================================================

CREATE PROCEDURE [dbo].[GLtoSubGroupChange]
			

									@GLCode				DECIMAL(18,0)
								   ,@SubGroupCode		DECIMAL(18,0)
																					   
AS

BEGIN
	
			SET NOCOUNT ON;

					  	BEGIN TRAN
									
									DECLARE @MaxControlCode	    DECIMAL(18,0)
								    DECLARE @MaxGLCode			DECIMAL(18,0)	
									DECLARE @GLName				NVARCHAR(100)
									

									IF EXISTS(SELECT 1 FROM Acc_ControlAccounts WHERE ParentCode = @SubGroupCode AND IsActive = 1)
										BEGIN
											SELECT @MaxControlCode = MAX(ControlCode) + 1  FROM Acc_ControlAccounts WHERE ParentCode = @SubGroupCode AND IsActive = 1	
										END

									ELSE

										BEGIN
											SET @MaxControlCode = CAST(CAST(@SubGroupCode AS NVARCHAR(10)) + '01' AS DECIMAL(18,0))
										END
									
									SELECT @GLName = AccountName FROM  Acc_GLAccounts WHERE AccountCode = @GLCode AND IsActive = 1

									INSERT INTO [dbo].[Acc_ControlAccounts]
													   ([ParentCode]
													   ,[ControlCode]
													   ,[ControlName]
													   ,[ControlLevel]
													   ,[SortOrder]
													   ,[IsActive])
												 VALUES(
														@SubGroupCode
													   ,@MaxControlCode
													   ,@GLName
													   ,4
													   ,1
													   ,1
													   )
																		
									SET @MaxGLCode = CAST(CAST(@MaxControlCode AS NVARCHAR(10)) + '001' AS DECIMAL(18,0))		
										
									UPDATE Acc_GLAccounts										 
									SET AccountCode = @MaxGLCode
										,ControlCode = @MaxControlCode
									WHERE AccountCode = @GLCode
																																																																																						
						COMMIT TRAN		
						

						DECLARE @Result INT = 1;

						IF (@@ERROR <> 0)
							SET @Result = 0;
		
						SELECT @Result;																									 					  					  														  						  											  							
END
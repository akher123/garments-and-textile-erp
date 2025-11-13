
-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <29/02/2016>
-- Description:	<> exec [SPSaveJobcardTemp] 
-- =============================================

CREATE PROCEDURE [dbo].[SPSaveJobcardTemp]
				
						@EmployeeCardId NVARCHAR(30) 
					   ,@Date			DATETIME

					   ,@OutTime		TIME = NULL
					   ,@ExtraOT		DECIMAL(18,2) = NULL
					   ,@TotalExtraOT	DECIMAL(18,2) = NULL				 																				 
AS

BEGIN
	
	SET NOCOUNT ON;
						   
				IF(@OutTime IS NOT NULL)
				BEGIN		
					UPDATE EmployeeInOut 
					SET OutTime = @OutTime
					WHERE EmployeeCardId = @EmployeeCardId AND  CAST(TransactionDate AS DATE) = CAST( @Date AS DATE) AND IsActive = 1
				END		
				
				IF(@ExtraOT IS NOT NULL)
				BEGIN		
					UPDATE EmployeeInOut 
					SET ExtraOTHours = @ExtraOT
					WHERE EmployeeCardId = @EmployeeCardId AND  CAST(TransactionDate AS DATE) = CAST( @Date AS DATE) AND IsActive = 1
				END	
				
				IF(@TotalExtraOT IS NOT NULL)
				BEGIN		
					UPDATE EmployeeJobCard 
					SET TotalExtraOTHours = @TotalExtraOT
					WHERE EmployeeCardId = @EmployeeCardId AND  CAST( @Date AS DATE) BETWEEN CAST(FromDate AS DATE) AND CAST(ToDate AS DATE) AND IsActive = 1
				END																		
																	 													
END
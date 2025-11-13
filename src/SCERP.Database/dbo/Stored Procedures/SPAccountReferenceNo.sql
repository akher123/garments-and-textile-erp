
-- ===========================================================================================================
-- Author:		<Yasir>
-- Create date: <2018-09-05>
-- Description:	<> EXEC SPAccountReferenceNo 'CP','2018-09-05', 'GET'
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPAccountReferenceNo]


								 @VoucherType			NVARCHAR(50)
								,@Date				    DATETIME
								,@ActionType			NVARCHAR(50)
						
AS

BEGIN
	
	SET NOCOUNT ON;
		
								DECLARE @ReferenceNo	INT

								IF(@ActionType = 'GET')
								BEGIN
										SELECT @ReferenceNo = ReferenceNo FROM Acc_ReferenceNo
										WHERE VoucherType = @VoucherType
										AND CAST(@Date AS DATE) BETWEEN FromDate AND ToDate
		
										SELECT @ReferenceNo + 1
								END

								IF(@ActionType = 'SAVE')
								BEGIN
										UPDATE Acc_ReferenceNo
										SET ReferenceNo = ReferenceNo + 1
										WHERE VoucherType = @VoucherType
										AND CAST(@Date AS DATE) BETWEEN FromDate AND ToDate																											
								END
						
END
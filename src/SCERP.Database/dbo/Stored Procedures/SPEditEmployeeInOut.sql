
-- ===========================================================================================================
-- Author:		<Golam Rabbi>
-- Create date: <14-Sep-15 2:09:40 PM>
-- Description:	<> EXEC [SPEditEmployeeInOut]  
-- ===========================================================================================================

CREATE PROCEDURE [dbo].[SPEditEmployeeInOut]
						

							  @InOutName					NVARCHAR(50)	
							 ,@EmployeeCardId				NVARCHAR(10)
							 ,@TransactionDate				DATETIME
							 ,@Status						NVARCHAR(50)
							 ,@InTime						TIME
							 ,@OutTime						TIME
							 ,@LateInMinutes				INT
							 ,@OTHours						DECIMAL(18,2)
							 ,@ExtraOTHours					DECIMAL(18,2)
							 ,@WeekendOTHours				DECIMAL(18,2)
							 ,@HolidayOTHours				DECIMAL(18,2)
							 ,@Remarks						NVARCHAR(50)

								
AS

BEGIN
	
	SET NOCOUNT ON;

					IF(@InTime = '00:00:00')
					SET @InTime = NULL

					IF(@OutTime = '00:00:00')
					SET @OutTime = NULL

				
					IF(@InOutName = 'EmployeeJobCardModel')
					BEGIN

							UPDATE EmployeeInOutModel
							SET Status = @Status
							   ,InTime = @InTime
							   ,OutTime  = @OutTime
							   ,LateInMinutes = @LateInMinutes
							   ,OTHours = @OTHours							
							   ,Remarks = @Remarks														
							WHERE CAST(TransactionDate AS DATE) = @TransactionDate 
							AND EmployeeCardId = @EmployeeCardId
					END
			 

					IF(@InOutName = 'EmployeeJobCard_10PM')
					BEGIN

							UPDATE EmployeeInOut_10PM
							SET Status = @Status
							   ,InTime = @InTime
							   ,OutTime  = @OutTime
							   ,LateInMinutes = @LateInMinutes
							   ,OTHours = @OTHours
							   ,ExtraOTHours = @ExtraOTHours
							   ,WeekendOTHours = @WeekendOTHours
							   ,HolidayOTHours = @HolidayOTHours
							   ,Remarks = @Remarks														
							WHERE CAST(TransactionDate AS DATE) = @TransactionDate 
							AND EmployeeCardId = @EmployeeCardId
					END


					IF(@InOutName = 'EmployeeJobCard_10PM_NoWeekend')
					BEGIN

							UPDATE EmployeeInOut_10PM_NoWeekend
							SET Status = @Status
							   ,InTime = @InTime
							   ,OutTime = @OutTime
							   ,LateInMinutes = @LateInMinutes
							   ,OTHours = @OTHours
							   ,ExtraOTHours = @ExtraOTHours
							   ,WeekendOTHours = @WeekendOTHours
							   ,HolidayOTHours = @HolidayOTHours
							   ,Remarks = @Remarks														
							WHERE CAST(TransactionDate AS DATE) = @TransactionDate 
							AND EmployeeCardId = @EmployeeCardId
					END


					IF(@InOutName = 'EmployeeJobCard_Original_NoPenalty')
					BEGIN

							UPDATE [EmployeeInOut_NoPenalty]
							SET Status = @Status
							   ,InTime = @InTime
							   ,OutTime = @OutTime
							   ,LateInMinutes = @LateInMinutes
							   ,OTHours = @OTHours
							   ,ExtraOTHours = @ExtraOTHours
							   ,WeekendOTHours = @WeekendOTHours
							   ,HolidayOTHours = @HolidayOTHours
							   ,Remarks = @Remarks														
							WHERE CAST(TransactionDate AS DATE) = @TransactionDate 
							AND EmployeeCardId = @EmployeeCardId
					END


					IF(@InOutName = 'EmployeeJobCard_Original_NoWeekend')
					BEGIN

							UPDATE [EmployeeInOut_Original_NoWeekend]
							SET Status = @Status
							   ,InTime = @InTime
							   ,OutTime = @OutTime
							   ,LateInMinutes = @LateInMinutes
							   ,OTHours = @OTHours
							   ,ExtraOTHours = @ExtraOTHours
							   ,WeekendOTHours = @WeekendOTHours
							   ,HolidayOTHours = @HolidayOTHours
							   ,Remarks = @Remarks														
							WHERE CAST(TransactionDate AS DATE) = @TransactionDate 
							AND EmployeeCardId = @EmployeeCardId
					END

					DECLARE @Result INT = 1

					IF (@@ERROR <> 0)
						SET @Result = 0
		
					SELECT @Result
					
END
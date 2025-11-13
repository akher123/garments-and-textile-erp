
-- =====================================================================================================================================================
-- Author : Yasir
-- Create date: 2017-04-18
-- Description:	EXEC SPDeleteOverTimeEligibility  '2017-04-18','2017-04-18','0835'
-- =====================================================================================================================================================

CREATE PROCEDURE [dbo].[SPDeleteOverTimeEligibility]

							
								  @FromDate DATETIME = NULL
								 ,@ToDate DATETIME = NULL
								 ,@EmployeeCardId NVARCHAR(20)			

AS
BEGIN	
								SET NOCOUNT ON;
									

								DECLARE @EmployeeId UNIQUEIDENTIFIER
								
								SELECT @EmployeeId = EmployeeId FROM Employee
								WHERE Employee.EmployeeCardId = @EmployeeCardId									
												
								UPDATE OvertimeEligibleEmployee
								SET IsActive = 0
								WHERE CAST(OvertimeDate AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE)
								AND EmployeeId = @EmployeeId													
	
END
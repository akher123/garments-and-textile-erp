-- =========================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <23/02/2016>
-- Description:	<> EXEC Utility_PenaltyOnLeaveDay '2020-09-01', '2020-09-22'
-- =========================================================================

CREATE PROCEDURE [dbo].[Utility_PenaltyOnLeaveDay]
			
									 
						   @FromDate		DATETIME
						  ,@ToDate			DATETIME
						   
AS

BEGIN
	
			SET NOCOUNT ON;
					  				
																	 
						SELECT	   EmployeeLeaveDetail.EmployeeCardId
								  ,ConsumedDate
								  ,HrmPenalty.PenaltyDate
								  ,LeaveTypeTitle   
								  FROM EmployeeLeaveDetail
								  JOIN HrmPenalty ON HrmPenalty.EmployeeId = EmployeeLeaveDetail.EmployeeId 								   
								  AND CAST(HrmPenalty.PenaltyDate AS DATE) = CAST(EmployeeLeaveDetail.ConsumedDate AS DATE)								  								   
								  AND HrmPenalty.IsActive = 1
								  AND HrmPenalty.PenaltyTypeId = 2
								  AND EmployeeLeaveDetail.IsActive = 1
								  WHERE CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) BETWEEN @FromDate AND @ToDate



								  DELETE FROM  HrmPenalty 
								  FROM EmployeeLeaveDetail
								  JOIN HrmPenalty ON HrmPenalty.EmployeeId = EmployeeLeaveDetail.EmployeeId 								   
								  AND CAST(HrmPenalty.PenaltyDate AS DATE) = CAST(EmployeeLeaveDetail.ConsumedDate AS DATE)								 								   
								  AND HrmPenalty.IsActive = 1
								  AND HrmPenalty.PenaltyTypeId = 2
								  AND EmployeeLeaveDetail.IsActive = 1
								  WHERE CAST(EmployeeLeaveDetail.ConsumedDate AS DATE) BETWEEN @FromDate AND @ToDate
													 					  					  														  						  											  							
END
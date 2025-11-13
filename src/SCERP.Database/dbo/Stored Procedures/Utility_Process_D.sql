-- ===============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <12/12/2019>
-- Description:	<> EXEC [Utility_Process_D]  2020, 09, '2020-09-01', '2020-09-30'
-- ===============================================================================

CREATE PROCEDURE [dbo].[Utility_Process_D]
			
							
									@Year			INT		 
								   ,@Month			INT
								   ,@FromDate		DATE
								   ,@ToDate			DATE
																		   
AS

BEGIN
	
			SET NOCOUNT ON;
					  	
					 --- Dyeing Original Process ---		
								
					  --EXEC  SPProcessEmployeeInOut			1, 1, 3, NULL, NULL, NULL, @FromDate, @ToDate, 0, 0, NULL, '', 'superadmin'

					  --EXEC  SPProcessEmployeeJobCard		1, 1, 3, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'

					  EXEC  SPProcessEmployeeSalary			1, 1, 3, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'
			

					 --- Dyeing compliance Inout Process ---

					  EXEC  SPProcessEmployeeInOut_NoPenalty_Dyeing				3, @FromDate, @ToDate, ''

					  EXEC  SPProcessEmployeeInOut_10PM_NoWeekend_Dyeing		3, @FromDate, @ToDate, ''

					  EXEC  SPProcessEmployeeInOut_Original_NoWeekend_Dyeing	3, @FromDate, @ToDate, ''

					  EXEC  SPProcessEmployeeInOutModel2Dyeing 					3, @FromDate, @ToDate, ''


					  --- Dyeing compliance Jobcard Process ---

					  EXEC  SPProcessEmployeeJobCard_NoPenalty 					3, @Year, @Month, @FromDate, @ToDate, ''

					  EXEC  SPProcessEmployeeJobCard_Original_NoWeekend			3, @Year, @Month, @FromDate, @ToDate, ''

					  EXEC [SPProcessEmployeeJobCardModel_Dyeing]				@Year, @Month, @FromDate, @ToDate
					  
					  --EXEC [SPProcessEmployeeJobCard_10PM_NoWeekend]   2019, 11, '2019-10-26','2019-11-25', ''    -- Dyeing need to develop


					  --- Dyeing compliance Salary Process ---
					 
					  EXEC  SPProcessEmployeeSalary_10PM_NoWeekend 		1, 1, 3, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin' 

					  EXEC  SPProcessEmployeeSalaryGrossDeduction		1, 1, 3, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'
													 					  					  														  						  											  							
END 
-- ================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <12/12/2019>
-- Description:	<> EXEC [Utility_Process_GK]  2020, 09, '2020-08-26', '2020-09-25'
-- ================================================================================

CREATE PROCEDURE [dbo].[Utility_Process_GK]
			
							
									@Year			INT		 
								   ,@Month			INT
								   ,@FromDate		DATE
								   ,@ToDate			DATE
																		   
AS

BEGIN
	
			SET NOCOUNT ON;
					  	
					 --- Garments Original Process ---		
								
					 --EXEC  SPProcessEmployeeInOut			1, 1, 1, NULL, NULL, NULL, @FromDate, @ToDate, 0, 0, NULL, '','superadmin'

					 --EXEC  SPProcessEmployeeJobCard	        1, 1, 1, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate,'superadmin'

					 EXEC  SPProcessEmployeeSalary			1, 1, 1, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'


					 --- Knitting Original Process ---		
								
					-- EXEC  SPProcessEmployeeInOut			1, 1, 2, NULL, NULL, NULL, @FromDate, @ToDate, 0, 0, NULL, '','superadmin'

					 --EXEC  SPProcessEmployeeJobCard			1, 1, 2, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate,'superadmin'

					 EXEC  SPProcessEmployeeSalary			1, 1, 2, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'



					 --- Garments/Knitting compliance Inout Process ---

					 EXEC  SPProcessEmployeeInOut_10PM				    @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeInOut_10PM_NoWeekend        @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeInOut_NoPenalty             @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeInOut_Original_NoWeekend    @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeInOutModel2				    @FromDate, @ToDate, ''


					 --- Garments/Knitting compliance Jobcard Process ---

					 EXEC  SPProcessEmployeeJobCard_10PM				@Year, @Month, @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeJobCard_10PM_NoWeekend		@Year, @Month, @FromDate, @ToDate, ''    -- Dyeing need to develop

					 EXEC  SPProcessEmployeeJobCard_GrossDeduction		@Year, @Month, @FromDate, @ToDate, ''     -- Execution Time : 1:30 minutes

					 EXEC  SPProcessEmployeeJobCard_NoPenalty			1, @Year, @Month, @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeJobCard_Original_NoWeekend	1, @Year, @Month, @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeJobCard_NoPenalty			2, @Year, @Month, @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeJobCard_Original_NoWeekend	2, @Year, @Month, @FromDate, @ToDate, ''

					 EXEC  SPProcessEmployeeJobCardModel_GarmentKnitting   @Year, @Month, @FromDate, @ToDate
					  

					 --- Garments/Knitting compliance Salary Process ---
					 
					 EXEC  SPProcessEmployeeSalary_10PM_NoWeekend  1, 1, 1, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin' 

					 EXEC  SPProcessEmployeeSalaryGrossDeduction   1, 1, 1, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin' 

					 EXEC  SPProcessEmployeeSalary_10PM_NoWeekend  1, 1, 2, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin' 

					 EXEC  SPProcessEmployeeSalaryGrossDeduction   1, 1, 2, -1, -1, -1, -1, '', @Year, @Month, @FromDate, @ToDate, 'superadmin'
													 					  					  														  						  											  							
END
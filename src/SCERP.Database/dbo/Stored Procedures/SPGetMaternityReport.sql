
-- ================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <29/02/2018>
-- Description:	<> EXEC SPGetMaternityReport  '0155', '2016-09-10'
-- ================================================================

CREATE PROCEDURE [dbo].[SPGetMaternityReport]
				
							
								       @EmployeeCardId			NVARCHAR(30)
									  ,@LeaveStartDate			DATETIME
																				 
AS

BEGIN
	
	SET NOCOUNT ON;																		
																								 											  					  
							 SELECT TOP(3) 									  
									   CAST(ROW_NUMBER() OVER (ORDER BY [EmployeeCardId]) AS NVARCHAR(1))  AS SerialId
									  ,[EmployeeProcessedSalary].[EmployeeId]
									  ,[EmployeeCardId]
									  ,[Name]
									  ,[NameInBengali]
									  ,[MobileNo]
									  ,[Year]
									  ,[Month]
									  ,[MonthName]
									  ,[FromDate]
									  ,[ToDate]				
									  ,[CompanyName]
									  ,[CompanyNameInBengali]
									  ,[CompanyAddress]
									  ,[CompanyAddressInBengali]				
									  ,[Branch]
									  ,[BranchInBengali]				
									  ,[Unit]
									  ,[UnitInBengali]						
									  ,[Department]
									  ,[DepartmentInBengali]							
									  ,[Section]
									  ,[SectionInBengali]					
									  ,[Line]
									  ,[LineInBengali]						
									  ,[EmployeeType]
									  ,[EmployeeTypeInBengali]							
									  ,[Grade]
									  ,[GradeInBengali]				
									  ,[Designation]
									  ,[DesignationInBengali]												
									  ,FORMAT(JoiningDate, 'dd/MM/yyyy', 'en-us')							 AS [JoiningDate]
									  ,FORMAT(QuitDate, 'dd/MM/yyyy', 'en-us')								 AS [QuitDate]	
									  ,FORMAT(@LeaveStartDate, 'dd/MM/yyyy', 'en-us')						 AS LeaveStartDate	
									  ,FORMAT(DATEADD(DAY, 112, @LeaveStartDate), 'dd/MM/yyyy', 'en-us')	 AS LeaveEndDate																							  														  											  												
									  ,FORMAT(HrmMaternityPayment.FirstPaymentDate, 'dd/MM/yyyy', 'en-us')   AS FirstPaymentDate									  
									  ,CAST(HrmMaternityPayment.FirstPaymentAmount AS NVARCHAR(10))			 AS FirstPaymentAmount 						
									  ,FORMAT(HrmMaternityPayment.SecondPaymentDate, 'dd/MM/yyyy', 'en-us')  AS SecondPaymentDate
									  ,CAST(HrmMaternityPayment.SecondPaymentAmount AS NVARCHAR(10))		 AS SecondPaymentAmount 
									  ,CAST(WorkingDays AS NVARCHAR(10))									 AS WorkingDays																																																
									  ,CAST(GrossSalary AS NVARCHAR(10))									 AS GrossSalary																																					  
									  ,CAST(NetAmount AS NVARCHAR(10))										 AS NetAmount
									  ,CAST((SELECT TOP(1) Amount FROM EmployeeBonus 
									   WHERE EmployeeBonus.EmployeeId = EmployeeProcessedSalary.EmployeeId 
									   AND CAST(EmployeeBonus.EffectiveDate AS DATE) BETWEEN CAST(EmployeeProcessedSalary.FromDate AS DATE) AND CAST(EmployeeProcessedSalary.ToDate AS DATE)) AS NVARCHAR(12)) AS Bonus 
									  ,'' AS TotalNetAmount
									  ,'' AS TotalWorkingDays
									  ,'' AS AverageWages
									  ,'' AS TotalMaternityAmount

							  FROM dbo.EmployeeProcessedSalary
							  LEFT JOIN HrmMaternityPayment ON HrmMaternityPayment.employeeId = EmployeeProcessedSalary.EmployeeId   AND HrmMaternityPayment.IsActive = 1
						
							  WHERE EmployeeProcessedSalary.EmployeeCardId = @EmployeeCardId 
							  AND EmployeeProcessedSalary.FromDate <= DATEADD(Month, -1, @LeaveStartDate) 
							  --AND EmployeeProcessedSalary.QuitDate IS NULL 
							  AND EmployeeProcessedSalary.IsActive = 1
							
							  ORDER BY Year DESC, Month DESC					  					   																																		 													
END
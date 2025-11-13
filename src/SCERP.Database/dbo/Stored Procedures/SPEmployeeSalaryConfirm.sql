-- =============================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <18/04/2015>
-- Description:	<> exec SPEmployeeSalaryConfirm 
-- =============================================

CREATE PROCEDURE [dbo].[SPEmployeeSalaryConfirm]

			@List AS dbo.EmployeeList READONLY
AS
BEGIN
	
		SET NOCOUNT ON;

		BEGIN TRANSACTION SALARYCONFIRM
				
			UPDATE EmployeeSalary_Processed
			SET EmployeeSalary_Processed.IsActive = 0
			FROM EmployeeSalary_Processed_Temp
			WHERE (MONTH(EmployeeSalary_Processed.ToDate) = MONTH(EmployeeSalary_Processed_Temp.ToDate) 
			AND YEAR(EmployeeSalary_Processed.ToDate) = YEAR(EmployeeSalary_Processed_Temp.ToDate))										
			AND EmployeeSalary_Processed.EmployeeId = EmployeeSalary_Processed_Temp.EmployeeId
			AND EmployeeSalary_Processed_Temp.EmployeeId IN(SELECT EmployeeId FROM @List)

			INSERT INTO EmployeeSalary_Processed ( [EmployeeId]
												  ,[FromDate]
												  ,[ToDate]
												  ,[GrossSalary]
												  ,[BasicSalary]
												  ,[HouseRent]
												  ,[MedicalAllowance]
												  ,[Conveyance]
												  ,[FoodAllowance]
												  ,[LWP]
												  ,[Absent]
												  ,[Advance]
												  ,[Stamp]
												  ,[TotalDeduction]
												  ,[AttendanceBonus]
												  ,[ShiftingAllowance]
												  ,[PayableAmount]
												  ,[OTHours]
												  ,[OTRate]
												  ,[OTAmount]
												  ,[NetSalaryPaid]
												  ,[Comments]
												  ,[Tax]
												  ,[ProvidentFund]
												  ,[CreatedDate]
												  ,[CreatedBy]
												  ,[EditedDate]
												  ,[EditedBy]
												  ,[IsActive])							
									SELECT 
												   [EmployeeId]
												  ,[FromDate]
												  ,[ToDate]
												  ,[GrossSalary]
												  ,[BasicSalary]
												  ,[HouseRent]
												  ,[MedicalAllowance]
												  ,[Conveyance]
												  ,[FoodAllowance]
												  ,[LWP]
												  ,[Absent]
												  ,[Advance]
												  ,[Stamp]
												  ,[TotalDeduction]
												  ,[AttendanceBonus]
												  ,[ShiftingAllowance]
												  ,[PayableAmount]
												  ,[OTHours]
												  ,[OTRate]
												  ,[OTAmount]
												  ,[NetSalaryPaid]
												  ,[Comments]
												  ,[Tax]
												  ,[ProvidentFund]
												  ,[CreatedDate]
												  ,[CreatedBy]
												  ,[EditedDate]
												  ,[EditedBy]
												  ,[IsActive]
												FROM EmployeeSalary_Processed_Temp
												WHERE IsActive = 1	
												AND EmployeeSalary_Processed_Temp.EmployeeId IN(SELECT EmployeeId FROM @List)	

			TRUNCATE TABLE EmployeeSalary_Processed_Temp

		COMMIT TRANSACTION SALARYCONFIRM;																																											
END




-- ====================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-09-08>
-- Description:	<> EXEC [SPCheckMerchandiser] '6CF6A835-E639-457E-A0D2-0E6633DAE942'
-- ====================================================================================

CREATE PROCEDURE [dbo].[SPCheckMerchandiser]
						
									
							@EmployeeId		UNIQUEIDENTIFIER		
									

AS

BEGIN
	
	SET NOCOUNT ON;
																
				 IF EXISTS (
							SELECT	Employee.EmployeeCardId																										 					
							FROM	 Employee employee						 
									 LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
									 ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
									 FROM EmployeeCompanyInfo AS employeeCompanyInfo 
									 WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= GETDATE()) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
									 ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1				
									 LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
									 LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
									 LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
									 LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
														  						       								 
									 WHERE employee.IsActive = 1
									 AND Status = 1
									 AND employeeCompanyInfo.BranchUnitDepartmentId = 9	
									 AND employee.employeeId = @EmployeeId
					      )
									BEGIN
											SELECT  1 
									END

									ELSE
									BEGIN
											SELECT  0
									END 
				
END

-- =================================================================================================================================
-- Author:		<Yasir Arafat>
-- Create date: <2018-10-06>
-- Description:	<> EXEC [dbo].[SPEmployeeLeaveSummaryIndividual] '43CB0E34-9226-451C-A9B1-25C78DA35477', '2018-10-12'
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeLeaveSummaryIndividual]

								 
								 @EmployeeId UNIQUEIDENTIFIER,
								 @UpToDate DATETIME = '1900-01-01'
				
AS

BEGIN

						  SELECT [EmployeeCardId]
								,[LeaveTypeTitle]
								,Year
							    ,TotalConsumed
								,[LeaveSetting].NoOfDays AS TotalAllowed
								,[LeaveSetting].NoOfDays - TotalConsumed AS TotalAvailable

								FROM 	

								(SELECT [EmployeeCardId]   							
									  ,[LeaveTypeTitle]
									  ,LeaveTypeId
									  ,YEAR(ConsumedDate ) AS Year
									  ,COUNT(1) AS TotalConsumed
									  ,EmployeeType.Id AS EmployeeTypeId

						        FROM [dbo].[EmployeeLeaveDetail]
								LEFT JOIN
								(SELECT EmployeeId, PunchCardNo, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, 
								ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum						
								FROM EmployeeCompanyInfo AS employeeCompanyInfo
								WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @UpToDate) OR (@UpToDate IS NULL))
								AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo 
								ON [EmployeeLeaveDetail].EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1  
	
								LEFT JOIN EmployeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id AND employeeDesignation.IsActive = 1 
								LEFT JOIN EmployeeType ON employeeDesignation.EmployeeTypeId = employeeType.Id AND employeeType.IsActive = 1 
								WHERE [EmployeeLeaveDetail].EmployeeId = @EmployeeId
								GROUP BY EmployeeCardId, LeaveTypeId, LeaveTypeTitle, YEAR(ConsumedDate), EmployeeType.Id
						
								) AS EmployeeLeave LEFT JOIN LeaveSetting ON LeaveSetting.EmployeeTypeId = EmployeeLeave.EmployeeTypeId 
								AND BranchUnitId = 1 AND LeaveSetting.LeaveTypeId = EmployeeLeave.LeaveTypeId
								ORDER BY Year DESC, LeaveTypeTitle

END
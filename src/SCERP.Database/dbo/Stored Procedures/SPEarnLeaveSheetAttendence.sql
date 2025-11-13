-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <07/09/2016>
-- Description:	<> EXEC SPEarnLeaveSheetAttendence '2018-01-01', '2018-12-31', '1795',-1,-1
-- ==========================================================================================

CREATE PROCEDURE [dbo].[SPEarnLeaveSheetAttendence]
			
		
						         @fromDate				DATETIME=NULL
						        ,@toDate				DATETIME=NULL
								,@EmployeeCardId NVARCHAR(100) = '',
							     @CompanyID INT = -1,
	                             @BranchID INT = -1,
	                             @BranchUnitID INT = -1,
	                             @BranchUnitDepartmentID INT = -1,
	                             @DepartmentSectionId INT = -1,
	                             @DepartmentLineId INT = -1,
	                             @EmployeeTypeID INT = -1,
	                             @EmployeeGradeID INT = -1,
	                             @EmployeeDesignationID INT = -1,
	                             @GenderId INT = NULL,
	                             @EmployeeName NVARCHAR(100) = NULL,
	                             @EmployeeMobilePhone NVARCHAR(100) = NULL,
	                             @EmployeeStatus INT = NULL,
	                             @UserName NVARCHAR(100)=NULL,
	                             @StartRowIndex INT = NULL,
	                             @MaxRows INT = NULL,
	                             @SortField INT = NULL,
	                             @SortDiriection INT = NULL
	                             --,@RowCount INT OUTPUT
								 
								 				

AS
BEGIN
	
							 SET NOCOUNT ON;

							 SELECT 
							 Employee.EmployeeId						AS EmployeeId
							,Employee.EmployeeCardId					AS CardId
						    ,Employee.Name								AS Employeename
							,EmployeeDesignation.Title					AS Designation
							,Department.Name		  					AS DepartmentName
						    ,employeeAtSum.January
							,employeeAtSum.February
							,employeeAtSum.March
							,employeeAtSum.April
							,employeeAtSum.May
							,employeeAtSum.June
							,employeeAtSum.July
							,employeeAtSum.August
							,employeeAtSum.September
							,employeeAtSum.October
							,employeeAtSum.November
							,employeeAtSum.December
							FROM [dbo].Employee
							LEFT JOIN (SELECT EmployeeId, FromDate,ToDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
							ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
							FROM EmployeeCompanyInfo AS employeeCompanyInfo
							WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @fromDate) AND (CAST(employeeCompanyInfo.ToDate AS Date) >= @fromDate) AND (employeeCompanyInfo.IsActive=1))) employeeCompanyInfo 
							ON Employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 
                            LEFT JOIN
							(Select EmployeeCardId,EmployeeId, isnull([1],0) as January, isnull([2],0) as February, isnull([3],0) as March, isnull([4],0) as April, isnull([5],0) as May, isnull([6],0) as June, isnull([7],0) as July, isnull([8],0) as August, isnull([9],0) as September, isnull([10],0) as October, isnull([11],0) as November, isnull([12],0) as December from  ( SELECT     EmployeeCardId,EmployeeId, MONTH(TransactionDate) AS xm, COUNT(*) AS tdays
                            FROM         EmployeeInOut
                            WHERE     (YEAR(TransactionDate) = YEAR(@fromDate)) and IsActive=1 and InTime is not null --and LateInMinutes>=15
                            GROUP BY EmployeeCardId,EmployeeId, MONTH(TransactionDate)  )  D PIVOT (max(tdays)
                            FOR  xm IN ( [1], [2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12] )) piv) employeeAtSum ON employeeAtSum.EmployeeId=Employee.EmployeeId
							LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
							LEFT JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
							LEFT JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
							LEFT JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
							LEFT JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
							LEFT JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
							WHERE  (employee.IsActive = 1)
								AND (employee.[Status] = 1)
								
							
					        AND (employee.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')
							AND (branch.Id=@BranchID OR @BranchID =-1)
					        AND (branchUnit.BranchUnitId=@BranchUnitID OR @BranchUnitID =-1)
							AND (branchUnitDepartment.BranchUnitDepartmentId=@BranchUnitDepartmentID OR @BranchUnitDepartmentID =-1)
							--AND (employeeAtSum.EmployeeCardId = @EmployeeCardId OR @EmployeeCardId ='')
							
					  					  														  						  											  							
END
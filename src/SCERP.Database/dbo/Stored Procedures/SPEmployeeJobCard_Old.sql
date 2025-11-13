
-- ==========================================================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <01/03/2015>
-- Description:	<> exec SPEmployeeJobCard  '05/31/2015','05/31/2015','ae1e9c92-418e-424b-be18-509a58c00f3d'
-- ==========================================================================================================

CREATE PROCEDURE [dbo].[SPEmployeeJobCard_Old]
	   

				@StartDate DATETIME = NULL,
				@EndDate DATETIME = NULL,
				@EmployeeId uniqueidentifier = NULL


AS
BEGIN
	
	SET NOCOUNT ON;


				Declare @FromDate Datetime = Current_timestamp
				EXEC SPGenerateTempDate @StartDate, @EndDate, @EmployeeId;
	  

		SELECT    CONVERT(VARCHAR(10), TempDate.MonthDate, 103)	AS Date
				 ,TempDate.MonthDay	AS DayName
				 ,WorkShift.Name AS Shift
				 ,dbo.fnGetDayStatusOfEmployee(@EmployeeId, TempDate.MonthDate) AS Status		
			     ,CONVERT(VARCHAR(5),dbo.fnGetEmployeeInTime(@EmployeeId, TempDate.MonthDate), 108)  AS InTime
				 ,CONVERT(VARCHAR(5),dbo.fnGetEmployeeOutTime(@EmployeeId, TempDate.MonthDate), 108)  AS OutTime	
				 ,[Delay]=
					CASE
					WHEN  DATEDIFF(minute, DATEADD(day, DATEDIFF(day, 0, dbo.fnGetEmployeeInTime(@EmployeeId, TempDate.MonthDate)), CONVERT(varchar(7), WorkShift.StartTime, 20)), DATEADD(minute, -WorkShift.InBufferTime, dbo.fnGetEmployeeInTime(@EmployeeId, TempDate.MonthDate))) < 0 THEN '0'
					ELSE DATEDIFF(minute, DATEADD(day, DATEDIFF(day, 0, dbo.fnGetEmployeeInTime(@EmployeeId, TempDate.MonthDate)), CONVERT(varchar(7), WorkShift.StartTime, 20)), DATEADD(minute, -WorkShift.InBufferTime, dbo.fnGetEmployeeInTime(@EmployeeId, TempDate.MonthDate)))				
					END				
				 ,dbo.fnGetOTHours(@EmployeeId, TempDate.MonthDate ) AS [OTHours]
				 ,dbo.fnGetDayRemarksOfEmployee(@EmployeeId, TempDate.MonthDate) AS Remarks						 	
				 ,Company.Name AS CompanyName
				 ,CONVERT(VARCHAR(10), GETDATE(), 103) AS DateToday
				 ,Employee.Name AS Name
				 ,Employee.EmployeeCardId AS CardId
				 ,Employee.EmployeeId AS EmployeeId
				 ,EmployeeDesignation.Title AS Designation
				 ,CONVERT(NVARCHAR(20),EmployeeSalary.GrossSalary) AS GrossSalary
				 ,Department.Name AS Department
				 ,Section.Name AS Section
				 ,CONVERT(VARCHAR(10),Employee.JoiningDate, 103) AS JoiningDate
				 ,EmployeePresentAddress.MobilePhone AS MobileNo
				 ,CONVERT(NVARCHAR(20), DATEDIFF(DAY, @StartDate, @EndDate) + 1) AS TotalDays	
				 ,CONVERT(NVARCHAR(20), dbo.fnGetPresentDays(Employee.EmployeeId, @StartDate, @EndDate))  AS PresentDays
				 ,CONVERT(NVARCHAR(20), dbo.fnGetAbsentDays(Employee.EmployeeId, @StartDate, @EndDate)) AS AbsentDays
				 ,CONVERT(NVARCHAR(20), dbo.fnGetWeekend(Employee.EmployeeId,@StartDate, @EndDate)) AS WeekendDays
				 ,CONVERT(NVARCHAR(20), dbo.fnGetHolidays(Employee.EmployeeId,@StartDate, @EndDate)) AS Holidays
				 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLeave(Employee.EmployeeId, @StartDate, @EndDate)) AS LeaveDays
				 ,'0' AS LWP
				 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalLateDays(Employee.EmployeeId, @StartDate, @EndDate))  AS LateDays
				 ,CONVERT(NVARCHAR(20), dbo.fnGetTotalOTHours(Employee.EmployeeId, @StartDate, @EndDate)) AS OTHourLast
				 				 		
		 FROM   TempDate LEFT OUTER JOIN
		 			
				EmployeeWorkShift ON EmployeeWorkShift.EmployeeId = TempDate.EmployeeId AND CAST(EmployeeWorkShift.ShiftDate AS date) = CAST(TempDate.MonthDate AS date) LEFT OUTER JOIN
				BranchUnitWorkShift ON EmployeeWorkShift.BranchUnitWorkShiftId = BranchUnitWorkShift.BranchUnitWorkShiftId AND BranchUnitWorkShift.IsActive = 1 LEFT OUTER JOIN
				WorkShift ON WorkShift.WorkShiftId = BranchUnitWorkShift.WorkShiftId LEFT OUTER JOIN
				Employee AS  employee ON Employee.EmployeeId = TempDate.EmployeeId

				LEFT JOIN (SELECT EmployeeId, FromDate,DesignationId,BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId,
				ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
				FROM EmployeeCompanyInfo AS employeeCompanyInfo 
				WHERE ((CAST(employeeCompanyInfo.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND employeeCompanyInfo.IsActive=1) employeeCompanyInfo 
				ON employee.EmployeeId = employeeCompanyInfo.EmployeeId

				LEFT JOIN EmployeePresentAddress AS employeePresentAddress  ON employee.EmployeeId = employeePresentAddress.EmployeeId
				LEFT JOIN District DIS ON employeePresentAddress.DistrictId = DIS.Id 
				LEFT OUTER JOIN PoliceStation PST ON employeePresentAddress.PoliceStationId = PST.Id 
				LEFT JOIN EmployeePermanentAddress AS EmployeePermanentAddress ON employee.EmployeeId = EmployeePermanentAddress.EmployeeId						
				LEFT JOIN District DIST ON EmployeePermanentAddress.DistrictId = DIST.Id 
				LEFT OUTER JOIN PoliceStation PSTE ON EmployeePermanentAddress.PoliceStationId = PSTE.Id 
				INNER JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId=employeeDesignation.Id
				INNER JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id
				INNER JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id
				INNER JOIN BranchUnitDepartment  AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId
				INNER JOIN BranchUnit  AS branchUnit ON branchUnitDepartment.BranchUnitId=branchUnit.BranchUnitId
				INNER JOIN UnitDepartment  AS unitDepartment ON branchUnitDepartment.UnitDepartmentId=unitDepartment.UnitDepartmentId
				INNER JOIN Unit  AS unit ON branchUnit.UnitId=unit.UnitId
				INNER JOIN Department  AS department ON unitDepartment.DepartmentId=department.Id
				INNER JOIN Branch  AS branch ON branchUnit.BranchId=branch.Id
				INNER JOIN Company  AS company ON branch.CompanyId=company.Id	
									
				LEFT JOIN (SELECT EmployeeId, FromDate, BasicSalary, HouseRent, MedicalAllowance, Conveyance, FoodAllowance, GrossSalary,
				ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNumSalary
				FROM EmployeeSalary AS EmployeeSalary 
				WHERE ((CAST(EmployeeSalary.FromDate AS Date) <= @FromDate) OR (@FromDate IS NULL)) AND EmployeeSalary.IsActive=1) EmployeeSalary 
				ON employee.EmployeeId = EmployeeSalary.EmployeeId AND EmployeeSalary.rowNumSalary = 1

				LEFT JOIN DepartmentSection departmentSection on employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId
				LEFT JOIN Section section on departmentSection.SectionId = section.SectionId
				LEFT JOIN DepartmentLine departmentLine on employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId
				LEFT JOIN Line line on departmentLine.LineId = line.LineId

				WHERE 
				--employee.IsActive = 1 AND employee.[Status] = 1 AND
				employeeCompanyInfo.rowNum = 1  AND employee.EmployeeId = @employeeId
				AND (employeePresentAddress.[Status] = 1 AND employeePresentAddress.IsActive = 1) 
				ORDER BY EmployeeCardId ASC				
END






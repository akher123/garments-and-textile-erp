


CREATE VIEW [dbo].[VEmployee]
AS
SELECT		employee.[Status], 
			employee.EmployeeId, 
			employee.EmployeeCardId, 
			employee.Name, 
			employee.NameInBengali,
			gender.GenderId AS GenderId, 
			gender.Title AS Gender, 
			employee.JoiningDate AS JoiningDate, 
			employee.ConfirmationDate AS ConfirmationDate, 
			employee.DateOfBirth AS DateOfBirth, 
			employee.QuitDate, 
			employee.IsActive, 
			company.Id AS CompanyId,
			company.Name AS CompanyName,
			company.FullAddress AS CompanyAddress,
			branch.Id AS BranchId,
			branch.Name AS BranchName,
			branchUnit.BranchUnitId AS BranchUnitId,
			unit.Name AS UnitName,
			employeeCompanyInfo.BranchUnitDepartmentId AS DeptID, 
			ISNULL(department.Name, '-') AS Department, 
			employeeCompanyInfo.DepartmentSectionId AS SecID, 
			ISNULL(section.Name, '-') AS Section,
			ISNULL(employeeCompanyInfo.DepartmentLineId, 0) AS LineID, 
			ISNULL(line.Name, '-') AS Line,
			employeeType.Id AS EmployeeTypeId,
			employeeType.Title AS EmployeeType, 
			employeeGrade.Id AS EmployeeGradeId, 
			employeeGrade.Name AS Grade, 
			employeeCompanyInfo.DesignationId AS DesID, 
			employeeDesignation.Title AS Designation,
			ISNULL(employeeSalary.GrossSalary, 0) AS GrossSalary, 
			ISNULL(employeeSalary.BasicSalary, 0) AS BasicSalary, 
			employeeCompanyInfo.IsEligibleForOvertime AS OTStatus 				
FROM        dbo.Employee AS employee
			LEFT JOIN
            (SELECT EmployeeId, 
					FromDate, 
				    DesignationId, 
					BranchUnitDepartmentId, 
					DepartmentSectionId, 
					DepartmentLineId, 
					IsEligibleForOvertime, 
					ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
			FROM    EmployeeCompanyInfo 
			WHERE   ((CAST(FromDate AS Date) <= CURRENT_TIMESTAMP) AND (IsActive = 1))) employeeCompanyInfo 
					ON employee.EmployeeId = employeeCompanyInfo.EmployeeId AND employeeCompanyInfo.rowNum = 1 
LEFT JOIN BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId 
LEFT JOIN BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId 
LEFT JOIN UnitDepartment AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId 
LEFT JOIN Unit AS unit ON branchUnit.UnitId = unit.UnitId 
LEFT JOIN Department AS department ON unitDepartment.DepartmentId = department.Id 
LEFT JOIN Branch AS branch ON branchUnit.BranchId = branch.Id 
LEFT JOIN Company AS company ON branch.CompanyId = company.Id 
LEFT JOIN DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId 
LEFT JOIN Section section ON departmentSection.SectionId = section.SectionId 
LEFT JOIN DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId 
LEFT JOIN Line line ON departmentLine.LineId = line.LineId 
LEFT JOIN EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id 
LEFT JOIN EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id 
LEFT JOIN EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id 
LEFT JOIN (SELECT EmployeeId, 
				  BasicSalary, 
				  GrossSalary, 
				  ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY FromDate DESC) AS rowNum
		  FROM    EmployeeSalary 
		  WHERE   (CAST(FromDate AS Date) <= CURRENT_TIMESTAMP) AND IsActive = 1) employeeSalary 
		  ON      employee.EmployeeId = employeeSalary.EmployeeId AND employeeSalary.rowNum = 1 
INNER JOIN Gender gender ON employee.GenderId = gender.GenderId





GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[20] 4[10] 2[22] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 13
         Width = 284
         Width = 750
         Width = 1380
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 780
         Width = 705
         Width = 1995
         Width = 645
         Width = 630
         Width = 900
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VEmployee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VEmployee';


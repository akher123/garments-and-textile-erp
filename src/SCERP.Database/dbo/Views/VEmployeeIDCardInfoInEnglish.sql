CREATE VIEW dbo.VEmployeeIDCardInfoInEnglish
AS
SELECT        Employee.EmployeeId, Employee.EmployeeCardId AS CardId, Employee.EmployeeCardId AS EmployeeCardId, Employee.Name AS Name, Company.Name AS CompanyName, Branch.Name AS Branch, Unit.Name AS Unit, 
                         Department.Name AS Department, Section.Name AS Section, Line.Name AS Line, EmployeeDesignation.Title AS Designation, EmployeeGrade.Name AS Grade, skillSet.Title AS EmployeeJobType, CONVERT(VARCHAR(20), 
                         Employee.JoiningDate, 103) JoiningDate, BranchUnitDepartment.BranchUnitDepartmentId AS BranchUnitDepartmentId, employeeCompanyInfo.DepartmentSectionId AS DepartmentSectionId, 
                         employeeCompanyInfo.DepartmentLineId AS DepartmentLineId, company.Id AS CompanyId, branch.Id AS BranchId, Employee.PhotographPath, employeeType.Id AS EmployeeTypeId, BloodGroup.GroupName AS BloodGroup, 
                         employee.FathersName, EmployeePermanentAddress.MailingAddress, EmployeePermanentAddress.PostOffice, PoliceStation.Name AS PoliceStation, District.Name AS DistrictName, 
                         EmployeePresentAddress.ContactPersonPhone AS EmergencyPhone, employee.NationalIdNo, employee.BirthRegistrationNo
FROM            Employee AS employee LEFT JOIN
                             (SELECT        EmployeeId, FromDate, DesignationId, BranchUnitDepartmentId, DepartmentSectionId, DepartmentLineId, JobTypeId, ROW_NUMBER() OVER (PARTITION BY EmployeeId
                               ORDER BY FromDate DESC) AS rowNum
FROM            EmployeeCompanyInfo AS employeeCompanyInfo
WHERE        (CAST(employeeCompanyInfo.FromDate AS Date) <= CURRENT_TIMESTAMP) AND employeeCompanyInfo.IsActive = 1) employeeCompanyInfo ON employee.EmployeeId = employeeCompanyInfo.EmployeeId INNER JOIN
EmployeeDesignation AS employeeDesignation ON employeeCompanyInfo.DesignationId = employeeDesignation.Id INNER JOIN
EmployeeGrade AS employeeGrade ON employeeDesignation.GradeId = employeeGrade.Id INNER JOIN
EmployeeType AS employeeType ON employeeGrade.EmployeeTypeId = employeeType.Id INNER JOIN
BranchUnitDepartment AS branchUnitDepartment ON employeeCompanyInfo.BranchUnitDepartmentId = branchUnitDepartment.BranchUnitDepartmentId INNER JOIN
BranchUnit AS branchUnit ON branchUnitDepartment.BranchUnitId = branchUnit.BranchUnitId INNER JOIN
UnitDepartment AS unitDepartment ON branchUnitDepartment.UnitDepartmentId = unitDepartment.UnitDepartmentId INNER JOIN
Unit AS unit ON branchUnit.UnitId = unit.UnitId INNER JOIN
Department AS department ON unitDepartment.DepartmentId = department.Id INNER JOIN
Branch AS branch ON branchUnit.BranchId = branch.Id INNER JOIN
Company AS company ON branch.CompanyId = company.Id LEFT JOIN
BloodGroup ON BloodGroup.Id = employee.BloodGroupId LEFT JOIN
EmployeePermanentAddress ON EmployeePermanentAddress.EmployeeId = employee.EmployeeId LEFT JOIN
PoliceStation ON PoliceStation.Id = EmployeePermanentAddress.PoliceStationId LEFT JOIN
District ON District.Id = EmployeePermanentAddress.DistrictId LEFT JOIN
EmployeePresentAddress ON EmployeePresentAddress.EmployeeId = employee.EmployeeId LEFT JOIN
DepartmentSection departmentSection ON employeeCompanyInfo.DepartmentSectionId = departmentSection.DepartmentSectionId LEFT JOIN
Section section ON departmentSection.SectionId = section.SectionId LEFT JOIN
DepartmentLine departmentLine ON employeeCompanyInfo.DepartmentLineId = departmentLine.DepartmentLineId LEFT JOIN
Line line ON departmentLine.LineId = line.LineId LEFT JOIN
SkillSet skillSet ON employeeCompanyInfo.JobTypeId = skillSet.Id
WHERE        employee.IsActive = 1 /*AND employee.[Status]=1 */ AND employeeCompanyInfo.rowNum = 1

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
      Begin ColumnWidths = 31
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VEmployeeIDCardInfoInEnglish';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VEmployeeIDCardInfoInEnglish';


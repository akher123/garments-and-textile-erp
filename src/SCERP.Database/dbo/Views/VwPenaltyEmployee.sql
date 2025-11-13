CREATE VIEW dbo.VwPenaltyEmployee
AS
SELECT        PL.PenaltyId, PL.EmployeeId, PL.EmployeeCardId, CASE PL.PenaltyTypeId WHEN 1 THEN CONVERT(NVARCHAR(50), PL.Penalty) + ' Hour(s)' WHEN 2 THEN CONVERT(NVARCHAR(50), PL.Penalty) 
                         + ' Day(s)' WHEN 3 THEN CONVERT(NVARCHAR(50), PL.Penalty) + ' Day(s)' WHEN 4 THEN CONVERT(NVARCHAR(50), PL.Penalty) + ' BDT' ELSE CONVERT(NVARCHAR(50), PL.Penalty) END AS Penalty, 
                         PT.Type AS PenaltyType, PL.PenaltyDate, PL.Reason, PL.ClaimerId, EM1.Name AS EmployeeName, EM2.Name AS ClaimerName, PL.IsActive
FROM            dbo.HrmPenalty AS PL INNER JOIN
                         dbo.HrmPenaltyType AS PT ON PL.PenaltyTypeId = PT.PenaltyTypeId INNER JOIN
                         dbo.Employee AS EM1 ON PL.EmployeeId = EM1.EmployeeId INNER JOIN
                         dbo.Employee AS EM2 ON PL.ClaimerId = EM2.EmployeeId
WHERE        (PL.IsActive = 1) AND (PT.IsActive = 1)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[11] 2[20] 3) )"
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
         Begin Table = "PL"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PT"
            Begin Extent = 
               Top = 6
               Left = 252
               Bottom = 136
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EM1"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "EM2"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
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
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwPenaltyEmployee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'       Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwPenaltyEmployee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwPenaltyEmployee';


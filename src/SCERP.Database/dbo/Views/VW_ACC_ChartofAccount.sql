CREATE VIEW [dbo].[VW_ACC_ChartofAccount]
AS
SELECT        A_4.ClsID, A_4.ClsParentCode, A_4.ClsControlCode, A_4.ClsControlName, A_4.GrpID, A_4.GrpParentCode, A_4.GrpControlCode, A_4.GrpControlName, A_4.SubGrpID, 
                         A_4.SubGrpParentCode, A_4.SubGrpControlCode, A_4.SubGrpControlName, A_4.ControlCodeId, A_4.ControlParentCode, A_4.ControlContolCode, 
                         A_4.ControlCodeName, B_1.GLCode, B_1.GLParentCode, B_1.AccountCode, B_1.AccountName, B_1.BalanceType, B_1.AccountType
FROM            (SELECT        A.ClsID, A.ClsParentCode, A.ClsControlCode, A.ClsControlName, A.GrpID, A.GrpParentCode, A.GrpControlCode, A.GrpControlName, A.SubGrpID, 
                                                    A.SubGrpParentCode, A.SubGrpControlCode, A.SubGrpControlName, B.ControlCodeId, B.ControlParentCode, B.ControlContolCode, 
                                                    B.ControlCodeName
                          FROM            (SELECT        A.ClsID, A.ClsParentCode, A.ClsControlCode, A.ClsControlName, A.GrpID, A.GrpParentCode, A.GrpControlCode, A.GrpControlName, 
                                                                              B.SubGrpID, B.SubGrpParentCode, B.SubGrpControlCode, B.SubGrpControlName
                                                    FROM            (SELECT        A.ClsID, A.ClsParentCode, A.ClsControlCode, A.ClsControlName, B.GrpID, B.GrpParentCode, B.GrpControlCode, 
                                                                                                        B.GrpControlName
                                                                              FROM            (SELECT        Id AS ClsID, ParentCode AS ClsParentCode, ControlCode AS ClsControlCode, ControlName AS ClsControlName
                                                                                                        FROM            dbo.Acc_ControlAccounts
                                                                                                        WHERE        (ControlLevel = 1)) AS A LEFT OUTER JOIN
                                                                                                            (SELECT        Id AS GrpID, ParentCode AS GrpParentCode, ControlCode AS GrpControlCode, 
                                                                                                                                        ControlName AS GrpControlName
                                                                                                              FROM            dbo.Acc_ControlAccounts AS Acc_ControlAccounts_3
                                                                                                              WHERE        (ControlLevel = 2)) AS B ON A.ClsControlCode = B.GrpParentCode) AS A LEFT OUTER JOIN
                                                                                  (SELECT        Id AS SubGrpID, ParentCode AS SubGrpParentCode, ControlCode AS SubGrpControlCode, 
                                                                                                              ControlName AS SubGrpControlName
                                                                                    FROM            dbo.Acc_ControlAccounts AS Acc_ControlAccounts_2
                                                                                    WHERE        (ControlLevel = 3)) AS B ON A.GrpControlCode = B.SubGrpParentCode) AS A LEFT OUTER JOIN
                                                        (SELECT        Id AS ControlCodeId, ParentCode AS ControlParentCode, ControlCode AS ControlContolCode, ControlName AS ControlCodeName
                                                          FROM            dbo.Acc_ControlAccounts AS Acc_ControlAccounts_1
                                                          WHERE        (ControlLevel = 4)) AS B ON A.SubGrpControlCode = B.ControlParentCode) AS A_4 LEFT OUTER JOIN
                             (SELECT        Id AS GLCode, ControlCode AS GLParentCode, AccountCode, AccountName, BalanceType, AccountType
                               FROM            dbo.Acc_GLAccounts AS A) AS B_1 ON A_4.ControlContolCode = B_1.GLParentCode





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
         Begin Table = "A_4"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "B_1"
            Begin Extent = 
               Top = 6
               Left = 276
               Bottom = 135
               Right = 446
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_ACC_ChartofAccount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_ACC_ChartofAccount';


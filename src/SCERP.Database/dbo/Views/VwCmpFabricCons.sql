CREATE VIEW dbo.VwCmpFabricCons
AS
SELECT     C.OrderStyleRefId, C.ConsumptionId, C.CompId, C.ConsRefId, C.Quantity, C.ItemCode, I.ItemName, CC.ComponentRefId, CC.ComponentSlNo, CD.GColorRefId, 
                      CD.GSizeRefId, CD.PColorRefId, CD.PPQty, CD.Weight, CD.PAllow, CD.TQty, ISNULL(CD.ProcessLoss, 0) AS PL, CD.TQty / (1 - ISNULL(CD.ProcessLoss, 0) * 0.01) 
                      AS GreyFabQty
FROM         dbo.OM_Consumption AS C INNER JOIN
                      dbo.Inventory_Item AS I ON C.CompId = I.CompId AND C.ItemCode = I.ItemCode INNER JOIN
                      dbo.OM_CompConsumption AS CC ON C.CompId = CC.CompId AND C.OrderStyleRefId = CC.OrderStyleRefId AND C.ConsRefId = CC.ConsRefId INNER JOIN
                      dbo.OM_CompConsumptionDetail AS CD ON CC.CompId = CD.CompID AND CC.OrderStyleRefId = CD.OrderStyleRefId AND CC.ConsRefId = CD.ConsRefId AND 
                      CC.ComponentSlNo = CD.CompomentSlNo
WHERE     (C.CompId = '001') AND (C.ItemCode LIKE '10%')

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[28] 4[11] 2[27] 3) )"
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
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 197
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "I"
            Begin Extent = 
               Top = 6
               Left = 235
               Bottom = 114
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CC"
            Begin Extent = 
               Top = 6
               Left = 449
               Bottom = 114
               Right = 628
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CD"
            Begin Extent = 
               Top = 6
               Left = 666
               Bottom = 114
               Right = 872
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
      Begin ColumnWidths = 19
         Width = 284
         Width = 1500
         Width = 1500
         Width = 750
         Width = 1050
         Width = 990
         Width = 900
         Width = 1500
         Width = 1500
         Width = 1335
         Width = 1080
         Width = 990
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
         Append = 1400', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwCmpFabricCons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwCmpFabricCons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VwCmpFabricCons';


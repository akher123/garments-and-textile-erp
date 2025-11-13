


CREATE VIEW [dbo].[VOM_BuyOrdStyle]
AS
SELECT   S.OrderStyleId,s.TnaMode,  O.CompId, O.OrderNo, O.RefNo, O.BuyerRefId,S.Rate,S.Amt, S.EFD, ISNULL
                          ((SELECT     TOP (1) BuyerName
                              FROM         dbo.OM_Buyer
                              WHERE     (CompId = O.CompId) AND (BuyerRefId = O.BuyerRefId)), '-') AS BuyerName, O.MerchandiserId, ISNULL
                          ((SELECT     TOP (1) EmpName
                              FROM         dbo.OM_Merchandiser
                              WHERE     (CompId = O.CompId) AND (EmpId = O.MerchandiserId)), '-') AS Merchandiser, O.BuyerRef, S.OrderStyleRefId, S.StyleRefId, ISNULL
                          ((SELECT     TOP (1) StyleName
                              FROM         dbo.OM_Style
                              WHERE     (CompID = S.CompId) AND (StylerefId = S.StyleRefId)), '-') AS StyleName, S.BuyerArt, S.Quantity,S.ActiveStatus,O.Closed
FROM         dbo.OM_BuyOrdStyle AS S INNER JOIN
                      dbo.OM_BuyerOrder AS O ON S.CompId = O.CompId AND S.OrderNo = O.OrderNo




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[33] 4[14] 2[32] 3) )"
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
         Begin Table = "S"
            Begin Extent = 
               Top = 14
               Left = 445
               Bottom = 209
               Right = 604
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "O"
            Begin Extent = 
               Top = 6
               Left = 235
               Bottom = 196
               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 4
         End
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
         Width = 1260
         Width = 1260
         Width = 1020
         Width = 1500
         Width = 1305
         Width = 1290
         Width = 1110
         Width = 1380
         Width = 960
         Width = 1155
         Width = 1260
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VOM_BuyOrdStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VOM_BuyOrdStyle';


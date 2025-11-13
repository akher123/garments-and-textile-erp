
CREATE VIEW [dbo].[VBuyerOrder]
AS
SELECT        BuyerOrderId, CompId, OrderNo, OrderDate, RefNo, RefDate, ISNULL(SampleOrdNo, '-') AS SampleOrdeNo,Closed,CloseDate, BuyerRefId, ISNULL
                             ((SELECT        TOP (1) BuyerName
                                 FROM            dbo.OM_Buyer
                                 WHERE        (CompId = O.CompId) AND (BuyerRefId = O.BuyerRefId)), '-') AS BuyerName, DGRefNo, BuyerRef, Quantity, MerchandiserId, ISNULL
                             ((SELECT        TOP (1) EmpName
                                 FROM            dbo.OM_Merchandiser
                                 WHERE        (CompId = O.CompId) AND (EmpId = O.MerchandiserId)), '-') AS EmpName, AgentRefId, ISNULL
                             ((SELECT        TOP (1) AgentName
                                 FROM            dbo.OM_Agent
                                 WHERE        (CompId = O.CompId) AND (AgentRefId = O.AgentRefId) AND (AType = 'B')), '-') AS AgentName, ShipagentRefId, ISNULL
                             ((SELECT        TOP (1) AgentName
                                 FROM            dbo.OM_Agent AS OM_Agent_1
                                 WHERE        (CompId = O.CompId) AND (AgentRefId = O.AgentRefId) AND (AType = 'S')), '-') AS SAgentName, ConsigneeRefId, ISNULL
                             ((SELECT        TOP (1) ConsigneeName
                                 FROM            dbo.OM_Consignee
                                 WHERE        (CompId = O.CompId) AND (ConsigneeRefId = O.ConsigneeRefId)), '-') AS ConsigneeName, PayTermRefId, ISNULL
                             ((SELECT        TOP (1) PayTerm
                                 FROM            dbo.OM_PaymentTerm
                                 WHERE        (CompId = O.CompId) AND (PayTermRefId = O.PayTermRefId)), '-') AS PayTerm, OrderTypeRefId, ISNULL
                             ((SELECT        TOP (1) OTypeName
                                 FROM            dbo.OM_OrderType
                                 WHERE        (OTypeRefId = O.OrderTypeRefId)), '-') AS OTypeName, SMode, PaymentModeRefId, OAmount, Fab, SCont, SeasonRefId, ISNULL
                             ((SELECT        TOP (1) SeasonName
                                 FROM            dbo.OM_Season
                                 WHERE        (CompId = O.CompId) AND (SeasonRefId = O.SeasonRefId)), '-') AS SeasonName, ISNULL
                             ((SELECT        COUNT(*) AS Expr1
                                 FROM            dbo.OM_BuyOrdStyle
                                 WHERE        (CompId = O.CompId) AND (OrderNo = O.OrderNo)), '-') AS TotalStyleInOrder,
                             (SELECT        MAX(ShipDate) AS Expr1
                               FROM            dbo.OM_BuyOrdShip
                               WHERE        (CompId = O.CompId) AND (OrderNo = O.OrderNo)) AS ShipDate, ISNULL
                             ((SELECT        SUM(despatchQty) AS Expr1
                                 FROM            dbo.OM_BuyOrdStyle AS BOS
                                 WHERE        (O.CompId = CompId) AND (O.OrderNo = OrderNo)), 0) AS ShipQty,
                             (SELECT        TOP (1) LcNo
                               FROM            dbo.COMMLcInfo
                               WHERE        (O.CompId = O.CompId) AND (LcId = O.LcRefId)) AS LcNo, LcRefId
FROM            dbo.OM_BuyerOrder AS O


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[4] 2[4] 3) )"
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
         Begin Table = "O"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 232
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
      Begin ColumnWidths = 9
         Width = 284
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VBuyerOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VBuyerOrder';


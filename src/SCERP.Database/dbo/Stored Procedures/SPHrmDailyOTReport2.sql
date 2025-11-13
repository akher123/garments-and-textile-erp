-- ==============================================================================
-- Author:		<Md.Yasir Arafat>
-- Create date: <02/11/2016>
-- Description:	<> EXEC SPHrmDailyOTReport2 '2017-01-01'
-- ==============================================================================

CREATE PROCEDURE [dbo].[SPHrmDailyOTReport2]
			

						   @fromDate DATETIME = '2016-09-26'				

AS
BEGIN
	
			SET NOCOUNT ON;
				

	SELECT n.[LineName],ISNULL(a.EmpNumber,0) as "1",ISNULL(a2.EmpNumber,0) as "2",ISNULL(b.EmpNumber,0) as "3"
   ,ISNULL(c.EmpNumber,0) as "4",ISNULL(d.EmpNumber,0) as "5",ISNULL(e.EmpNumber,0) as "6",ISNULL(f.EmpNumber,0) as "7"
   ,ISNULL(g.EmpNumber,0) as "8",ISNULL(h.EmpNumber,0) as "9",ISNULL(i.EmpNumber,0) as "10",ISNULL(j.EmpNumber,0) as "11"
   ,ISNULL(k.EmpNumber,0) as "12",ISNULL(l.EmpNumber,0) as "13",ISNULL(m.EmpNumber,0) as "14",ISNULL(n15.EmpNumber,0) as "15"
  , (ISNULL(a.EmpNumber,0)+ISNULL(a2.EmpNumber,0)+ISNULL(b.EmpNumber,0)+ISNULL(c.EmpNumber,0)+ISNULL(d.EmpNumber,0)+ISNULL(e.EmpNumber,0)+ISNULL(f.EmpNumber,0)+ISNULL(f.EmpNumber,0)+ISNULL(g.EmpNumber,0)+ISNULL(h.EmpNumber,0)+ISNULL(i.EmpNumber,0)+ISNULL(j.EmpNumber,0)+ISNULL(k.EmpNumber,0)+ISNULL(l.EmpNumber,0)+ISNULL(m.EmpNumber,0)+ISNULL(n15.EmpNumber,0)) as TotalMan from
      (select [LineName]
      
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and LineName IS NOT NULL group by LineName) n
	LEFT JOIN  (select [LineName]
       ,SUM([OTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and OTHours=1 and LineName IS NOT NULL group by LineName) a on a.LineName=n.LineName
  LEFT JOIN (select [LineName]
       ,SUM([OTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and OTHours=2 and LineName IS NOT NULL group by LineName) a2 on a2.LineName=n.LineName
  LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=1 and LineName IS NOT NULL group by LineName) b on b.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=2 and LineName IS NOT NULL group by LineName) c on c.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=3 and LineName IS NOT NULL group by LineName) d on d.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=4 and LineName IS NOT NULL group by LineName) e on e.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=5 and LineName IS NOT NULL group by LineName) f on f.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=6 and LineName IS NOT NULL group by LineName) g on g.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=7 and LineName IS NOT NULL group by LineName) h on h.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=8 and LineName IS NOT NULL group by LineName) i on i.LineName=n.LineName
   LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=9 and LineName IS NOT NULL group by LineName) j on j.LineName=n.LineName
  LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=10 and LineName IS NOT NULL group by LineName) k on k.LineName=n.LineName 
  LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=11 and LineName IS NOT NULL group by LineName) l on l.LineName=n.LineName
  LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=12 and LineName IS NOT NULL group by LineName) m on m.LineName=n.LineName
  LEFT JOIN (select [LineName]
       ,SUM([ExtraOTHours]) AS OT
      ,COUNT([EmployeeId]) AS EmpNumber
  FROM [SCERPDB_PILOT].[dbo].[EmployeeInOut]  WHERE CAST(TransactionDate AS DATE) = @fromDate and ExtraOTHours=13 and LineName IS NOT NULL group by LineName) n15 on n15.LineName=n.LineName ORDER BY n.[LineName]	  														  						  											  							
END



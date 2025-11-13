create procedure [dbo].[SpProdLineWiseDailySewingProduction]
@CurrentMonth as varchar(20),
@CurrentYear int,
@CompId varchar(3)
as
SELECT
(select top(1) Name from Production_Machine where MachineId=LineId and IsActive=1 order by MachineId )
    Line,
	
   ISNULL([1],0) as [1] ,  ISNULL([2],0) as [2] ,ISNULL([3],0) as  [3] ,ISNULL([4],0) as  [4] ,ISNULL([5],0) as  [5] , ISNULL([6],0) as [6] , ISNULL([7],0) as [7] , ISNULL([8],0) as[8] , ISNULL([9],0) as[9] ,ISNULL([10],0) as [10],ISNULL([11],0) as [11], ISNULL([12],0) as [12], ISNULL([13],0) as[13] ,  ISNULL([14],0) as [14] ,ISNULL([15],0) as [15] ,ISNULL([16],0) as [16] , ISNULL([17],0) as [17] , ISNULL([18],0) as [18] ,ISNULL([19],0) as [19] ,ISNULL([20],0) as [20] ,ISNULL([21],0) as [21] ,ISNULL([22],0) as [22],ISNULL([23],0) as [23],ISNULL([24],0) as [24],ISNULL([25],0) as [25],ISNULL([26],0) as [26],ISNULL([27],0) as  [27],ISNULL([28],0) as  [28],ISNULL([29],0) as[29],ISNULL([30],0) as [30],ISNULL([31],0) as [31]

FROM
(Select 
S.LineId,S.Qty,Day(S.OutputDate) as TDay from
    (    select SO.LineId ,SUM(ISNULL(SOD.Quantity,0)) as Qty,  SO.OutputDate  from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where year(SO.OutputDate)=2017 and  DATENAME(month, SO.OutputDate)=@CurrentMonth and SO.CompId=@CompId
group by SO.LineId ,SO.OutputDate
) as S )  source
PIVOT
(
    SUM(Qty)
    FOR TDay
    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] ,[13] ,   [14] , [15] , [16] , [17] , [18] , [19] , [20] , [21] ,[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])
) AS pvtMonth

order by pvtMonth.LineId
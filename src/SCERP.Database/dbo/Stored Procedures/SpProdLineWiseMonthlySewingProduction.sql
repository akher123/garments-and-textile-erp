CREATE procedure [dbo].[SpProdLineWiseMonthlySewingProduction]
@CurrntYear as int,
@CompId varchar(3)
as 

SELECT
LineId ,
(select top(1) Name from Production_Machine where MachineId=LineId and IsActive=1 order by MachineId )
    Line,
	
    ISNULL([1],0) AS January,
    ISNULL([2],0) AS February,
    ISNULL([3],0) AS March,
    ISNULL([4],0) AS April,
    ISNULL([5],0) AS May,
    ISNULL([6],0) AS June,
    ISNULL([7],0) AS July,
    ISNULL([8],0) AS August,
     ISNULL([9],0) AS September,
    ISNULL([10],0) AS October,
    ISNULL([11],0) AS November,
     ISNULL([12],0) AS December
FROM
(Select 
S.LineId,S.Qty,MONTH(S.OutputDate) as TMonth from
    (    select SO.LineId ,SUM(ISNULL(SOD.Quantity,0)) as Qty,  SO.OutputDate  from PROD_SewingOutPutProcess as SO
inner join PROD_SewingOutPutProcessDetail as SOD on SO.SewingOutPutProcessId=SOD.SewingOutPutProcessId
where Year(SO.OutputDate)=@CurrntYear and SO.CompId=@CompId
group by SO.LineId ,SO.OutputDate
) as S )  source
PIVOT
(
    SUM(Qty)
    FOR TMonth
    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
) AS pvtMonth

order by pvtMonth.LineId

--exec SpProdLineWiseMonthlySewingProduction 2017,'001'
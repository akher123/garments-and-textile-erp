CREATE procedure SpDayMonthWiseCutting
@YearId int,
@CompId varchar(3)
as 
SELECT 
    [Month/Day], 
	ISNULL([1],0)+ISNULL([2],0)+ISNULL([3],0)+ ISNULL([4],0)+ ISNULL([5],0)+ ISNULL([6],0)+ ISNULL([7],0)+ ISNULL([8],0)+ISNULL([9],0)+ISNULL( [10],0)+ 
    ISNULL([11],0)+ ISNULL([12],0)+ ISNULL([13],0)+ ISNULL([14],0)+ ISNULL([15],0)+ ISNULL([16],0)+ ISNULL([17],0)+ ISNULL([18],0)+ ISNULL([19],0)+ ISNULL([20],0)+ 
    ISNULL([21],0)+ ISNULL([22],0)+ ISNULL([23],0)+ISNULL([24],0)+ ISNULL([25],0)+ ISNULL([26],0)+ ISNULL([27],0)+ ISNULL([28],0)+ ISNULL([29],0)+ ISNULL([30],0)+ ISNULL([31],0) as Total,
    [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], 
    [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], 
    [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31]
FROM 
    (select DATENAME(month,CB.CuttingDate) as [Month/Day],MONTH(CB.CuttingDate) as Mth,day( CB.CuttingDate) as CuttingDate, SUM(BC.Quantity) as CuttQty from PROD_CuttingBatch as CB 
inner join PROD_BundleCutting  as BC on CB.CuttingBatchId=BC.CuttingBatchId
where  Year(CB.CuttingDate)=@YearId and CB.ComponentRefId='001' and CB.CompId=@CompId

group by  CB.CuttingDate

) As tmpProducts
PIVOT (
    SUM(tmpProducts.CuttQty)
    FOR tmpProducts.CuttingDate In
       ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], 
        [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], 
        [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31])
    ) as piv

	order by Mth

--exec SpDayMonthWiseCutting 2017,'001'
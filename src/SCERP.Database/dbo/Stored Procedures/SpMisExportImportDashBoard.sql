CREATE procedure [dbo].[SpMisExportImportDashBoard]
as
Delete From MIS_DashBoard where  isnull((M01 + M02 + M03 + M04 + M05 + M06 + M07 + M08 + M09 + M10 + M11 + M12),0) = 0;

SELECT        
OM_Buyer.BuyerName as Buyer, DataType, M01 as [	Jan-20 ], M02 as [Feb-20 ], M03 as [Mar-20], M04 as [Apr-20 ],	M05 as [May-20]	,  M06 as [Jun-20]	
, M07 as [Jul-20] ,M08 as [Aug-20] ,M09 as [Sep-20],M10 as [Oct-20 ] 	,  M11 as [Nov-20] ,  M12 as [	Dec-20 ]  	 

FROM    MIS_DashBoard
inner join OM_Buyer on MIS_DashBoard.BuyerRefId=OM_Buyer.BuyerRefId
where OM_Buyer.CompId='001'
ORDER BY OM_Buyer.BuyerName, MIS_DashBoard.DataType

--SELECT * FROM    MIS_DashBoard

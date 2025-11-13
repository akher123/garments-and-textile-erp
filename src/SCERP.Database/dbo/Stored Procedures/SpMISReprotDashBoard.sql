CREATE procedure [dbo].[SpMISReprotDashBoard]
as

Delete From MIS_DashBoard where BuyerRefId in ( SELECT  BuyerRefId
FROM            MIS_DashBoard as AA
WHERE        (M01 + M02 + M03 + M04 + M05 + M06 + M07 + M08 + M09 + M10 + M11 + M12 = 0) AND (DataType = '1') );

UPDATE    MIS_DashBoard
SET              BuyerName = ISNULL
                          ((SELECT     TOP (1) BuyerName
                              FROM         OM_Buyer
                              WHERE     (CompId = '001') AND (BuyerRefId = MIS_DashBoard.BuyerRefId)), '');

INSERT INTO MIS_DashBoard
                      (BuyerRefId, DataType, M01, M02, M03, M04, M05, M06, M07, M08, M09, M10, M11, M12, BuyerName)
SELECT     '999' AS BuyerRefId, DataType, ISNULL(SUM(M01), 0) AS M01, ISNULL(SUM(M02), 0) AS M02, ISNULL(SUM(M03), 0) AS M03, ISNULL(SUM(M04), 0) AS M04, 
                      ISNULL(SUM(M05), 0) AS M05, ISNULL(SUM(M06), 0) AS M06, ISNULL(SUM(M07), 0) AS M07, ISNULL(SUM(M08), 0) AS M08, ISNULL(SUM(M09), 0) AS M09, 
                      ISNULL(SUM(M10), 0) AS M10, ISNULL(SUM(M11), 0) AS M11, ISNULL(SUM(M12), 0) AS M12, 'Summary' AS BuyerName
FROM         MIS_DashBoard AS MIS_DashBoard_1
GROUP BY DataType;

--201800, 6 
SELECT        
BuyerName as Buyer, DataType,

M01 as [Jan-20], 
M02 as [Feb-20], 
M03 as [Mar-20],	
M04 as [Apr-20],	
M05 as [May-20], 
M06 as [Jun-20]	,
M07 as [Jul-20],
M08 as [Aug-20],
M09 as [Sep-20],
M10 as [Oct-20], 
M11 as [Nov-20], 
M12 as [Dec-20]   	 

--OM_Buyer.BuyerName as Buyer, DataType, M01 ,
--M02  ,M03 ,M04 ,M05  	,  M06  ,  M07  , M08 , M09 , M10 , M11 ,	M12 	  	 	 

FROM    MIS_DashBoard

ORDER BY BuyerRefId, BuyerName, DataType



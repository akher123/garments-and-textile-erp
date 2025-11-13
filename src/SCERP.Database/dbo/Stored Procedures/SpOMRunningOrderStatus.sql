create procedure SpOMRunningOrderStatus
@CompId varchar(3)
as 

SELECT     B.BuyerName, SUM(S.Quantity) AS OQty, ISNULL(SUM(S.Amt), 0) AS OAmt, ISNULL(SUM(S.despatchQty), 0) AS SQty, ISNULL(SUM(S.despatchQty * S.Rate), 0) 
                      AS SAmt
FROM         OM_BuyOrdStyle AS S INNER JOIN
                      OM_BuyerOrder AS O ON S.CompId = O.CompId AND S.OrderNo = O.OrderNo INNER JOIN
                      OM_Buyer AS B ON O.CompId = B.CompId AND O.BuyerRefId = B.BuyerRefId
WHERE     (S.CompId = @CompId) AND (S.ActiveStatus =1) 
GROUP BY B.BuyerName  
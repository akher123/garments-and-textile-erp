CREATE PROCEDURE [dbo].[SpProdBatchReport] 
 @BatchId bigint,
 @CompId VARCHAR(3)
AS
BEGIN
SELECT B.BtRefNo AS BatchRefNo,B.BatchNo,B.BatchDate,P.Name AS PartyName,M.Name AS MachineName,B.BtType,BD.RollQty as RollQy, BD.FLength as RLength, 
(select GroupName from PROD_GroupSubProcess where GroupSubProcessId=B.GroupSubProcessId) as ProcessName,BD.StLength as StLength,
(select Name from Employee  where EmployeeId=B.CreatedBy) as CreatorName,
B.Gsm AS OrderNo,
BYR.BuyerName,BO.RefNo AS OrderName,S.StyleName,C.ColorName,
B.BatchQty,B.ShadePerc,B.CostRate,B.BillRate,B.ApprovedLdNo,B.Remarks,B.LoadingDateTime,B.UnLoadingDateTime,
I.ItemName,CMP.ComponentName,Fd.SizeName as FdiaSizeName,md.SizeName as MdiaSizeName,BD.GSM,BD.Quantity,BD.Remarks
FROM Pro_Batch AS B
LEFT JOIN Party AS P ON B.PartyId=P.PartyId AND B.CompId=P.CompId
LEFT JOIN Production_Machine AS M ON B.MachineId=M.MachineId AND B.CompId=P.CompId
LEFT JOIN OM_Buyer AS BYR ON B.BuyerRefId=BYR.BuyerRefId AND B.CompId=BYR.CompId
LEFT JOIN OM_BuyerOrder AS BO ON B.OrderNo=BO.OrderNo AND B.CompId=BO.CompId
LEFT JOIN VOMBuyOrdStyle AS S ON B.OrderStyleRefId=S.OrderStyleRefId AND B.CompId=S.CompId
LEFT JOIN OM_Color AS C ON B.GrColorRefId=C.ColorRefId AND B.CompId=C.CompId
LEFT JOIN PROD_BatchDetail AS BD ON B.BatchId=BD.BatchId AND B.CompId=BD.CompId
LEFT JOIN Inventory_Item AS I ON BD.ItemId=I.ItemId AND BD.CompId=I.CompId
left JOIN OM_Component AS CMP ON BD.ComponentRefId=CMP.ComponentRefId AND BD.CompId=CMP.CompId
left  JOIN OM_Size AS Fd ON BD.FdiaSizeRefId=Fd.SizeRefId and  BD.CompId=Fd.CompId
left  JOIN OM_Size AS md ON BD.MdiaSizeRefId=md.SizeRefId and  BD.CompId=md.CompId
Where B.BatchId=@BatchId AND B.CompId=@CompId
END




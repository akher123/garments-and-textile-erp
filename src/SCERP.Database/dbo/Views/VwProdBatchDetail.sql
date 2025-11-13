
CREATE VIEW [dbo].[VwProdBatchDetail]
AS
SELECT BD.BatchDetailId,BD.CompId,BD.BatchId,BD.ItemId,
BD.ComponentRefId,
Bd.MdiaSizeRefId,
BD.FdiaSizeRefId,
BD.GSM,
BD.Quantity,
BD.Remarks,
BD.Rate ,
I.ItemName,
C.ComponentName,
Fd.SizeName as FdiaSizeName,
md.SizeName as MdiaSizeName ,
BD.RollQty ,BD.FLength , 
BD.StLength 
 FROM PROD_BatchDetail AS BD
inner JOIN Inventory_Item AS I
ON BD.ItemId=I.ItemId AND BD.CompId=I.CompId
inner JOIN OM_Component AS C
ON BD.ComponentRefId=C.ComponentRefId AND BD.CompId=C.CompId
left  JOIN OM_Size AS Fd ON BD.FdiaSizeRefId=Fd.SizeRefId and  BD.CompId=Fd.CompId
left  JOIN OM_Size AS md ON BD.MdiaSizeRefId=md.SizeRefId and  BD.CompId=md.CompId




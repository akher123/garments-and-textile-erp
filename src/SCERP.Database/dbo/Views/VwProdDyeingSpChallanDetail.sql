
CREATE VIEW [dbo].[VwProdDyeingSpChallanDetail]
AS
SELECT DD.*,B.BatchNo,BD.ItemId,I.ItemName,GSP.GroupName FROM PROD_DyeingSpChallanDetail AS DD
LEFT JOIN Pro_Batch AS B ON DD.BatchId=B.BatchId AND DD.CompId=B.CompId
LEFT JOIN PROD_BatchDetail AS BD ON DD.BatchDetailId=BD.BatchDetailId AND DD.CompId=BD.CompId
LEFT JOIN Inventory_Item AS I ON BD.ItemId=I.ItemId AND BD.CompId=I.CompId
LEFT JOIN PROD_GroupSubProcess AS GSP ON DD.SpGroupId=GSP.GroupSubProcessId AND DD.CompId=GSP.CompId





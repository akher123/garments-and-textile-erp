create view VwCuttFabReject
as
select CFR.*,B.BtRefNo,B.BatchNo, B.BuyerName,B.OrderName,B.StyleName,B.GColorName as ColorName,BD.ItemName,BD.GSM,BD.FdiaSizeName,BD.MdiaSizeName from PROD_CuttFabReject as CFR
inner join VProBatch as B on CFR.BatchId=B.BatchId
inner join VwProdBatchDetail as BD on BD.BatchDetailId=CFR.BatchDetailId




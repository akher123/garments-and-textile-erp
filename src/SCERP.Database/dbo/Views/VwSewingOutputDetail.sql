CREATE VIEW VwSewingOutputDetail
AS
Select *, IsNull((select Sum(Quantity) AS Quantity From PROD_SewingOutPutProcessDetail where SizeRefId=VwCutBank.SizeRefId and SewingOutPutProcessId IN(select SewingOutPutProcessId from PROD_SewingOutPutProcess where OrderStyleRefId= VwCutBank.OrderStyleRefId AND ColorRefId= VwCutBank.ColorRefId AND CompId= VwCutBank.CompId)
GROUP BY SizeRefId),0) as OutputQuantity from VwCutBank



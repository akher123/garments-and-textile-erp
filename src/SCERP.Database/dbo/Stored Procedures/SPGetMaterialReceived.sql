
CREATE PROCEDURE [dbo].[SPGetMaterialReceived] 
(
@FromDate Datetime,
@ToDate Datetime,
@ChallanNo VARCHAR(6),
@RegisterType VARCHAR(20),
@CompId VARCHAR(3)
)
AS
BEGIN
SELECT 
 MR.MaterialReceivedRefId,  
  MR.CompId, 
  MR.GRN, 
  MR.GEN,
  MR.ReceivedDate,
  MR.ChallanNo,
  MR.ChallanDate,
  MR.SupplierName, 
  IIF((MR.RegisterType='YARNREGISTER' or MR.RegisterType='GENERALREGISTER'  or MR.RegisterType='FINISHFABRIC'),MRD.BuyerNameDtl,MR.BuyerName) as BuyerName,
  IIF((MR.RegisterType='YARNREGISTER' or MR.RegisterType='GENERALREGISTER'  or MR.RegisterType='FINISHFABRIC'),MRD.OrderNameDtl,  MR.OrderNo) as OrderNo,
 IIF((MR.RegisterType='YARNREGISTER' or MR.RegisterType='GENERALREGISTER'  or MR.RegisterType='FINISHFABRIC'),MRD.StyleNameDtl,  MR.StyleNo) as StyleNo,
  MRD.Item AS [Description],
  MRD.Color, 
  MRD.Size,
  MR.Article,
  MRD.LotNo,
  MRD.UnitName, 
  MRD.ReceivedQty, 
  MR.LCNo, 
  MRD.Rate, 
  MRD.TotalAmount,
  MRD.Brand,
  MR.BillStatus,
  MR.Remarks,
  MR.RegisterType
FROM  Inventory_MaterialReceived AS MR
INNER JOIN Inventory_MaterialReceivedDetail AS MRD
on MR.MaterialReceivedId=MRD.MaterialReceivedId AND MR.CompId=MRD.CompId
where Convert(date,MR.ReceivedDate)>=Convert(date,@FromDate ) AND Convert(date,MR.ReceivedDate) <=Convert(date,@ToDate) AND (MR.RegisterType=@RegisterType OR @RegisterType='') AND MR.CompId=@CompId AND (MR.ChallanNo=@ChallanNo OR @ChallanNo='') 
order by MR.MaterialReceivedRefId
END




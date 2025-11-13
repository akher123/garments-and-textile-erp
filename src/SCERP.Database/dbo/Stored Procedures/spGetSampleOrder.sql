CREATE procedure [dbo].[spGetSampleOrder]
@SampleOrderId int ,
@CompId char(3)
as

SELECT 
C.Name AS CompanyName,
C.FullAddress AS ComapnyAddres,
C.ImagePath ,
 SO.RefId as OrderRefId,
 (select EmpName from OM_Merchandiser where MerchandiserId=SO.MerchandiserId) as Merchandiser,
 (select BuyerName from OM_Buyer where BuyerId=SO.BuyerId) as BuyerName,
 SO.Agent,
 SO.OrderNo,
 SO.OrderDate,
 SO.OrderQty,
 SO.Season ,
 SO.ApprovedDate ,
 SO.Remarks,
 (select Name from Employee where EmployeeId=SO.CreatedBy) as CreatedEmployee,
 ISNULL((select Name from Employee where EmployeeId=SO.ApprovedBy),'Not Approved') as ApprovedByEmployee ,

 SS.StyleNo,
 SS.StyleQty,
 SS.SampleDate,
 SS.ItemName,
 SS.SampleType,
 SS.EFDate,
 SS.Fabrication,
 SS.Gsm,
 SS.ColorName,
 SS.SizeName,
 SS.FabQty,
  SS.FinishDia,
	  SS.RibFab,
	    SS.RibQty,
		  SS.ContasFab,
		      SS.ContasQty
 FROM OM_SampleOrder AS SO

inner join OM_SampleStyle AS SS ON SO.SampleOrderId=SS.SampleOrderId
INNER JOIN Company AS C ON SO.CompId=C.CompanyRefId
where SO.SampleOrderId=@SampleOrderId and SO.CompId=@CompId